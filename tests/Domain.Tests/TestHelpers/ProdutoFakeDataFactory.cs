using Domain.Entities;
using Domain.ValueObjects;
using Infra.Dto;

namespace Domain.Tests.TestHelpers;

public static class ProdutoFakeDataFactory
{
    public static Produto CriarProdutoValido()
    {
        return new Produto(Guid.NewGuid(), "Produto Exemplo", "Descrição do Produto", 100.00m, Categoria.Lanche, true);
    }

    public static ProdutoDb CriarProdutoDbValido()
    {
        return new ProdutoDb
        {
            Id = Guid.NewGuid(),
            Nome = "Produto Exemplo",
            Descricao = "Descrição do Produto",
            Preco = 100.00m,
            Categoria = Categoria.Lanche.ToString(),
            Ativo = true
        };
    }
}