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

            var clienteDtoExistente = clienteRepository.Find(e => e.Id == cliente.Id || e.Cpf == cliente.Cpf || e.Email == cliente.Email, cancellationToken).FirstOrDefault(g => g.Id == cliente.Id);

            if (clienteDtoExistente is not null)
            {
                Notificar("Cliente já existente");
                return false;
            }

            if (!ExecutarValidacao(new ValidarCliente(), cliente))
            {
                return false;
            }

            var clienteDto = new ClienteDto
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Email = cliente.Email,
                Cpf = cliente.Cpf,
                Ativo = cliente.Ativo
            };

            await clienteRepository.InsertAsync(clienteDto, cancellationToken);

            return await clienteRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<bool> AtualizarClienteAsync(Cliente cliente, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(cliente);

            var clienteDtoExistente = await clienteRepository.FindByIdAsync(cliente.Id, cancellationToken);

            if (clienteDtoExistente is null)
            {
                Notificar("Cliente inexistente");
                return false;
            }

            if (!ExecutarValidacao(new ValidarCliente(), cliente))
            {
                return false;
            }

            var clienteDto = new ClienteDto
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Email = cliente.Email,
                Cpf = cliente.Cpf,
                Ativo = cliente.Ativo
            };

            await clienteRepository.UpdateAsync(clienteDto, cancellationToken);

            return await clienteRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<bool> DeletarClienteAsync(Guid id, CancellationToken cancellationToken)
        {
            var clienteDtoExistente = await clienteRepository.FindByIdAsync(id, cancellationToken);

            if (clienteDtoExistente is null)
            {
                Notificar("Cliente inexistente");
                return false;
            }

            await clienteRepository.DeleteAsync(id, cancellationToken);

            return await clienteRepository.UnitOfWork.CommitAsync(cancellationToken);
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
