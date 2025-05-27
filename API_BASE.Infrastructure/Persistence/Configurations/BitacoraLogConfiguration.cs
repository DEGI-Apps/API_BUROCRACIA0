using API_BASE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_BASE.Infrastructure.Persistence.Configurations
{
    public class BitacoraLogConfiguration : IEntityTypeConfiguration<BitacoraLog>
    {
        public void Configure(EntityTypeBuilder<BitacoraLog> builder)
        {
            builder.ToTable("BitacoraLogs");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Usuario)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(b => b.Fecha)
                .IsRequired();

            builder.Property(b => b.Accion)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(b => b.Entidad)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(b => b.Detalles)
                .IsRequired();
        }
    }
}
