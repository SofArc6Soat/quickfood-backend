using Domain.Entities;
using UseCases.Models.Request;

namespace UseCases.UseCases
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
