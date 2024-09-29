using System.Diagnostics.CodeAnalysis;

namespace Gateways.Configurations
{
    [ExcludeFromCodeCoverage]
    public class Cognito(string clientId, string clientSecret, string userPoolId) : ICognito
    {
        public string ClientId { get; set; } = clientId;
        public string ClientSecret { get; set; } = clientSecret;
        public string UserPoolId { get; set; } = userPoolId;
    }

    public interface ICognito
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string UserPoolId { get; set; }
    }
}