using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Domain.Repositories;
using UseCases.Models.Request;

namespace UseCases.UseCases
{
    public class ClienteUseCase(IClienteRepository clienteRepository, INotificador notificador) : BaseUseCase(notificador), IClienteUseCase
    {
        public async Task<bool> CadastrarClienteAsync(ClienteRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var clienteExistente = clienteRepository.Find(e => e.Id == request.Id || e.Cpf == request.Cpf || e.Email == request.Email).FirstOrDefault(g => g.Id == request.Id);

            if (clienteExistente is not null)
            {
                Notificar("Cliente já existente");
                return false;
            }

            var cliente = new Cliente(request.Id, request.Nome, request.Email, request.Cpf, request.Ativo);

            if (!ExecutarValidacao(new ValidarCliente(), cliente))
            {
                return false;
            }

            await clienteRepository.InsertAsync(cliente, cancellationToken);

            return await clienteRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<bool> AtualizarClienteAsync(ClienteRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var clienteExistente = await clienteRepository.FindByIdAsync(request.Id, cancellationToken);

            if (clienteExistente is null)
            {
                Notificar("Cliente inexistente");
                return false;
            }

            var cliente = new Cliente(request.Id, request.Nome, request.Email, request.Cpf, request.Ativo);

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

        public async Task<Cliente?> IdentificarClienteCpfAsync(IdentifiqueSeRequest request, CancellationToken cancellationToken)
        {
            var cliente = await clienteRepository.IdentificarClienteCpfAsync(request.Cpf, cancellationToken);

            if (cliente is null)
            {
                Notificar($"Cliente {request.Cpf} não encontrado.");

                return cliente;
            }

            return cliente;
        }
    }
}
