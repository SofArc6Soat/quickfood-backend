﻿using Controllers.Dtos.Request;

namespace Controllers
{
    public interface IPedidoController
    {
        Task<string> ObterTodosPedidosAsync(CancellationToken cancellationToken);
        Task<string> ObterTodosPedidosOrdenadosAsync(CancellationToken cancellationToken);
        Task<bool> CadastrarPedidoAsync(PedidoDto pedidoDto, CancellationToken cancellationToken);
        Task<bool> AlterarStatusAsync(Guid pedidoId, PedidoStatusDto pedidoStatusDto, CancellationToken cancellationToken);
    }
}
