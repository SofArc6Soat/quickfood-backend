﻿using Infra.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Mappings
{
    [ExcludeFromCodeCoverage]
    public class UsuarioMapping : IEntityTypeConfiguration<UsuarioDb>
    {
        public void Configure(EntityTypeBuilder<UsuarioDb> builder)
        {
            builder.ToTable("Usuarios", "dbo");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                   .IsRequired()
                   .HasColumnType("varchar(50)");

            builder.Property(c => c.Email)
                   .IsRequired()
                   .HasColumnType("varchar(100)");

            // UK
            builder.HasIndex(u => u.Email)
                   .IsUnique();
        }
    }
}