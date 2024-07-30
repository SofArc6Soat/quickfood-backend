using Domain.Entities;
using Gateways.Dtos.Request;

namespace Controllers
{
    public interface IClientesController
    {
        Task<IEnumerable<Cliente>> ObterTodosClientesAsync(CancellationToken cancellationToken);
        Task<Cliente?> IdentificarClienteCpfAsync(string cfp, CancellationToken cancellationToken);
        Task<bool> CadastrarClienteAsync(ClienteRequestDto clienteRequestDto, CancellationToken cancellationToken);
        Task<bool> AtualizarClienteAsync(ClienteRequestDto clienteRequestDto, CancellationToken cancellationToken);
        Task<bool> DeletarClienteAsync(Guid id, CancellationToken cancellationToken);
    }
}
