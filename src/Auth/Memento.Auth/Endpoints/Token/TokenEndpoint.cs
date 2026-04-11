using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using FastEndpoints.Security;
using Memento.Auth.Services;
using Microsoft.AspNetCore.Identity;

namespace Memento.Auth.Endpoints.Token;

public sealed class TokenEndpoint(UserManager<IdentityUser> userManager) : Endpoint<TokenRequest, TokenResponse>
{
    private readonly UserManager<IdentityUser> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager), "User manager must not be null");

    public override void Configure()
    {
        Post("/api/token");
        AllowAnonymous();
    }

    public override async Task HandleAsync(TokenRequest request, CancellationToken token)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            await Send.UnauthorizedAsync(cancellation: token);
            return;
        }

        var roles = await _userManager.GetRolesAsync(user);

        Response = await CreateTokenWith<TokenService>(user.Id, u =>
        {
            u.Roles.AddRange(roles);
        });

        await Send.OkAsync(Response, cancellation: token);
    }
}
