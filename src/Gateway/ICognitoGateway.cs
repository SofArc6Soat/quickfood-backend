using Domain.Entities;
using Gateways.Dtos.Response;

namespace Gateways
{
    public interface ICognitoGateway
    {
        Task<bool> CriarUsuarioClienteAsync(Cliente cliente, string senha, CancellationToken cancellationToken);
        Task<bool> CriarUsuarioAsync(Usuario usuario, string senha, CancellationToken cancellationToken);
        Task<TokenUsuario> IdentificarClientePorCpfAsync(string cpf, string senha, CancellationToken cancellationToken);
    }
}
