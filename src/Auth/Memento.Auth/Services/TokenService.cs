using System;
using System.Threading.Tasks;
using FastEndpoints;
using FastEndpoints.Security;
using Memento.Auth.Options;
using Memento.Auth.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Memento.Auth.Services;

public sealed class TokenService : RefreshTokenService<TokenRequest, TokenResponse>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenRepository _tokenRepository;

    public TokenService(
        IOptions<JwtOptions> jwtOptions,
        UserManager<IdentityUser> userManager,
        ITokenRepository tokenRepository)
    {
        _userManager = userManager;
        _tokenRepository = tokenRepository;

        Setup(o =>
        {
            o.TokenSigningKey = jwtOptions.Value.SigningKey;
            o.Audience = jwtOptions.Value.Audience;
            o.Issuer = jwtOptions.Value.Issuer;

            o.AccessTokenValidity = TimeSpan.FromMinutes(jwtOptions.Value.AccessExpirationInMinutes);
            o.RefreshTokenValidity = TimeSpan.FromMinutes(jwtOptions.Value.RefreshExpirationInMinutes);

            o.Endpoint("/api/refresh-token", _ => { });
        });
    }

    public override async Task PersistTokenAsync(TokenResponse response)
    {
        await _tokenRepository.StoreToken(response);
    }

    public override async Task RefreshRequestValidationAsync(TokenRequest req)
    {
        if (!await _tokenRepository.TokenIsValid(req.UserId, req.RefreshToken))
        {
            AddError(r => r.RefreshToken, "Refresh token is invalid!");
        }
    }

    public override async Task SetRenewalPrivilegesAsync(TokenRequest request, UserPrivileges privileges)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null)
        {
            return;
        }

        var roles = await _userManager.GetRolesAsync(user);
        privileges.Roles.AddRange(roles);
    }
}
