using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Domain.ValueObjects;
using Gateways;
using Infra.Repositories;

namespace UseCases
{
    public class ProdutoUseCase(IProdutoGateway produtoGateway, IProdutoRepository produtoRepository, INotificador notificador) : BaseUseCase(notificador), IProdutoUseCase
    {
        public async Task<bool> CadastrarProdutoAsync(Produto produto, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(produto);

            if (produtoGateway.VerificarProdutoExistente(produto.Id, produto.Nome, produto.Descricao, cancellationToken))
            {
                Notificar("Produto já existente");
                return false;
            }

            return ExecutarValidacao(new ValidarProduto(), produto)
                   && await produtoGateway.CadastrarProdutoAsync(produto, cancellationToken);
        }

        public async Task<bool> AtualizarProdutoAsync(Produto produto, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(produto);

            if (!await produtoGateway.VerificarProdutoExistenteAsync(produto.Id, cancellationToken))
            {
                Notificar("Produto inexistente");
                return false;
            }

            return ExecutarValidacao(new ValidarProduto(), produto)
                   && await produtoGateway.AtualizarProdutoAsync(produto, cancellationToken);
        }

        public async Task<bool> DeletarProdutoAsync(Guid id, CancellationToken cancellationToken)
        {
            if (!await produtoGateway.VerificarProdutoExistenteAsync(id, cancellationToken))
            {
                Notificar("Produto inexistente");

                return false;
            }

            return await produtoGateway.DeletarProdutoAsync(id, cancellationToken);
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
