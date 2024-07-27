using Core.Domain.Entities;

namespace Infra.Dto
{
    public class ClienteDto : Entity
    {
        public string Nome { get; private set; }
        public string? Email { get; private set; }
        public string Cpf { get; private set; }
        public bool Ativo { get; private set; }

        public ClienteDto(Guid id, string nome, string? email, string cpf, bool ativo)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Cpf = cpf;
            Ativo = ativo;
        }
    }
}
