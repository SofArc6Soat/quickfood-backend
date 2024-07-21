using Cora.Infra.Repository;
using Domain.Entities;
using Domain.Repositories;
using Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class PedidoRepository(ApplicationDbContext context) : RepositoryGeneric<Pedido>(context), IPedidoRepository
    {
        private readonly DbSet<Pedido> _pedidos = context.Set<Pedido>();

        public async Task<IEnumerable<Pedido>> ObterTodosPedidosAsync() =>
            await _pedidos.AsNoTracking().AsSplitQuery().Include(p => p.PedidoItems).OrderBy(p => p.NumeroPedido).ToListAsync();
    }
}
