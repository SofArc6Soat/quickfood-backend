using Cora.Infra.Repository;
using Dapper;
using Infra.Context;
using Infra.Dto;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class PedidoRepository(ApplicationDbContext context) : RepositoryGeneric<PedidoDto>(context), IPedidoRepository
    {
        private readonly DbSet<PedidoDto> _pedidos = context.Set<PedidoDto>();

        public async Task<IEnumerable<PedidoDto>> ObterTodosPedidosAsync() =>
            await _pedidos.AsNoTracking().AsSplitQuery().Include(p => p.PedidoItems).OrderBy(p => p.NumeroPedido).ToListAsync();

        public async Task<string> ObterTodosPedidosOrdenadosAsync(CancellationToken cancellationToken)
        {
            var query = @"
                SELECT CONVERT(VARCHAR(MAX),(
                    (SELECT p.Id, p.NumeroPedido, p.Status, p.ValorTotal, p.DataCriacao
                    FROM Pedidos p
                    WHERE Status IN ('Pronto', 'EmPreparacao', 'Recebido')
                    ORDER BY 
                        CASE Status
                            WHEN 'Pronto' THEN 1
                            WHEN 'EmPreparacao' THEN 2
                            WHEN 'Recebido' THEN 3
                        END,
                        DataCriacao ASC
                    FOR JSON PATH)
                ));";

            var result = await GetDbConnection().QueryFirstOrDefaultAsync<string>(query);

            return !string.IsNullOrEmpty(result) ? result : "[]";
        }
    }
}
