using Domain.Entities;
using Infra.Dto;
using Infra.Repositories;

namespace Gateways
{
    public class ClienteGateway(IClienteRepository clienteRepository) : IClienteGateway
    {
        public async Task<bool> CadastrarClienteAsync(Cliente cliente, CancellationToken cancellationToken)
        {
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
            await clienteRepository.DeleteAsync(id, cancellationToken);

            return await clienteRepository.UnitOfWork.CommitAsync(cancellationToken);
        }


        public bool VerificarClienteExistente(Guid id, string? cpf, string? email, CancellationToken cancellationToken)
        {
            var clienteExistente = clienteRepository.Find(e => e.Id == id || e.Cpf == cpf || e.Email == email, cancellationToken).FirstOrDefault(g => g.Id == id);

            return clienteExistente is not null;
        }

        public async Task<bool> VerificarClienteExistenteAsync(Guid id, CancellationToken cancellationToken)
        {
            var clienteExistente = await clienteRepository.FindByIdAsync(id, cancellationToken);

            return clienteExistente is not null;
        }
    }
}
