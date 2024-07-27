using Core.Domain.Data;
using Infra.Dto;

namespace Infra.Repositories
{
    public interface IClienteRepository : IRepositoryGeneric<ClienteDto>
    {
        Task<IEnumerable<ClienteDto>> ObterTodosClientesAsync();

        Task<ClienteDto?> IdentificarClienteCpfAsync(string cpf, CancellationToken cancellationToken);
    }
}
