namespace Gateways.Configurations
{
    public class Cognito : ICognito
    {
        public Cognito(string clientId, string clientSecret, string userPoolId)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            UserPoolId = userPoolId;
        }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string UserPoolId { get; set; }
    }

    public interface ICognito
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string UserPoolId { get; set; }
    }
}