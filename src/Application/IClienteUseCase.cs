using Domain.Entities;

namespace UseCases
{
    public interface IClienteUseCase
    {
        Task<IEnumerable<Cliente>> ObterTodosClientesAsync(CancellationToken cancellationToken);
        Task<Cliente?> IdentificarClienteCpfAsync(string cpf, CancellationToken cancellationToken);
        Task<bool> CadastrarClienteAsync(Cliente cliente, CancellationToken cancellationToken);
        Task<bool> AtualizarClienteAsync(Cliente cliente, CancellationToken cancellationToken);
        Task<bool> DeletarClienteAsync(Guid id, CancellationToken cancellationToken);
    }
}