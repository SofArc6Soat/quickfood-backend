namespace Api.Configuration
{
    public static class EnvironmentConfig
    {
        public static Settings ConfigureEnvironment(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new Settings();
            ConfigurationBinder.Bind(configuration, settings);

            services.AddSingleton<ICognitoSettings>(settings.CognitoSettings);

            return settings;
        }
    }

    public record Settings
    {
        public ConnectionStrings ConnectionStrings { get; set; } = new ConnectionStrings();

        public CognitoSettings CognitoSettings { get; set; } = new CognitoSettings();
    }

    public record ConnectionStrings
    {
        public string DefaultConnection { get; set; } = string.Empty;
    }

    public class CognitoSettings : ICognitoSettings
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string UserPoolId { get; set; } = string.Empty;
    }

    public interface ICognitoSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string UserPoolId { get; set; }
    }
}