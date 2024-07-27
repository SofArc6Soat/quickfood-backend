using Infra.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Mappings
{
    public class PedidoItemMapping : IEntityTypeConfiguration<PedidoItemDto>
    {
        public void Configure(EntityTypeBuilder<PedidoItemDto> builder)
        {
            builder.ToTable("PedidosItens", "dbo");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.ValorUnitario)
                   .HasColumnType("decimal(18,2)")
                   .HasPrecision(2);
        }
    }
}
