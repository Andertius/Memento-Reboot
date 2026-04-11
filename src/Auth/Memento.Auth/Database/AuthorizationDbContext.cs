using Memento.Auth.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Memento.Auth.Database;

public sealed class AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options)
    : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthorizationDbContext).Assembly);
    }
}
