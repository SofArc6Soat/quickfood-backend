using Infra.Dto;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Mappings.SeedData
{
    [ExcludeFromCodeCoverage]
    public static class ClienteSeedData
    {
        public static List<ClienteDto> GetSeedData() =>
        [
            new ClienteDto(Guid.Parse("efee2d79-ce89-479a-9667-04f57f9e2e5e"), "João", "joao@gmail.com", "08062759016", true),
            new ClienteDto(Guid.Parse("fdff63d2-127f-49c5-854a-e47cae8cedb9"), "Maria", "maria@gmail.com", "05827307084", true)
        ];
    }
}