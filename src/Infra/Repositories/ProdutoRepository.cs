using Cora.Infra.Repository;
using Infra.Context;
using Infra.Dto;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class ProdutoRepository(ApplicationDbContext context) : RepositoryGeneric<ProdutoDto>(context), IProdutoRepository
    {
        private readonly DbSet<ProdutoDto> _produtos = context.Set<ProdutoDto>();

        public async Task<IEnumerable<ProdutoDto>> ObterTodosProdutosAsync(CancellationToken cancellationToken) =>
            await _produtos.AsNoTracking().Where(p => p.Ativo).ToListAsync(cancellationToken);

        public async Task<IEnumerable<ProdutoDto>> ObterProdutosCategoriaAsync(string categoria, CancellationToken cancellationToken) =>
            await _produtos.AsNoTracking().Where(p => p.Ativo && p.Categoria == categoria).ToListAsync(cancellationToken);
    }
}
