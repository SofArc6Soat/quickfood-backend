using Infra.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Mappings
{
    public class PagamentoMapping : IEntityTypeConfiguration<PagamentoDto>
    {
        public void Configure(EntityTypeBuilder<PagamentoDto> builder)
        {
            builder.ToTable("Pagamentos", "dbo");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Valor)
                   .HasColumnType("decimal(18,2)")
                   .HasPrecision(2);

            builder.Property(c => c.Status)
                   .IsRequired()
                   .HasColumnType("varchar(20)");
        }
    }
}
