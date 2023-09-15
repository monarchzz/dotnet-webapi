using Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder
            .Property(b => b.Name)
            .HasMaxLength(1024);

        
        builder.HasOne(b => b.Brand).WithMany(b => b.Products).HasForeignKey(b => b.BrandId);
    }
}