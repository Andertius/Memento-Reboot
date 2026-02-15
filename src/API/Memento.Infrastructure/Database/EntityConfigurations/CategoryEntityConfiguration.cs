using Memento.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Memento.Infrastructure.Database.EntityConfigurations;

public sealed class CategoryEntityConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Image).HasMaxLength(512);

        builder.HasIndex(x => x.Name).IsUnique();

        builder
            .HasMany(x => x.Tags)
            .WithMany(x => x.Categories);
    }
}
