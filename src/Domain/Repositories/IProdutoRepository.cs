using Core.Domain.Data;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Repositories
{
    public interface IProdutoRepository : IRepositoryGeneric<Produto>
    {
        Task<IEnumerable<Produto>> ObterTodosProdutosAsync();
        Task<IEnumerable<Produto>> ObterProdutosCategoriaAsync(Categoria categoria);
    }
}
