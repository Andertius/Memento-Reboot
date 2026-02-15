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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CardDbContext).Assembly);
    }
}
