using Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request
{
    public record PedidoStatusRequest
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EnumDataType(typeof(PedidoStatus))]
        public PedidoStatus Status { get; set; }
    }
}
