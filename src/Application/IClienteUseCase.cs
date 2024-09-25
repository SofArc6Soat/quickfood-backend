using Domain.Entities;
using Gateways.Dtos.Response;

namespace UseCases
{
    public interface IClienteUseCase
    {
        Task<IEnumerable<Cliente>> ObterTodosClientesAsync(CancellationToken cancellationToken);
        Task<TokenUsuario> IdentificarClienteCpfAsync(string cpf, string senha, CancellationToken cancellationToken);
        Task<bool> CadastrarClienteAsync(Cliente cliente, string senha, CancellationToken cancellationToken);
        Task<bool> AtualizarClienteAsync(Cliente cliente, CancellationToken cancellationToken);
        Task<bool> DeletarClienteAsync(Guid id, CancellationToken cancellationToken);
    }
}