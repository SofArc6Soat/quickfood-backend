using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Mappings
{
    public class PagamentoMapping : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.ToTable("Pagamentos", "dbo");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Valor)
                   .HasColumnType("decimal(18,2)")
                   .HasPrecision(2);

            builder.Property(c => c.Status)
                .HasColumnType("varchar(20)")
                .HasConversion<string>();
        }
    }
}
