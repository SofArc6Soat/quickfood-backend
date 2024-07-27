using System.ComponentModel.DataAnnotations;

namespace Controllers.Dtos.Request
{
    public record IdentifiqueSeDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Length(11, 11, ErrorMessage = "O campo {0} deve conter {1} caracteres.")]
        [Display(Name = "CPF")]
        public required string Cpf { get; set; }
    }
}
