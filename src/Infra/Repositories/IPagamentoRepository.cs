using Core.Domain.Data;
using Infra.Dto;

namespace Infra.Repositories
{
    public interface IPagamentoRepository : IRepositoryGeneric<PagamentoDto>
    {
        Task<string> ObterPagamentoPorPedidoAsync(Guid pedidoId, CancellationToken cancellation);
    }
}
