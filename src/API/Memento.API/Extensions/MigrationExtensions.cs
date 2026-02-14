using System.Threading.Tasks;
using Memento.Infrastructure.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Memento.API.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrations(this IApplicationBuilder app)
    {
        await using var scope = app.ApplicationServices.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<CardDbContext>();

        await context.Database.MigrateAsync();
    }
}
