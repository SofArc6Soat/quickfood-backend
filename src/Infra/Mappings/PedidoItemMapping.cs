using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Mappings
{
    public class PedidoItemMapping : IEntityTypeConfiguration<PedidoItem>
    {
        public void Configure(EntityTypeBuilder<PedidoItem> builder)
        {
            builder.ToTable("PedidosItens", "dbo");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.ValorUnitario)
                   .HasColumnType("decimal(18,2)")
                   .HasPrecision(2);

            // Relacionamentos

            builder.HasOne(c => c.Pedido)
                .WithMany(c => c.PedidoItems);
        }
    }
}
