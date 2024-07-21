using Core.Domain.Data;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IClienteRepository : IRepositoryGeneric<Cliente>
    {
        Task<IEnumerable<Cliente>> ObterTodosClientesAsync();

        Task<Cliente?> IdentificarClienteCpfAsync(string cpf, CancellationToken cancellationToken);
    }
}
