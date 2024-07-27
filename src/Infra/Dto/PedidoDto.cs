using Core.Domain.Entities;

namespace Infra.Dto
{
    public class PedidoDto : Entity
    {
        public int NumeroPedido { get; private set; }
        public Guid? ClienteId { get; private set; }
        public string Status { get; private set; } = string.Empty;
        public decimal ValorTotal { get; private set; }
        public DateTime DataCriacao { get; private set; }

        public List<PedidoItemDto> PedidoItems;

        protected PedidoDto() => PedidoItems = [];

        public PedidoDto(Guid id, int numeroPedido, Guid? clienteId, string status, decimal valorTotal, DateTime dataCricacao, List<PedidoItemDto> pedidoItemDtos)
        {
            Id = id;
            NumeroPedido = numeroPedido;
            ClienteId = clienteId;
            Status = status;
            ValorTotal = valorTotal;
            DataCriacao = dataCricacao;
            PedidoItems = pedidoItemDtos;
        }
    }
}
