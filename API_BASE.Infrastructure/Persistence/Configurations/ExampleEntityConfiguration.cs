using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// Reemplaza "ExampleEntity" por tu entidad real cuando la tengas
// namespace API_BASE.Domain.Entities { public class ExampleEntity { ... } }

namespace API_BASE.Infrastructure.Persistence.Configurations
{
    // Esta clase es solo un ejemplo, no será registrada hasta que se use
    public class ExampleEntityConfiguration : IEntityTypeConfiguration<object> // clase de ejemplo "object" 
    {
        public void Configure(EntityTypeBuilder<object> builder)
        {
            // Ejemplo de configuración:
            // builder.ToTable("ExampleEntities");
            // builder.HasKey(x => x.Id);
            // builder.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
        }
    }
}
