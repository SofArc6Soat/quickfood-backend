using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Gateways.Dtos.Request
{
    public record IdentifiqueSeRequestDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Length(11, 11, ErrorMessage = "O campo {0} deve conter {1} caracteres.")]
        [Display(Name = "CPF")]
        public required string Cpf { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Length(8, 50, ErrorMessage = "O campo {0} deve conter {1} caracteres.")]
        [PasswordPropertyText]
        public required string Senha { get; set; }
    }
}
