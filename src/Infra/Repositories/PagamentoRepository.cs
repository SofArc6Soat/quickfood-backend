using Cora.Infra.Repository;
using Dapper;
using Domain.Entities;
using Domain.Repositories;
using Infra.Context;

namespace Infra.Repositories
{
    public class PagamentoRepository(ApplicationDbContext context) : RepositoryGeneric<Pagamento>(context), IPagamentoRepository
    {
        public async Task<string> ObterPagamentoPorPedidoAsync(Guid pedidoId, CancellationToken cancellationToken)
        {
            var query = @"
                SELECT pedidoId, status, valor, dataCriacao AS dataPagamento
                FROM Pagamentos
                WHERE PedidoId = @vPedidoId
                FOR JSON PATH";

            var result = await GetDbConnection().QueryFirstOrDefaultAsync<string>(query, new { vPedidoId = pedidoId });

            return !string.IsNullOrEmpty(result) ? result : "[]";
        }
    }
}