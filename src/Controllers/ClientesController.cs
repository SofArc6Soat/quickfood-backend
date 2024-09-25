using Domain.Entities;
using Gateways.Dtos.Request;
using Gateways.Dtos.Response;
using UseCases;

namespace Controllers
{
    public class ClientesController(IClienteUseCase clienteUseCase) : IClientesController
    {
        public async Task<bool> CadastrarClienteAsync(ClienteRequestDto clienteRequestDto, CancellationToken cancellationToken)
        {
            var cliente = new Cliente(clienteRequestDto.Id, clienteRequestDto.Nome, clienteRequestDto.Email, clienteRequestDto.Cpf, clienteRequestDto.Ativo);

            return await clienteUseCase.CadastrarClienteAsync(cliente, clienteRequestDto.Senha, cancellationToken);
        }

        public async Task<bool> AtualizarClienteAsync(ClienteRequestDto clienteRequestDto, CancellationToken cancellationToken)
        {
            var cliente = new Cliente(clienteRequestDto.Id, clienteRequestDto.Nome, clienteRequestDto.Email, clienteRequestDto.Cpf, clienteRequestDto.Ativo);

            return await clienteUseCase.AtualizarClienteAsync(cliente, cancellationToken);
        }

        public async Task<bool> DeletarClienteAsync(Guid id, CancellationToken cancellationToken) =>
            await clienteUseCase.DeletarClienteAsync(id, cancellationToken);

        public async Task<IEnumerable<Cliente>> ObterTodosClientesAsync(CancellationToken cancellationToken) =>
            await clienteUseCase.ObterTodosClientesAsync(cancellationToken);

        public async Task<TokenUsuario> IdentificarClienteCpfAsync(string cfp, string senha, CancellationToken cancellationToken) =>
            await clienteUseCase.IdentificarClienteCpfAsync(cfp, senha, cancellationToken);
    }
}
