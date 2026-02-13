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
            entityBuilder.Property(x => x.Word).IsRequired();
            entityBuilder.Property(x => x.Definition).IsRequired();

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
            entityBuilder.Property(x => x.Name).IsRequired();
            entityBuilder.Property(x => x.Description).IsRequired();

            entityBuilder
                .HasMany(x => x.Tags)
                .WithMany(x => x.Categories);
        });

        modelBuilder.Entity<TagEntity>(entityBuilder =>
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Name).IsRequired();
        });
    }
}
