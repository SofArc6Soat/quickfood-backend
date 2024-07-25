using Application.UseCases;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Application.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationDependencyServices(this IServiceCollection services)
        {
            services.AddTransient<IProdutoUseCase, ProdutoUseCase>();
            services.AddTransient<IClienteUseCase, ClienteUseCase>();
            services.AddTransient<IPedidoUseCase, PedidoUseCase>();
            services.AddTransient<IPagamentoUseCase, PagamentoUseCase>();
        }
    }
}