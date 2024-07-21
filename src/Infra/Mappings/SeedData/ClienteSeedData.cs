using Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Mappings.SeedData
{
    [ExcludeFromCodeCoverage]
    public static class ClienteSeedData
    {
        public static List<Cliente> GetSeedData() =>
        [
            new Cliente(Guid.Parse("efee2d79-ce89-479a-9667-04f57f9e2e5e"), "João", "joao@gmail.com", "08062759016", true),
            new Cliente(Guid.Parse("fdff63d2-127f-49c5-854a-e47cae8cedb9"), "Maria", "maria@gmail.com", "05827307084", true)
        ];
    }
}