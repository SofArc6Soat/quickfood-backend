﻿using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using UseCases.DependencyInjection;

namespace Controllers.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddControllerDependencyServices(this IServiceCollection services)
        {
            services.AddScoped<IClientesController, ClientesController>();
            services.AddScoped<IProdutosController, ProdutosController>();
            services.AddScoped<IPedidoController, PedidoController>();
            services.AddScoped<IPagamentoController, PagamentoController>();
            services.AddScoped<IUsuariosController, UsuariosController>();

            services.AddUseCasesDependencyServices();
        }
    }
}