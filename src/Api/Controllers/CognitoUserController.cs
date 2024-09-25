using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Api.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Api.Controllers
{
    [Route("cognito")]
    public class CognitoUserController : ControllerBase
    {
        private readonly IAmazonCognitoIdentityProvider _cognitoClientIdentityProvider;
        private readonly ICognitoSettings _cognitoSettings;

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _userPoolId;

        public CognitoUserController(IAmazonCognitoIdentityProvider cognitoClientIdentityProvider, ICognitoSettings cognitoSettings)
        {
            _cognitoClientIdentityProvider = cognitoClientIdentityProvider;
            _cognitoSettings = cognitoSettings;

            _clientId = _cognitoSettings.ClientId;
            _clientSecret = _cognitoSettings.ClientSecret;
            _userPoolId = _cognitoSettings.UserPoolId;
        }


        [HttpPost("ConfirmEmailVerification")]
        public async Task<IActionResult> ConfirmEmailVerification([FromBody] ConfirmEmailModel model)
        {
            var request = new ConfirmSignUpRequest
            {
                ClientId = _clientId,
                Username = model.Email,
                ConfirmationCode = model.CodigoVerificacao,
                SecretHash = CalculateSecretHash(_clientId, _clientSecret, model.Email)
            };

            try
            {
                var response = await _cognitoClientIdentityProvider.ConfirmSignUpAsync(request);
                return Ok(new { message = "Email verificado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var request = new ForgotPasswordRequest
            {
                ClientId = _clientId,
                Username = model.Email,
                SecretHash = CalculateSecretHash(_clientId, _clientSecret, model.Email)
            };

            try
            {
                var response = await _cognitoClientIdentityProvider.ForgotPasswordAsync(request);
                return Ok(new { message = "Código de recuperação de senha enviado para o email." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("ConfirmForgotPassword")]
        public async Task<IActionResult> ConfirmForgotPassword([FromBody] ConfirmForgotPasswordModel model)
        {
            var request = new ConfirmForgotPasswordRequest
            {
                ClientId = _clientId,
                Username = model.Email,
                ConfirmationCode = model.CodigoVerificacao,
                Password = model.NovaSenha,
                SecretHash = CalculateSecretHash(_clientId, _clientSecret, model.Email)
            };

            try
            {
                var response = await _cognitoClientIdentityProvider.ConfirmForgotPasswordAsync(request);
                return Ok(new { message = "Senha redefinida com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var userPool = new CognitoUserPool(_userPoolId, _clientId, _cognitoClientIdentityProvider);

            var user = new CognitoUser(model.Email, _clientId, userPool, _cognitoClientIdentityProvider, _clientSecret);

            var authRequest = new InitiateSrpAuthRequest
            {
                Password = model.Senha
            };

            try
            {
                var authResponse = await user.StartWithSrpAuthAsync(authRequest);

                if (authResponse.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED)
                {
                    return BadRequest(new { message = "Atualize sua senha temporaria" });
                }

                var timeSpan = TimeSpan.FromSeconds(authResponse.AuthenticationResult.ExpiresIn);
                var expiry = DateTimeOffset.UtcNow + timeSpan;

                return Ok(new
                {
                    authResponse.AuthenticationResult.RefreshToken,
                    authResponse.AuthenticationResult.AccessToken,
                    Expiry = expiry
                });
            }
            catch (NotAuthorizedException)
            {
                return Unauthorized(new { message = "Credenciais inválidas." });
            }
            catch (UserNotConfirmedException)
            {
                return BadRequest(new { message = "Usuário não confirmado. Por favor, verifique seu e-mail." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private async Task<bool> CheckIfEmailExistsAsync(string email)
        {
            var request = new ListUsersRequest
            {
                UserPoolId = _userPoolId,
                Filter = $"email = \"{email}\""
            };

            var response = await _cognitoClientIdentityProvider.ListUsersAsync(request);
            return response.Users.Count != 0;
        }

        private static string CalculateSecretHash(string clientId, string clientSecret, string username)
        {
            var keyBytes = Encoding.UTF8.GetBytes(clientSecret);
            var messageBytes = Encoding.UTF8.GetBytes(username + clientId);

            using var hmac = new HMACSHA256(keyBytes);
            var hash = hmac.ComputeHash(messageBytes);
            return Convert.ToBase64String(hash);
        }
    }

    public record UserModel
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [PasswordPropertyText]
        public required string Senha { get; set; }
    }

    public record ConfirmEmailModel
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [Length(6, 6, ErrorMessage = "O campo {0} deve conter {1} caracteres.")]
        public required string CodigoVerificacao { get; set; }
    }

    public record ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }

    public record ConfirmForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [Length(6, 6, ErrorMessage = "O campo {0} deve conter {1} caracteres.")]
        public required string CodigoVerificacao { get; set; }

        [Required]
        [PasswordPropertyText]
        public required string NovaSenha { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [PasswordPropertyText]
        public required string Senha { get; set; }
    }
}