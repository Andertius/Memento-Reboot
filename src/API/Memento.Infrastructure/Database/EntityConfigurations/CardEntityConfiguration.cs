using Memento.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Memento.Infrastructure.Database.EntityConfigurations;

public sealed class CardEntityConfiguration : IEntityTypeConfiguration<CardEntity>
{
    public void Configure(EntityTypeBuilder<CardEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Word).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Translation).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Definition).HasMaxLength(256);
        builder.Property(x => x.Hint).HasMaxLength(256);
        builder.Property(x => x.Image).HasMaxLength(512);

        builder
            .HasMany(x => x.Categories)
            .WithMany(x => x.Cards);

        builder
            .HasMany(x => x.Tags)
            .WithMany(x => x.Cards);
    }
}
