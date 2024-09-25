using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Domain.Entities;
using Gateways.Configurations;
using Gateways.Dtos.Response;
using System.Security.Cryptography;
using System.Text;

namespace Gateways
{
    public class CognitoGateway : ICognitoGateway
    {
        private readonly IAmazonCognitoIdentityProvider _cognitoClientIdentityProvider;

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _userPoolId;

        public CognitoGateway(IAmazonCognitoIdentityProvider cognitoClientIdentityProvider, ICognito cognitoSettings)
        {
            _cognitoClientIdentityProvider = cognitoClientIdentityProvider;

            var _cognitoSettings = cognitoSettings;

            _clientId = _cognitoSettings.ClientId;
            _clientSecret = _cognitoSettings.ClientSecret;
            _userPoolId = _cognitoSettings.UserPoolId;
        }

        public async Task<bool> CriarUsuarioClienteAsync(Cliente cliente, string senha, CancellationToken cancellationToken)
        {
            if (await VerificarSeCpfExisteAsync(cliente.Cpf, cancellationToken) && await VerificarSeEmailExisteAsync(cliente.Email, cancellationToken))
            {
                return false;
            }

            var signUpRequest = new SignUpRequest
            {
                ClientId = _clientId,
                SecretHash = CalcularSecretHash(_clientId, _clientSecret, cliente.Email),
                Username = cliente.Email,
                Password = senha,
                UserAttributes =
                [
                    new() { Name = "email", Value = cliente.Email },
                    new() { Name = "name", Value = cliente.Nome },
                    new() { Name = "custom:cpf", Value = cliente.Cpf }
                ]
            };

            var addToGroupRequest = new AdminAddUserToGroupRequest
            {
                GroupName = "cliente",
                Username = cliente.Email,
                UserPoolId = _userPoolId
            };

            return await CriarUsuarioCognito(signUpRequest, addToGroupRequest, cliente.Email, cancellationToken);
        }

        public async Task<bool> CriarUsuarioAsync(Usuario usuario, string senha, CancellationToken cancellationToken)
        {
            if (await VerificarSeEmailExisteAsync(usuario.Email, cancellationToken))
            {
                return false;
            }

            var signUpRequest = new SignUpRequest
            {
                ClientId = _clientId,
                SecretHash = CalcularSecretHash(_clientId, _clientSecret, usuario.Email),
                Username = usuario.Email,
                Password = senha,
                UserAttributes =
                [
                    new() { Name = "email", Value = usuario.Email },
                    new() { Name = "name", Value = usuario.Nome }
                ]
            };

            var addToGroupRequest = new AdminAddUserToGroupRequest
            {
                GroupName = "admin",
                Username = usuario.Email,
                UserPoolId = _userPoolId
            };

            return await CriarUsuarioCognito(signUpRequest, addToGroupRequest, usuario.Email, cancellationToken);
        }

        public async Task<TokenUsuario> IdentificarClientePorCpfAsync(string cpf, string senha, CancellationToken cancellationToken)
        {
            var userPool = new CognitoUserPool(_userPoolId, _clientId, _cognitoClientIdentityProvider);

            var userId = await ObterUserIdPorCpfAsync(cpf, cancellationToken);

            var user = new CognitoUser(userId, _clientId, userPool, _cognitoClientIdentityProvider, _clientSecret);

            var authRequest = new InitiateSrpAuthRequest
            {
                Password = senha
            };

            try
            {
                var authResponse = await user.StartWithSrpAuthAsync(authRequest, cancellationToken);

                if (authResponse.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED)
                {
                    return null;
                }

                var timeSpan = TimeSpan.FromSeconds(authResponse.AuthenticationResult.ExpiresIn);
                var expiry = DateTimeOffset.UtcNow + timeSpan;

                return new()
                {
                    RefreshToken = authResponse.AuthenticationResult.RefreshToken,
                    AccessToken = authResponse.AuthenticationResult.AccessToken,
                    Expiry = expiry
                };
            }
            catch (NotAuthorizedException)
            {
                throw new NotAuthorizedException("Credenciais inválidas.");
            }
            catch (UserNotConfirmedException)
            {
                throw new UserNotConfirmedException("Usuário não confirmado. Por favor, verifique seu e-mail.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<bool> CriarUsuarioCognito(SignUpRequest signUpRequest, AdminAddUserToGroupRequest addToGroupRequest, string email, CancellationToken cancellationToken)
        {
            try
            {
                var signUpResponse = await _cognitoClientIdentityProvider.SignUpAsync(signUpRequest, cancellationToken);

                if (signUpResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    var addToGroupResponse = await _cognitoClientIdentityProvider.AdminAddUserToGroupAsync(addToGroupRequest, cancellationToken);

                    if (addToGroupResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return true;
                    }

                    await DeletarUsuarioCognito(email, cancellationToken);
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeletarUsuarioCognito(string email, CancellationToken cancellationToken)
        {
            try
            {
                var username = await ObertUsuarioCognitoPorEmailAsync(email, cancellationToken);

                if (username == null)
                {
                    return false;
                }

                var deleteUserRequest = new AdminDeleteUserRequest
                {
                    UserPoolId = _userPoolId,
                    Username = username
                };

                await _cognitoClientIdentityProvider.AdminDeleteUserAsync(deleteUserRequest, cancellationToken);

                return true;
            }
            catch (UserNotFoundException)
            {
                return false;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> VerificarSeEmailExisteAsync(string email, CancellationToken cancellationToken)
        {
            try
            {
                var request = new ListUsersRequest
                {
                    UserPoolId = _userPoolId,
                    Filter = $"email = \"{email}\""
                };

                var response = await _cognitoClientIdentityProvider.ListUsersAsync(request, cancellationToken);
                return response.Users.Count != 0;
            }
            catch (Exception ex)
            {
                throw new HttpRequestException(ex.Message);
            }
        }

        private async Task<bool> VerificarSeCpfExisteAsync(string cpf, CancellationToken cancellationToken)
        {
            var users = await ObterTodosUsuariosCognitoAsync(cancellationToken);

            var userWithCpf = users.FirstOrDefault(user =>
                user.Attributes.Any(attribute =>
                    attribute.Name == "custom:cpf" && attribute.Value == cpf));

            return userWithCpf != null;
        }

        private async Task<string> ObterUserIdPorCpfAsync(string cpf, CancellationToken cancellationToken)
        {
            var users = await ObterTodosUsuariosCognitoAsync(cancellationToken);

            var userWithCpf = users.FirstOrDefault(user =>
                    user.Attributes.Any(attribute =>
                        attribute.Name == "custom:cpf" && attribute.Value == cpf));

            if (userWithCpf == null)
            {
                return string.Empty;
            }

            var emailAttribute = userWithCpf.Attributes.FirstOrDefault(attr => attr.Name == "email");
            return emailAttribute?.Value;
        }

        private static string CalcularSecretHash(string clientId, string clientSecret, string nomeUsuario)
        {
            var keyBytes = Encoding.UTF8.GetBytes(clientSecret);
            var messageBytes = Encoding.UTF8.GetBytes(nomeUsuario + clientId);

            using var hmac = new HMACSHA256(keyBytes);
            var hash = hmac.ComputeHash(messageBytes);
            return Convert.ToBase64String(hash);
        }

        private async Task<List<UserType>> ObterTodosUsuariosCognitoAsync(CancellationToken cancellationToken)
        {
            try
            {
                var users = new List<UserType>();
                string? paginationToken = null;

                do
                {
                    var request = new ListUsersRequest
                    {
                        UserPoolId = _userPoolId,
                        PaginationToken = paginationToken
                    };

                    var response = await _cognitoClientIdentityProvider.ListUsersAsync(request, cancellationToken);
                    users.AddRange(response.Users);
                    paginationToken = response.PaginationToken;
                }
                while (!string.IsNullOrEmpty(paginationToken));

                return users;
            }
            catch (Exception ex)
            {
                throw new HttpRequestException(ex.Message);
            }
        }

        private async Task<string> ObertUsuarioCognitoPorEmailAsync(string email, CancellationToken cancellationToken)
        {
            var request = new ListUsersRequest
            {
                UserPoolId = _userPoolId,
                Filter = $"email = \"{email}\""
            };

            var response = await _cognitoClientIdentityProvider.ListUsersAsync(request, cancellationToken);

            var user = response.Users.FirstOrDefault();
            return user?.Username;
        }
    }
}