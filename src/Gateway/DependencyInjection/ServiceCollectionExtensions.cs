using Gateways.Configurations;
using Infra.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Gateways.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddGatewayDependencyServices(this IServiceCollection services, string connectionString, string clientId, string clientSecret, string userPoolId)
        {
            services.AddScoped<IClienteGateway, ClienteGateway>();
            services.AddScoped<IProdutoGateway, ProdutoGateway>();
            services.AddScoped<IPedidoGateway, PedidoGateway>();
            services.AddScoped<IPagamentoGateway, PagamentoGateway>();
            services.AddScoped<IFuncionarioGateway, FuncionarioGateway>();

            services.AddSingleton<ICognito>(new Cognito(clientId, clientSecret, userPoolId));
            services.AddScoped<ICognitoGateway, CognitoGateway>();

            services.AddInfraDependencyServices(connectionString);
        }
    }
}