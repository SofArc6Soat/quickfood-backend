using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Mappings
{
    public class PedidoMapping : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedidos", "dbo");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.ValorTotal)
                   .HasColumnType("decimal(18,2)")
                   .HasPrecision(2);

            builder.Property(c => c.Status)
                .HasColumnType("varchar(20)")
                .HasConversion<string>();

            builder.Property(c => c.Pagamento)
                .HasColumnType("varchar(20)")
                .HasConversion<string>();

            builder.Property(c => c.NumeroPedido).UseIdentityColumn(10, 1).ValueGeneratedOnAddOrUpdate();

            // Relacionamentos

            builder.HasMany(c => c.PedidoItems)
                .WithOne(c => c.Pedido)
                .HasForeignKey(c => c.PedidoId);
        }
    }
}
