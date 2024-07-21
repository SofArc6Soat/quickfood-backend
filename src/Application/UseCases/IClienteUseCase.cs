using Application.Models.Request;
using Domain.Entities;

namespace Application.UseCases
{
    public interface IClienteUseCase
    {
        Task<IEnumerable<Cliente>> ObterTodosClientesAsync(CancellationToken cancellationToken);
        Task<Cliente?> IdentificarClienteCpfAsync(IdentifiqueSeRequest request, CancellationToken cancellationToken);
        Task<bool> CadastrarClienteAsync(ClienteRequest request, CancellationToken cancellationToken);
        Task<bool> AtualizarClienteAsync(ClienteRequest request, CancellationToken cancellationToken);
        Task<bool> DeletarClienteAsync(Guid id, CancellationToken cancellationToken);
    }
}
