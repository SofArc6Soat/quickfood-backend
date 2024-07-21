namespace Api.Configuration
{
    public static class EnvironmentConfig
    {
        public static Settings ConfigureEnvironment(IConfiguration configuration)
        {
            var settings = new Settings();
            ConfigurationBinder.Bind(configuration, settings);
            return settings;
        }
    }

    public record Settings
    {
#pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.
        public ConnectionStrings ConnectionStrings { get; set; }
#pragma warning restore CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.
    }

    public record ConnectionStrings
    {
        public required string DefaultConnection { get; set; }
    }
}
