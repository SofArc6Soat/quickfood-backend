﻿using Domain.Entities;
using Domain.ValueObjects;
using Infra.Dto;
using Infra.Repositories;

namespace Gateways
{
    public class ProdutoGateway(IProdutoRepository produtoRepository) : IProdutoGateway
    {
        public async Task<bool> CadastrarProdutoAsync(Produto produto, CancellationToken cancellationToken)
        {
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
            await produtoRepository.DeleteAsync(id, cancellationToken);

            return await produtoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }


        public bool VerificarProdutoExistente(Guid id, string nome, string descricao, CancellationToken cancellationToken)
        {
            var produtoExistente = produtoRepository.Find(e => e.Id == id || e.Nome == nome || e.Descricao == descricao, cancellationToken).FirstOrDefault(g => g.Id == id);

            return produtoExistente is not null;
        }

        public async Task<bool> VerificarProdutoExistenteAsync(Guid id, CancellationToken cancellationToken)
        {
            var produtoExistente = await produtoRepository.FindByIdAsync(id, cancellationToken);

            return produtoExistente is not null;
        }

        public async Task<Produto?> ObterProdutoAsync(Guid id, CancellationToken cancellationToken)
        {
            var produtoDto = await produtoRepository.FindByIdAsync(id, cancellationToken);

            if (produtoDto is null)
            {
                return null;
            }

            _ = Enum.TryParse(produtoDto.Categoria, out Categoria categoria);
            return new Produto(produtoDto.Id, produtoDto.Nome, produtoDto.Descricao, produtoDto.Preco, categoria, produtoDto.Ativo);
        }
    }
}
