using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Gateways;
using Gateways.Dtos.Response;

namespace UseCases
{
    public class ClienteUseCase(IClienteGateway clientesGateway, ICognitoGateway cognitoGateway, INotificador notificador) : BaseUseCase(notificador), IClienteUseCase
    {
        public async Task<bool> CadastrarClienteAsync(Cliente cliente, string senha, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(cliente);

            if (clientesGateway.VerificarClienteExistente(cliente.Id, cliente.Cpf, cliente.Email, cancellationToken))
            {
                Notificar("Cliente já existente");
                return false;
            }

            if (ExecutarValidacao(new ValidarCliente(), cliente)
                   && await clientesGateway.CadastrarClienteAsync(cliente, senha, cancellationToken))
            {
                return true;
            }

            Notificar($"Ocorreu um erro ao cadastra o cliente com o e-mail: {cliente.Email}");
            return false;
        }

        public async Task<bool> AtualizarClienteAsync(Cliente cliente, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(cliente);

            if (!await clientesGateway.VerificarClienteExistenteAsync(cliente.Id, cancellationToken))
            {
                Notificar("Cliente inexistente");
                return false;
            }

            return ExecutarValidacao(new ValidarCliente(), cliente)
                   && await clientesGateway.AtualizarClienteAsync(cliente, cancellationToken);
        }

        public async Task<bool> DeletarClienteAsync(Guid id, CancellationToken cancellationToken)
        {
            if (!await clientesGateway.VerificarClienteExistenteAsync(id, cancellationToken))
            {
                Notificar("Cliente inexistente");
                return false;
            }

            return await clientesGateway.DeletarClienteAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Cliente>> ObterTodosClientesAsync(CancellationToken cancellationToken) =>
            await clientesGateway.ObterTodosClientesAsync(cancellationToken);

        public async Task<TokenUsuario?> IdentificarClienteCpfAsync(string cpf, string senha, CancellationToken cancellationToken) =>
            await cognitoGateway.IdentifiqueSe(null, cpf, senha, cancellationToken);
    }
}
