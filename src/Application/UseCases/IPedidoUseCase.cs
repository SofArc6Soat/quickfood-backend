using Domain.Entities;
using Domain.ValueObjects;
using UseCases.Models.Request;

namespace UseCases.UseCases
{
    public interface IPedidoUseCase
    {
        Task<IEnumerable<Pedido>> ObterTodosPedidosAsync(CancellationToken cancellationToken);
        Task<string> ObterTodosPedidosOrdenadosAsync(CancellationToken cancellationToken);
        Task<bool> CadastrarPedidoAsync(PedidoRequest request, CancellationToken cancellationToken);
        Task<bool> AlterarStatusAsync(Guid pedidoId, PedidoStatus pedidoStatus, CancellationToken cancellationToken);
    }
}
