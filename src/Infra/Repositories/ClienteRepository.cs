using Cora.Infra.Repository;
using Infra.Context;
using Infra.Dto;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class ClienteRepository(ApplicationDbContext context) : RepositoryGeneric<ClienteDto>(context), IClienteRepository
    {
        private readonly DbSet<ClienteDto> _clientes = context.Set<ClienteDto>();

        public async Task<ClienteDto?> IdentificarClienteCpfAsync(string cpf, CancellationToken cancellationToken) =>
            await _clientes.AsNoTracking().Where(p => p.Cpf == cpf).FirstOrDefaultAsync(cancellationToken);

        public async Task<IEnumerable<ClienteDto>> ObterTodosClientesAsync() =>
            await _clientes.AsNoTracking().Where(p => p.Ativo).ToListAsync();
    }
}
