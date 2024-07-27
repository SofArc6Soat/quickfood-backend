using Cora.Infra.Repository;
using Infra.Context;
using Infra.Dto;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class ProdutoRepository(ApplicationDbContext context) : RepositoryGeneric<ProdutoDto>(context), IProdutoRepository
    {
        private readonly DbSet<ProdutoDto> _produtos = context.Set<ProdutoDto>();

        public async Task<IEnumerable<ProdutoDto>> ObterTodosProdutosAsync() =>
            await _produtos.AsNoTracking().Where(p => p.Ativo).ToListAsync();

        public async Task<IEnumerable<ProdutoDto>> ObterProdutosCategoriaAsync(string categoria) =>
            await _produtos.AsNoTracking().Where(p => p.Ativo && p.Categoria == categoria).ToListAsync();
    }
}
