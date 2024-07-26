﻿using Domain.Entities;
using Domain.ValueObjects;
using UseCases.Models.Request;

namespace UseCases.UseCases
{
    public interface IProdutoUseCase
    {
        Task<IEnumerable<Produto>> ObterTodosProdutosAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Produto>> ObterProdutosCategoriaAsync(Categoria categoria, CancellationToken cancellationToken);
        Task<bool> CadastrarProdutoAsync(ProdutoRequest request, CancellationToken cancellationToken);
        Task<bool> AtualizarProdutoAsync(ProdutoRequest request, CancellationToken cancellationToken);
        Task<bool> DeletarProdutoAsync(Guid id, CancellationToken cancellationToken);
    }
}
