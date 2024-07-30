using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Gateways;
using Infra.Repositories;

namespace UseCases
{
    public class ClienteUseCase(IClienteGateway clientesGateway, IClienteRepository clienteRepository, INotificador notificador) : BaseUseCase(notificador), IClienteUseCase
    {
        public async Task<bool> CadastrarClienteAsync(Cliente cliente, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(cliente);

            if (clientesGateway.VerificarClienteExistente(cliente.Id, cliente.Cpf, cliente.Email, cancellationToken))
            {
                Notificar("Cliente já existente");
                return false;
            }

            return ExecutarValidacao(new ValidarCliente(), cliente)
                   && await clientesGateway.CadastrarClienteAsync(cliente, cancellationToken);
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

        public async Task<IEnumerable<Cliente>> ObterTodosClientesAsync(CancellationToken cancellationToken)
        {
            var clienteDto = await clienteRepository.ObterTodosClientesAsync(cancellationToken);

            if (clienteDto.Any())
            {
                var cliente = new List<Cliente>();
                foreach (var item in clienteDto)
                {
                    cliente.Add(new Cliente(item.Id, item.Nome, item.Email, item.Cpf, item.Ativo));
                }

                return cliente;
            }

            return [];
        }

        public async Task<Cliente?> IdentificarClienteCpfAsync(string cpf, CancellationToken cancellationToken)
        {
            var clienteDto = await clienteRepository.IdentificarClienteCpfAsync(cpf, cancellationToken);

            if (clienteDto is null)
            {
                Notificar($"Cliente {cpf} não encontrado.");

                return null;
            }

            return new Cliente(clienteDto.Id, clienteDto.Nome, clienteDto.Email, clienteDto.Cpf, clienteDto.Ativo);
        }
    }
}
