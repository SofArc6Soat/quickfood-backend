using Controllers.Dtos.Request;
using Domain.Entities;

namespace Controllers
{
    public interface IPedidoController
    {
        Task<IEnumerable<Pedido>> ObterTodosPedidosAsync(CancellationToken cancellationToken);
        Task<string> ObterTodosPedidosOrdenadosAsync(CancellationToken cancellationToken);
        Task<bool> CadastrarPedidoAsync(PedidoDto pedidoDto, CancellationToken cancellationToken);
        Task<bool> AlterarStatusAsync(Guid pedidoId, PedidoStatusDto pedidoStatusDto, CancellationToken cancellationToken);
    }
}
