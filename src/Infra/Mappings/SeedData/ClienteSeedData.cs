using Infra.Dto;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Mappings.SeedData
{
    [ExcludeFromCodeCoverage]
    public static class ClienteSeedData
    {
        public static List<ClienteDto> GetSeedData() =>
        [
            new ClienteDto {
                Id = Guid.Parse("efee2d79-ce89-479a-9667-04f57f9e2e5e"),
                Nome = "João",
                Email = "joao@gmail.com",
                Cpf = "08062759016",
                Ativo = true
            },
            new ClienteDto {
                Id = Guid.Parse("fdff63d2-127f-49c5-854a-e47cae8cedb9"),
                Nome = "Maria",
                Email = "maria@gmail.com",
                Cpf = "05827307084",
                Ativo = true
            }
        ];
    }
}