using Application.Models.Request;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.UseCases
{
    public interface IPedidoUseCase
    {
        Task<IEnumerable<Pedido>> ObterTodosPedidosAsync(CancellationToken cancellationToken);
        Task<bool> CadastrarPedidoAsync(PedidoRequest request, CancellationToken cancellationToken);
        Task<bool> EfetuarCheckoutAsync(Guid pedidoId, CancellationToken cancellationToken);
        Task<bool> AlterarStatusAsync(Guid pedidoId, PedidoStatus pedidoStatus, CancellationToken cancellationToken);
    }
}
