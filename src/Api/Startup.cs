using Api.Configuration;
using Controllers.DependencyInjection;
using Core.WebApi.DependencyInjection;
using Gateways.DependencyInjection;
using Infra.Context;

namespace Api
{
    public class Startup
    {
        public IConfiguration _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var settings = EnvironmentConfig.ConfigureEnvironment(_configuration);

            services.AddApiDefautConfig();

            services.AddHealthCheckConfig(settings.ConnectionStrings.DefaultConnection);

            services.AddControllerDependencyServices();
            services.AddGatewayDependencyServices(settings.ConnectionStrings.DefaultConnection);
        }

        public void Configure(IApplicationBuilder app, ApplicationDbContext context)
        {
            DatabaseMigratorBase.MigrateDatabase(context);

            app.UseApiDefautConfig();
        }
    }
}