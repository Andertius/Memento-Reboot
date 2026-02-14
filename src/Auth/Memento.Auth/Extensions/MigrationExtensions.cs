using System.Threading.Tasks;
using Memento.Auth.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Memento.Auth.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrations(this IApplicationBuilder app)
    {
        await using var scope = app.ApplicationServices.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>();

        await context.Database.MigrateAsync();
    }
}
