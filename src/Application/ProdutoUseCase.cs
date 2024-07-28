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

            var produtoDtoExistente = produtoRepository.Find(e => e.Id == produto.Id || e.Nome == produto.Nome || e.Descricao == produto.Descricao).FirstOrDefault(g => g.Id == produto.Id);

            if (produtoDtoExistente is not null)
            {
                Notificar("Produto já existente");
                return false;
            }

            if (!ExecutarValidacao(new ValidarProduto(), produto))
            {
                return false;
            }

            var produtoDto = new ProdutoDto
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Categoria = produto.Categoria.ToString(),
                Ativo = produto.Ativo
            };

            await produtoRepository.InsertAsync(produtoDto, cancellationToken);

            return await produtoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<bool> AtualizarProdutoAsync(Produto produto, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(produto);

            var produtoDtoExistente = await produtoRepository.FindByIdAsync(produto.Id, cancellationToken);

            if (produtoDtoExistente is null)
            {
                Notificar("Produto inexistente");
                return false;
            }

            if (!ExecutarValidacao(new ValidarProduto(), produto))
            {
                return false;
            }

            var produtoDto = new ProdutoDto
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Categoria = produto.Categoria.ToString(),
                Ativo = produto.Ativo
            };

            await produtoRepository.UpdateAsync(produtoDto, cancellationToken);

            return await produtoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<bool> DeletarProdutoAsync(Guid id, CancellationToken cancellationToken)
        {
            var produtoDtoExistente = await produtoRepository.FindByIdAsync(id, cancellationToken);

            if (produtoDtoExistente is null)
            {
                Notificar("Produto inexistente");
                return false;
            }

            await produtoRepository.DeleteAsync(id, cancellationToken);

            return await produtoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<IEnumerable<Produto>> ObterTodosProdutosAsync(CancellationToken cancellationToken)
        {
            var produtoDto = await produtoRepository.ObterTodosProdutosAsync(cancellationToken);

            if (produtoDto.Any())
            {
                var produto = new List<Produto>();
                foreach (var item in produtoDto)
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
            var produtoDto = await produtoRepository.ObterProdutosCategoriaAsync(categoria.ToString(), cancellationToken);

            if (produtoDto.Any())
            {
                var produto = new List<Produto>();
                foreach (var item in produtoDto)
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
