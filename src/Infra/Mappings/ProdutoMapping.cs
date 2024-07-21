using Domain.Entities;
using Domain.ValueObjects;
using Infra.Mappings.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Mappings
{
    [ExcludeFromCodeCoverage]
    public class ProdutoMapping : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produtos", "dbo");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                   .IsRequired()
                   .HasColumnType("varchar(40)");

            builder.Property(c => c.Descricao)
                   .IsRequired()
                   .HasColumnType("varchar(200)");

            builder.Property(c => c.Preco)
                   .HasColumnType("decimal(18,2)")
                   .HasPrecision(2);

            builder.Property(u => u.Categoria)
                   .IsRequired()
                   .HasConversion(r => r.ToString(), v => (Categoria)Enum.Parse(typeof(Categoria), v));

            // Data
            builder.HasData(ProdutoSeedData.GetSeedData());
        }
    }
}