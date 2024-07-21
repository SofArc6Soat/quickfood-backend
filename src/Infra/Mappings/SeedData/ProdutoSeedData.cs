using Domain.Entities;
using Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Mappings.SeedData
{
    [ExcludeFromCodeCoverage]
    public static class ProdutoSeedData
    {
        public static List<Produto> GetSeedData() =>
        [
            new Produto(Guid.Parse("efee2d79-ce89-479a-9667-04f57f9e2e5e"), "X-SALADA", "Pão brioche, hambúrguer (150g), queijo prato, pepino, tomate italiano e alface americana.", 30, Categoria.Lanche, true),
            new Produto(Guid.Parse("fdff63d2-127f-49c5-854a-e47cae8cedb9"), "X-BACON", "Pão brioche, hambúrguer (150g), queijo prato, bacon, pepino, tomate italiano e alface americana.", 33, Categoria.Lanche, true),
            new Produto(Guid.Parse("eee57eb1-1dde-4162-998f-d7148d0c2417"), "X-BURGUER", "Pão brioche, hambúrguer (150g) e queijo prato.", 28, Categoria.Lanche, true),
            new Produto(Guid.Parse("719bc73f-b90a-4bb0-b6d0-8060ea9f1d4c"), "X-DUPLO BACON", "Pão smash, 2 hambúrgueres (150g cada), maionese do feio ,2 queijos cheddar e muito bacon.", 36, Categoria.Lanche, true),

            new Produto(Guid.Parse("50ba333a-c804-4d2a-a284-9ff1d147df4e"), "BATATA FRITA", "Porção individual de batata frita (100g)", 9, Categoria.Acompanhamento, true),
            new Produto(Guid.Parse("1bb2aef8-97d7-4fb0-94f5-53bff2f3a618"), "ONION RINGS", "Anéis de cebola (100g)", 10, Categoria.Acompanhamento, true),

            new Produto(Guid.Parse("111cb598-2df6-41bf-b51b-d4e0f292bda3"), "PEPSI LATA", "350ml", 7, Categoria.Bebida, true),
            new Produto(Guid.Parse("c0eab3dc-2ddf-4dde-a64f-392f2412201f"), "GUARANÁ ANTARCTICA LATA", "350ml", 7, Categoria.Bebida, true),
            new Produto(Guid.Parse("3de0c5e7-787b-4885-8ec8-020866971d3b"), "ÁGUA", "500ml", 5, Categoria.Bebida, true),

            new Produto(Guid.Parse("b17f425e-e0ff-41cd-92a6-00d78172d7a5"), "BROWNIE CHOCOLATE", "70g", 10, Categoria.Sobremesa, true),
            new Produto(Guid.Parse("e206c795-d6d6-491e-90ed-fdc08e057939"), "BROWNIE CHOCOLATE BRANCO", "70g", 10, Categoria.Sobremesa, true),
            new Produto(Guid.Parse("c398d290-d1a1-4f2e-a907-ef55e92beef6"), "SORVETE DE CHOCOLATE", "100g", 12, Categoria.Sobremesa, true),
            new Produto(Guid.Parse("782725ea-70a5-49db-95b2-c4eea841ebca"), "SORVETE DE CREME", "100g", 12, Categoria.Sobremesa, true)
        ];
    }
}