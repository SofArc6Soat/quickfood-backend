using Application.Models.Request;
using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;

namespace Application.UseCases
{
    public class ProdutoUseCase(IProdutoRepository produtoRepository, INotificador notificador) : BaseUseCase(notificador), IProdutoUseCase
    {
        public async Task<bool> CadastrarProdutoAsync(ProdutoRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var produtoExistente = produtoRepository.Find(e => e.Id == request.Id || e.Nome == request.Nome || e.Descricao == request.Descricao).FirstOrDefault(g => g.Id == request.Id);

            if (produtoExistente is not null)
            {
                Notificar("Produto já existente");
                return false;
            }

            var produto = new Produto(request.Id, request.Nome, request.Descricao, request.Preco, request.Categoria, request.Ativo);

            if (!ExecutarValidacao(new ValidarProduto(), produto))
            {
                return false;
            }

            await produtoRepository.InsertAsync(produto, cancellationToken);

            return await produtoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<bool> AtualizarProdutoAsync(ProdutoRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var produtoExistente = await produtoRepository.FindByIdAsync(request.Id, cancellationToken);

            if (produtoExistente is null)
            {
                Notificar("Produto inexistente");
                return false;
            }

            var produto = new Produto(request.Id, request.Nome, request.Descricao, request.Preco, request.Categoria, request.Ativo);

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
