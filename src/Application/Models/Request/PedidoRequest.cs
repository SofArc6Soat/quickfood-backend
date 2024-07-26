using Core.Domain.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace UseCases.Models.Request
{
    public record PedidoRequest
    {
        [RequiredGuid(ErrorMessage = "O campo {0} é obrigatório.")]
        public required Guid PedidoId { get; set; }

        public Guid? ClienteId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public required List<PedidoItemRequest> Items { get; set; } = [];
    }

    public record PedidoItemRequest
    {
        [RequiredGuid(ErrorMessage = "O campo {0} é obrigatório.")]
        public required Guid ProdutoId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Range(1, 9999, ErrorMessage = "O campo {0} deve ter o valor entre {1} e {2}.")]
        public required int Quantidade { get; set; }
    }
}
