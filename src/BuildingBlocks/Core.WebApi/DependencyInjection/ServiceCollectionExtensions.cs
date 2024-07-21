using Core.Domain.Notificacoes;
using Core.WebApi.Configurations;
using Core.WebApi.GlobalErrorMiddleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.WebApi.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApiDefautConfig(this IServiceCollection services)
        {
            services.AddScoped<INotificador, Notificador>();

            services.AddControllers().AddJsonOptions(delegate (JsonOptions options)
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

            });

            services.AddSwaggerConfig();

        }

        public static void UseApiDefautConfig(this IApplicationBuilder app)
        {
            app.UseApplicationErrorMiddleware();

            app.UseSwaggerConfig();

            app.UseHsts();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}