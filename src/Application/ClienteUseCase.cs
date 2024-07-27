using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Domain.Repositories;

namespace UseCases
{
    public class ClienteUseCase(IClienteRepository clienteRepository, INotificador notificador) : BaseUseCase(notificador), IClienteUseCase
    {
        public async Task<bool> CadastrarClienteAsync(Cliente cliente, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(cliente);

            var clienteExistente = clienteRepository.Find(e => e.Id == cliente.Id || e.Cpf == cliente.Cpf || e.Email == cliente.Email).FirstOrDefault(g => g.Id == cliente.Id);

            if (clienteExistente is not null)
            {
                Notificar("Cliente já existente");
                return false;
            }

            if (!ExecutarValidacao(new ValidarCliente(), cliente))
            {
                return false;
            }

            await clienteRepository.InsertAsync(cliente, cancellationToken);

            return await clienteRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<bool> AtualizarClienteAsync(Cliente cliente, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(cliente);

            var clienteExistente = await clienteRepository.FindByIdAsync(cliente.Id, cancellationToken);

            if (clienteExistente is null)
            {
                Notificar("Cliente inexistente");
                return false;
            }

            if (!ExecutarValidacao(new ValidarCliente(), cliente))
            {
                return false;
            }

            await clienteRepository.UpdateAsync(cliente, cancellationToken);

            return await clienteRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<bool> DeletarClienteAsync(Guid id, CancellationToken cancellationToken)
        {
            var clienteExistente = await clienteRepository.FindByIdAsync(id, cancellationToken);

            if (clienteExistente is null)
            {
                Notificar("Cliente inexistente");
                return false;
            }

            await clienteRepository.DeleteAsync(id, cancellationToken);

            return await clienteRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<IEnumerable<Cliente>> ObterTodosClientesAsync(CancellationToken cancellationToken) =>
            await clienteRepository.ObterTodosClientesAsync();

        public async Task<Cliente?> IdentificarClienteCpfAsync(string cpf, CancellationToken cancellationToken)
        {
            var cliente = await clienteRepository.IdentificarClienteCpfAsync(cpf, cancellationToken);

            if (cliente is null)
            {
                Notificar($"Cliente {cpf} não encontrado.");

                return cliente;
            }

            return cliente;
        }
    }
}
