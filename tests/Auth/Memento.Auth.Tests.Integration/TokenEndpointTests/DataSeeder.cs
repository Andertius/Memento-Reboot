using System;
using System.Threading.Tasks;
using Memento.Auth.Database;
using Microsoft.AspNetCore.Identity;

namespace Memento.Auth.Tests.Integration.TokenEndpointTests;

public sealed class DataSeeder(AuthorizationDbContext _dbContext, UserManager<IdentityUser> _userManager, RoleManager<IdentityRole> _roleManager)
{
    public async Task SeedAsync()
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        var user = new IdentityUser { UserName = "test" };

        var result = await _userManager.CreateAsync(user, "test");

        if (!result.Succeeded)
        {
            throw new Exception($"User could not be created: {string.Join("; ", result.Errors)}");
        }

        await _roleManager.CreateAsync(new IdentityRole(Constants.Role));

        var roleResult = await _userManager.AddToRoleAsync(user, Constants.Role);

        if (!roleResult.Succeeded)
        {
            throw new Exception($"Role could not be created: {string.Join("; ", roleResult.Errors)}");
        }

        await transaction.CommitAsync();
    }

    public async Task UnseedAsync()
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        var user = await _userManager.FindByNameAsync("test");

        if (user is not null)
        {
            await _userManager.DeleteAsync(user);
        }

        var role = await _roleManager.FindByNameAsync(Constants.Role);

        if (role is not null)
        {
            await _roleManager.DeleteAsync(role);
        }

        await transaction.CommitAsync();
    }
}
