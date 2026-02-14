using System.Threading.Tasks;
using Memento.Auth.Database;
using Microsoft.AspNetCore.Identity;

namespace Memento.Auth.Tests.Integration.CreateUser;

public sealed class DataSeeder(AuthorizationDbContext _dbContext, UserManager<IdentityUser> _userManager, RoleManager<IdentityRole> _roleManager)
{
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

        await transaction.CommitAsync();
    }
}
