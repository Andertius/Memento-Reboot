using System;
using System.Threading.Tasks;
using FastEndpoints.Security;
using Memento.Auth.Database;
using Memento.Auth.Entities;
using Microsoft.EntityFrameworkCore;

namespace Memento.Auth.Repositories;

public interface ITokenRepository
{
    Task StoreToken(TokenResponse response);

    Task<bool> TokenIsValid(string userId, string refreshToken);
}

public sealed class TokenRepository(AuthorizationDbContext _context, TimeProvider _time) : ITokenRepository
{
    public async Task StoreToken(TokenResponse response)
    {
        var user = await _context.RefreshTokens.FindAsync(response.UserId);

        if (user is not null)
        {
            user.Token = response.RefreshToken;
            await _context.SaveChangesAsync();

            return;
        }

        var token = new RefreshTokenEntity
        {
            Token = response.RefreshToken,
            UserId = response.UserId,
            RefreshExpiry = response.RefreshExpiry,
        };

        _context.RefreshTokens.Add(token);
        await _context.SaveChangesAsync();
    }

    public Task<bool> TokenIsValid(string userId, string refreshToken)
        => _context
            .RefreshTokens
            .AnyAsync(x => x.UserId == userId && x.Token == refreshToken && x.RefreshExpiry >= _time.GetUtcNow());
}
