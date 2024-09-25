using Domain.Entities;
using Infra.Dto;
using Infra.Repositories;

namespace Gateways
{
    public class UsuarioGateway(IUsuarioRepository usuarioRepository, ICognitoGateway cognitoGateway) : IUsuarioGateway
    {
        public async Task<bool> CadastrarUsuarioAsync(Usuario usuario, string senha, CancellationToken cancellationToken)
        {
            var usuarioDto = new UsuarioDb
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Ativo = usuario.Ativo
            };

            await usuarioRepository.InsertAsync(usuarioDto, cancellationToken);

            return await usuarioRepository.UnitOfWork.CommitAsync(cancellationToken) && await cognitoGateway.CriarUsuarioAsync(usuario, senha, cancellationToken);
        }

        public bool VerificarUsuarioExistente(Guid id, string? email, CancellationToken cancellationToken)
        {
            var usuarioExistente = usuarioRepository.Find(e => e.Id == id || e.Email == email, cancellationToken)
                                                     .FirstOrDefault(g => g.Id == id);

            return usuarioExistente is not null;
        }
    }
}