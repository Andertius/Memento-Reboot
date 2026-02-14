using Memento.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Memento.Infrastructure.Database;

public sealed class CardDbContext(DbContextOptions<CardDbContext> options) : DbContext(options)
{
    public DbSet<CardEntity> Cards { get; set; } = null!;

    public DbSet<CategoryEntity> Categories { get; set; } = null!;

    public DbSet<TagEntity> Tags { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CardEntity>(entityBuilder =>
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Word).IsRequired().HasMaxLength(256);
            entityBuilder.Property(x => x.Translation).IsRequired().HasMaxLength(256);
            entityBuilder.Property(x => x.Definition).HasMaxLength(256);
            entityBuilder.Property(x => x.Hint).HasMaxLength(256);
            entityBuilder.Property(x => x.Image).HasMaxLength(512);

            entityBuilder
                .HasMany(x => x.Categories)
                .WithMany(x => x.Cards);

            entityBuilder
                .HasMany(x => x.Tags)
                .WithMany(x => x.Cards);
        });

        modelBuilder.Entity<CategoryEntity>(entityBuilder =>
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Name).IsRequired().HasMaxLength(256);
            entityBuilder.Property(x => x.Description).IsRequired().HasMaxLength(256);
            entityBuilder.Property(x => x.Image).HasMaxLength(512);

            entityBuilder
                .HasMany(x => x.Tags)
                .WithMany(x => x.Categories);
        });

        modelBuilder.Entity<TagEntity>(entityBuilder =>
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        });
    }
}
