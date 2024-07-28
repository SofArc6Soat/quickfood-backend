using Core.Domain.Data;
using Infra.Dto;

namespace Infra.Repositories
{
    public interface IProdutoRepository : IRepositoryGeneric<ProdutoDto>
    {
        Task<IEnumerable<ProdutoDto>> ObterTodosProdutosAsync(CancellationToken cancellationToken);
        Task<IEnumerable<ProdutoDto>> ObterProdutosCategoriaAsync(string categoria, CancellationToken cancellationToken);
    }
}
