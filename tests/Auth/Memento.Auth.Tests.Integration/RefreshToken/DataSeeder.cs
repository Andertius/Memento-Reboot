using System;
using System.Threading.Tasks;
using Memento.Auth.Database;
using Memento.Auth.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Memento.Auth.Tests.Integration.RefreshToken;

public sealed class DataSeeder(AuthorizationDbContext _dbContext, UserManager<IdentityUser> _userManager, RoleManager<IdentityRole> _roleManager)
{
    public async Task<RefreshTokenEntity> SeedAsync()
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        var user = new IdentityUser { UserName = "test" };

        var result = await _userManager.CreateAsync(user, "test");

        if (!result.Succeeded)
        {
            throw new Exception($"User could not be created: {String.Join("; ", result.Errors)}");
        }

        await _roleManager.CreateAsync(new IdentityRole(Constants.Role));

        var roleResult = await _userManager.AddToRoleAsync(user, Constants.Role);

        if (!roleResult.Succeeded)
        {
            throw new Exception($"Role could not be created: {String.Join("; ", roleResult.Errors)}");
        }

        var token = new RefreshTokenEntity
        {
            UserId = user.Id,
            Token = "test",
            RefreshExpiry = new DateTime(2000, 12, 31, 0, 0, 0, DateTimeKind.Utc),
        };

        _dbContext.RefreshTokens.Add(token);
        await _dbContext.SaveChangesAsync();

        await transaction.CommitAsync();

        return token;
    }

    public async Task UnseedAsync()
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

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

        await _dbContext.RefreshTokens.ExecuteDeleteAsync();

        await transaction.CommitAsync();
    }
}
