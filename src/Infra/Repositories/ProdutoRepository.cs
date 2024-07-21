using Cora.Infra.Repository;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class ProdutoRepository(ApplicationDbContext context) : RepositoryGeneric<Produto>(context), IProdutoRepository
    {
        private readonly DbSet<Produto> _produtos = context.Set<Produto>();

        public async Task<IEnumerable<Produto>> ObterTodosProdutosAsync() =>
            await _produtos.AsNoTracking().Where(p => p.Ativo).ToListAsync();

        public async Task<IEnumerable<Produto>> ObterProdutosCategoriaAsync(Categoria categoria) =>
            await _produtos.AsNoTracking().Where(p => p.Ativo && p.Categoria == categoria).ToListAsync();
    }
}
