using System.ComponentModel.DataAnnotations;

namespace Controllers.Dtos.Request
{
    public record PedidoStatusDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [AllowedValues("EmPreparacao", "Pronto", "Finalizado", ErrorMessage = "Status inválido.")]
        public required string Status { get; set; }
    }
}