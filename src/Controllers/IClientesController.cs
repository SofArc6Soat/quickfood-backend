using Domain.Entities;
using Gateways.Dtos.Request;
using Gateways.Dtos.Response;

namespace Controllers
{
    public interface IClientesController
    {
        Task<IEnumerable<Cliente>> ObterTodosClientesAsync(CancellationToken cancellationToken);
        Task<TokenUsuario> IdentificarClienteCpfAsync(string cfp, string senha, CancellationToken cancellationToken);
        Task<bool> CadastrarClienteAsync(ClienteRequestDto clienteRequestDto, CancellationToken cancellationToken);
        Task<bool> AtualizarClienteAsync(ClienteRequestDto clienteRequestDto, CancellationToken cancellationToken);
        Task<bool> DeletarClienteAsync(Guid id, CancellationToken cancellationToken);
    }
}
