using Api.Configuration;
using Application.DependencyInjection;
using Core.WebApi.DependencyInjection;
using Infra.Context;
using Infra.DependencyInjection;

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

            services.AddApplicationDependencyServices();

            services.AddInfraDependencyServices(settings.ConnectionStrings.DefaultConnection);

        }

        public void Configure(IApplicationBuilder app, ApplicationDbContext context)
        {
            DatabaseMigratorBase.MigrateDatabase(context);

            app.UseApiDefautConfig();
        }
    }
}