using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Domain.ValueObjects;
using Infra.Dto;
using Infra.Repositories;

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

            var dto = new ProdutoDto(produto.Id, produto.Nome, produto.Descricao, produto.Preco, produto.Categoria.ToString(), produto.Ativo);

            await produtoRepository.InsertAsync(dto, cancellationToken);

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

            var dto = new ProdutoDto(produto.Id, produto.Nome, produto.Descricao, produto.Preco, produto.Categoria.ToString(), produto.Ativo);

            await produtoRepository.UpdateAsync(dto, cancellationToken);

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

        public async Task<IEnumerable<Produto>> ObterTodosProdutosAsync(CancellationToken cancellationToken)
        {
            var dto = await produtoRepository.ObterTodosProdutosAsync();

            if (dto.Any())
            {
                var produto = new List<Produto>();
                foreach (var item in dto)
                {
                    _ = Enum.TryParse(item.Categoria, out Categoria produtoCategoria);

                    produto.Add(new Produto(item.Id, item.Nome, item.Descricao, item.Preco, produtoCategoria, item.Ativo));
                }

                return produto;
            }

            return [];
        }

        public async Task<IEnumerable<Produto>> ObterProdutosCategoriaAsync(Categoria categoria, CancellationToken cancellationToken)
        {
            var dto = await produtoRepository.ObterProdutosCategoriaAsync(categoria.ToString());

            if (dto.Any())
            {
                var produto = new List<Produto>();
                foreach (var item in dto)
                {
                    _ = Enum.TryParse(item.Categoria, out Categoria produtoCategoria);

                    produto.Add(new Produto(item.Id, item.Nome, item.Descricao, item.Preco, produtoCategoria, item.Ativo));
                }

                return produto;
            }

            return [];
        }
    }
}
