using Domain.Entities;
using Gateways.Dtos.Request;
using UseCases;

namespace Controllers
{
    public class ClientesController(IClienteUseCase clienteUseCase) : IClientesController
    {
        public async Task<bool> CadastrarClienteAsync(ClienteDto clienteDto, CancellationToken cancellationToken)
        {
            var cliente = new Cliente(clienteDto.Id, clienteDto.Nome, clienteDto.Email, clienteDto.Cpf, clienteDto.Ativo);

            return await clienteUseCase.CadastrarClienteAsync(cliente, cancellationToken);
        }

        public async Task<bool> AtualizarClienteAsync(ClienteDto clienteDto, CancellationToken cancellationToken)
        {
            var cliente = new Cliente(clienteDto.Id, clienteDto.Nome, clienteDto.Email, clienteDto.Cpf, clienteDto.Ativo);

            return await clienteUseCase.AtualizarClienteAsync(cliente, cancellationToken);
        }

        public async Task<bool> DeletarClienteAsync(Guid id, CancellationToken cancellationToken) =>
            await clienteUseCase.DeletarClienteAsync(id, cancellationToken);

        public async Task<IEnumerable<Cliente>> ObterTodosClientesAsync(CancellationToken cancellationToken) =>
            await clienteUseCase.ObterTodosClientesAsync(cancellationToken);

        public async Task<Cliente?> IdentificarClienteCpfAsync(string cfp, CancellationToken cancellationToken) =>
            await clienteUseCase.IdentificarClienteCpfAsync(cfp, cancellationToken);
    }
}
