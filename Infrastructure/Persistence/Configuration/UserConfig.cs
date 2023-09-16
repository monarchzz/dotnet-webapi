using Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(b => b.Id);
        builder.HasIndex(b => b.Email).IsUnique();

        builder.Property(b => b.FirstName)
            .HasMaxLength(256);
        builder.Property(b => b.LastName)
            .HasMaxLength(256);
        builder.Property(b => b.FullName).HasComputedColumnSql("""
                                                               "FirstName" || ' ' || "LastName"
                                                               """, stored: true)
            .HasMaxLength(512);
        builder.Property(b => b.Email)
            .HasMaxLength(256);
        builder.Property(b => b.Password)
            .HasMaxLength(256);
        builder.Property(b => b.Role)
            .HasMaxLength(256);
    }
}