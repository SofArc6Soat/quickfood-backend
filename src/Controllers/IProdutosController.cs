using Domain.Entities;
using Gateways.Dtos.Request;

namespace Controllers
{
    public interface IProdutosController
    {
        Task<IEnumerable<Produto>> ObterTodosProdutosAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Produto>> ObterProdutosCategoriaAsync(string categoria, CancellationToken cancellationToken);
        Task<bool> CadastrarProdutoAsync(ProdutoDto produtoDto, CancellationToken cancellationToken);
        Task<bool> AtualizarProdutoAsync(ProdutoDto produtoDto, CancellationToken cancellationToken);
        Task<bool> DeletarProdutoAsync(Guid id, CancellationToken cancellationToken);
    }
}
