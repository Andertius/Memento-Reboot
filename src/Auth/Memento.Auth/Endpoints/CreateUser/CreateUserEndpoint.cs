using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Memento.Auth.Constants;
using Memento.Auth.Database;
using Microsoft.AspNetCore.Identity;

namespace Memento.Auth.Endpoints.CreateUser;

public sealed class CreateUserEndpoint(
    AuthorizationDbContext context,
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager) : Endpoint<CreateUserRequest>
{
    private readonly AuthorizationDbContext _context = context ?? throw new ArgumentNullException(nameof(context), "Db Context must not be null");
    private readonly UserManager<IdentityUser> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager), "User Manager must not be null");
    private readonly RoleManager<IdentityRole> _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager), "User Manager must not be null");

    public override void Configure()
    {
        Post("/api/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserRequest request, CancellationToken token)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(token);
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

        await transaction.CommitAsync(token);
        await Send.OkAsync(cancellation: token);
    }
}
