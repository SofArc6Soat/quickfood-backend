using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Infra.Dto;
using Infra.Repositories;

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

            var dto = new ClienteDto(cliente.Id, cliente.Nome, cliente.Email, cliente.Cpf, cliente.Ativo);

            await clienteRepository.InsertAsync(dto, cancellationToken);

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

            var dto = new ClienteDto(cliente.Id, cliente.Nome, cliente.Email, cliente.Cpf, cliente.Ativo);

            await clienteRepository.UpdateAsync(dto, cancellationToken);

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

        public async Task<IEnumerable<Cliente>> ObterTodosClientesAsync(CancellationToken cancellationToken)
        {
            var dto = await clienteRepository.ObterTodosClientesAsync();

            if (dto.Any())
            {
                var cliente = new List<Cliente>();
                foreach (var item in dto)
                {
                    cliente.Add(new Cliente(item.Id, item.Nome, item.Email, item.Cpf, item.Ativo));
                }

                return cliente;
            }

            return [];
        }

        public async Task<Cliente?> IdentificarClienteCpfAsync(string cpf, CancellationToken cancellationToken)
        {
            var dto = await clienteRepository.IdentificarClienteCpfAsync(cpf, cancellationToken);

            if (dto is null)
            {
                Notificar($"Cliente {cpf} não encontrado.");

                return null;
            }

            return new Cliente(dto.Id, dto.Nome, dto.Email, dto.Cpf, dto.Ativo);
        }
    }
}
