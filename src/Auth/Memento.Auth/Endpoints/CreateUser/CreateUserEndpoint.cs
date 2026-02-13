using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.Auth.Constants;
using Memento.Auth.Database;
using Microsoft.AspNetCore.Identity;

namespace Memento.Auth.Endpoints.CreateUser;

public sealed class CreateUserEndpoint(AuthorizationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : Endpoint<CreateUserRequest>
{
    private readonly AuthorizationDbContext _context = context ?? throw new ArgumentNullException("Db Context must not be null", nameof(context));
    private readonly UserManager<IdentityUser> _userManager = userManager ?? throw new ArgumentNullException("User Manager must not be null", nameof(userManager));
    private readonly RoleManager<IdentityRole> _roleManager = roleManager ?? throw new ArgumentNullException("User Manager must not be null", nameof(roleManager));

    public override void Configure()
    {
        Post("/api/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserRequest request, CancellationToken token)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        var user = new IdentityUser { UserName = request.Username };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            await Send.ErrorsAsync(cancellation: token);
            return;
        }

        if (!await _roleManager.RoleExistsAsync(RoleNames.Learner))
        {
            await _roleManager.CreateAsync(new IdentityRole(RoleNames.Learner));
        }

        var roleResult = await _userManager.AddToRoleAsync(user, RoleNames.Learner);

        if (!roleResult.Succeeded)
        {
            await Send.ErrorsAsync(cancellation: token);
            return;
        }

        await transaction.CommitAsync();
        await Send.OkAsync();
    }
}
