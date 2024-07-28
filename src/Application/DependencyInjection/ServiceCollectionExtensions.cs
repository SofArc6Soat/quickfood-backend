using Infra.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace UseCases.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddUseCasesDependencyServices(this IServiceCollection services, string connectionString)
        {
            services.AddInfraDependencyServices(connectionString);

            services.AddScoped<IProdutoUseCase, ProdutoUseCase>();
            services.AddScoped<IClienteUseCase, ClienteUseCase>();
            services.AddScoped<IPedidoUseCase, PedidoUseCase>();
            services.AddScoped<IPagamentoUseCase, PagamentoUseCase>();
        }
    }
}