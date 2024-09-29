using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Domain.Entities;
using Domain.ValueObjects;
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

            var adminAddUserToGroupRequest = new AdminAddUserToGroupRequest
            {
                GroupName = "cliente",
                Username = cliente.Email,
                UserPoolId = _userPoolId
            };

            return await CriarUsuarioCognitoAsync(signUpRequest, adminAddUserToGroupRequest, cliente.Email, cancellationToken);
        }

        public async Task<bool> ConfirmarEmailVerificacaoAsync(EmailVerificacao emailVerificacao, CancellationToken cancellationToken)
        {
            var confirmSignUpRequest = new ConfirmSignUpRequest
            {
                ClientId = _clientId,
                Username = emailVerificacao.Email,
                ConfirmationCode = emailVerificacao.CodigoVerificacao,
                SecretHash = CalcularSecretHash(_clientId, _clientSecret, emailVerificacao.Email)
            };

            try
            {
                var response = await _cognitoClientIdentityProvider.ConfirmSignUpAsync(confirmSignUpRequest, cancellationToken);

                return response is not null && response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SolicitarRecuperacaoSenhaAsync(RecuperacaoSenha recuperacaoSenha, CancellationToken cancellationToken)
        {
            var forgotPasswordRequest = new ForgotPasswordRequest
            {
                ClientId = _clientId,
                Username = recuperacaoSenha.Email,
                SecretHash = CalcularSecretHash(_clientId, _clientSecret, recuperacaoSenha.Email)
            };

            try
            {
                var response = await _cognitoClientIdentityProvider.ForgotPasswordAsync(forgotPasswordRequest, cancellationToken);

                return response is not null && response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EfetuarResetSenhaAsync(ResetSenha resetSenha, CancellationToken cancellationToken)
        {
            var confirmForgotPasswordRequest = new ConfirmForgotPasswordRequest
            {
                ClientId = _clientId,
                Username = resetSenha.Email,
                ConfirmationCode = resetSenha.CodigoVerificacao,
                Password = resetSenha.NovaSenha,
                SecretHash = CalcularSecretHash(_clientId, _clientSecret, resetSenha.Email)
            };

            try
            {
                var response = await _cognitoClientIdentityProvider.ConfirmForgotPasswordAsync(confirmForgotPasswordRequest, cancellationToken);

                return response is not null && response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CriarUsuarioFuncionarioAsync(Funcionario funcionario, string senha, CancellationToken cancellationToken)
        {
            if (await VerificarSeEmailExisteAsync(funcionario.Email, cancellationToken))
            {
                return false;
            }

            var signUpRequest = new SignUpRequest
            {
                ClientId = _clientId,
                SecretHash = CalcularSecretHash(_clientId, _clientSecret, funcionario.Email),
                Username = funcionario.Email,
                Password = senha,
                UserAttributes =
                [
                    new() { Name = "email", Value = funcionario.Email },
                    new() { Name = "name", Value = funcionario.Nome }
                ]
            };

            var adminAddUserToGroupRequest = new AdminAddUserToGroupRequest
            {
                GroupName = "admin",
                Username = funcionario.Email,
                UserPoolId = _userPoolId
            };

            return await CriarUsuarioCognitoAsync(signUpRequest, adminAddUserToGroupRequest, funcionario.Email, cancellationToken);
        }

        public async Task<TokenUsuario?> IdentifiqueSeAsync(string? email, string? cpf, string senha, CancellationToken cancellationToken)
        {
            var userPool = new CognitoUserPool(_userPoolId, _clientId, _cognitoClientIdentityProvider);

            var userId = string.Empty;

            if (email is not null || cpf is not null)
            {
                if (email is not null)
                {
                    userId = await ObertUsuarioCognitoPorEmailAsync(email, cancellationToken);
                }

                if (cpf is not null)
                {
                    userId = await ObterUserIdPorCpfAsync(cpf, cancellationToken);

                }

                if (!string.IsNullOrEmpty(userId))
                {
                    var cognitoUser = new CognitoUser(userId, _clientId, userPool, _cognitoClientIdentityProvider, _clientSecret);

                    var authRequest = new InitiateSrpAuthRequest
                    {
                        Password = senha
                    };

                    try
                    {
                        var respose = await cognitoUser.StartWithSrpAuthAsync(authRequest, cancellationToken);

                        if (respose is null || respose.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED)
                        {
                            return null;
                        }

                        var timeSpan = TimeSpan.FromSeconds(respose.AuthenticationResult.ExpiresIn);
                        var expiry = DateTimeOffset.UtcNow + timeSpan;

                        return new()
                        {
                            Email = email,
                            Cpf = cpf,
                            AccessToken = respose.AuthenticationResult.AccessToken,
                            RefreshToken = respose.AuthenticationResult.RefreshToken,
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

                return null;
            }

            return null;
        }

        private async Task<bool> CriarUsuarioCognitoAsync(SignUpRequest signUpRequest, AdminAddUserToGroupRequest addToGroupRequest, string email, CancellationToken cancellationToken)
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

                    await DeletarUsuarioCognitoAsync(email, cancellationToken);
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeletarUsuarioCognitoAsync(string email, CancellationToken cancellationToken)
        {
            try
            {
                var username = await ObertUsuarioCognitoPorEmailAsync(email, cancellationToken);

                if (username == null)
                {
                    return false;
                }

                var adminDeleteUserRequest = new AdminDeleteUserRequest
                {
                    UserPoolId = _userPoolId,
                    Username = username
                };

                await _cognitoClientIdentityProvider.AdminDeleteUserAsync(adminDeleteUserRequest, cancellationToken);

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
            var usuarios = await ObterTodosUsuariosCognitoAsync(cancellationToken);

            var usuariosComCpf = usuarios.FirstOrDefault(usuario =>
                usuario.Attributes.Any(attribute =>
                    attribute.Name == "custom:cpf" && attribute.Value == cpf));

            return usuariosComCpf != null;
        }

        private async Task<string> ObterUserIdPorCpfAsync(string cpf, CancellationToken cancellationToken)
        {
            var usuarios = await ObterTodosUsuariosCognitoAsync(cancellationToken);

            var usuariosComCpf = usuarios.FirstOrDefault(usuario =>
                    usuario.Attributes.Any(attribute =>
                        attribute.Name == "custom:cpf" && attribute.Value == cpf));

            if (usuariosComCpf == null)
            {
                return string.Empty;
            }

            var emailAttribute = usuariosComCpf.Attributes.FirstOrDefault(attr => attr.Name == "email");
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
                var usuarios = new List<UserType>();
                string? paginationToken = null;

                do
                {
                    var request = new ListUsersRequest
                    {
                        UserPoolId = _userPoolId,
                        PaginationToken = paginationToken
                    };

                    var response = await _cognitoClientIdentityProvider.ListUsersAsync(request, cancellationToken);
                    usuarios.AddRange(response.Users);
                    paginationToken = response.PaginationToken;
                }
                while (!string.IsNullOrEmpty(paginationToken));

                return usuarios;
            }
            catch (Exception ex)
            {
                throw new HttpRequestException(ex.Message);
            }
        }

        private async Task<string?> ObertUsuarioCognitoPorEmailAsync(string email, CancellationToken cancellationToken)
        {
            var request = new ListUsersRequest
            {
                UserPoolId = _userPoolId,
                Filter = $"email = \"{email}\""
            };

            var response = await _cognitoClientIdentityProvider.ListUsersAsync(request, cancellationToken);

            var usuario = response.Users.FirstOrDefault();
            return usuario?.Username;
        }
    }
}