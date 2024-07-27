using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;

namespace UseCases
{
    public class ProdutoUseCase(IProdutoRepository produtoRepository, INotificador notificador) : BaseUseCase(notificador), IProdutoUseCase
    {
        public async Task<bool> CadastrarProdutoAsync(Produto produto, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(produto);

            var produtoExistente = produtoRepository.Find(e => e.Id == produto.Id || e.Nome == produto.Nome || e.Descricao == produto.Descricao).FirstOrDefault(g => g.Id == produto.Id);

            if (produtoExistente is not null)
            {
                Notificar("Produto já existente");
                return false;
            }

            if (!ExecutarValidacao(new ValidarProduto(), produto))
            {
                return false;
            }

            await produtoRepository.InsertAsync(produto, cancellationToken);

            return await produtoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<bool> AtualizarProdutoAsync(Produto produto, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(produto);

            var produtoExistente = await produtoRepository.FindByIdAsync(produto.Id, cancellationToken);

            if (produtoExistente is null)
            {
                Notificar("Produto inexistente");
                return false;
            }

            if (!ExecutarValidacao(new ValidarProduto(), produto))
            {
                return false;
            }

            await produtoRepository.UpdateAsync(produto, cancellationToken);

            return await produtoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<bool> DeletarProdutoAsync(Guid id, CancellationToken cancellationToken)
        {
            var produtoExistente = await produtoRepository.FindByIdAsync(id, cancellationToken);

            if (produtoExistente is null)
            {
                Notificar("Produto inexistente");
                return false;
            }

            await produtoRepository.DeleteAsync(id, cancellationToken);

            return await produtoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<IEnumerable<Produto>> ObterTodosProdutosAsync(CancellationToken cancellationToken) =>
            await produtoRepository.ObterTodosProdutosAsync();

        public async Task<IEnumerable<Produto>> ObterProdutosCategoriaAsync(Categoria categoria, CancellationToken cancellationToken) =>
            await produtoRepository.ObterProdutosCategoriaAsync(categoria);
    }
}
