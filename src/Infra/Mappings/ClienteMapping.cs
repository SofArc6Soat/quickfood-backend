﻿using Domain.Entities;
using Infra.Mappings.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Mappings
{
    [ExcludeFromCodeCoverage]
    public class ClienteMapping : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes", "dbo");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                   .IsRequired()
                   .HasColumnType("varchar(50)");

            builder.Property(c => c.Email)
                   .HasColumnType("varchar(100)");

            builder.Property(c => c.Cpf)
                   .HasColumnType("varchar(11)");

            // Data
            builder.HasData(ClienteSeedData.GetSeedData());

            builder.HasIndex(u => u.Email)
                   .IsUnique();

            builder.HasIndex(u => u.Cpf)
                   .IsUnique();
        }
    }
}