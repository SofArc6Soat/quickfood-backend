using System.ComponentModel.DataAnnotations;

namespace Controllers.Dtos.Request
{
    public record PedidoStatusDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public required string Status { get; set; }
    }
}
