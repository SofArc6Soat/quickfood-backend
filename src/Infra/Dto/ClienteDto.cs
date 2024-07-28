using Core.Domain.Entities;

namespace Infra.Dto
{
    public class ClienteDto : Entity
    {
        public string Nome { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string Cpf { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }
}
