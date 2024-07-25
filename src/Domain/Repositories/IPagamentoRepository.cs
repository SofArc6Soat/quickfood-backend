using Core.Domain.Data;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IPagamentoRepository : IRepositoryGeneric<Pagamento>
    {
        Task<string> ObterPagamentoPorPedidoAsync(Guid pedidoId, CancellationToken cancellation);
    }
}
