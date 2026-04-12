using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Testing;
using Memento.Auth.Database;
using Memento.Auth.Entities;
using Memento.Auth.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using Microsoft.IdentityModel.Tokens;
using Xunit;
using TokenRequest = FastEndpoints.Security.TokenRequest;

namespace Memento.Auth.Tests.Integration.RefreshToken;

[Collection<AuthCollection>]
public sealed class RefreshTokenEndpointTests : TestBase<AuthApp>
{
    private readonly AuthApp _app;
    private readonly IServiceScope _scope;
    private readonly DataSeeder _seeder;
    private readonly TimeProvider _time;
    private readonly AuthorizationDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    private RefreshTokenEntity _seededToken = null!;
    
    public RefreshTokenEndpointTests(AuthApp app)
    {
        _app = app;

        _scope = app.Services.CreateScope();
        _time = _scope.ServiceProvider.GetRequiredService<TimeProvider>();
        _dbContext = _scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>();
        _userManager = _scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        _seeder = new DataSeeder(
            _scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>(),
            _userManager,
            _scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>());
    }

    [Fact]
    public async Task Should_return_access_token_on_valid_credentials()
    {
        // Arrange
        var request = new TokenRequest
        {
            UserId = _seededToken.UserId,
            RefreshToken = _seededToken.Token,
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var validationParameters = new TokenValidationParameters
        {
            ValidateLifetime = false,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidIssuer = Constants.Issuer,
            ValidAudience = Constants.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.SigningKey)),
        };

        // Act
        var result = await _app.Client.POSTAsync<TokenService, TokenRequest, TokenResponse>(request);

        // Assert
        Assert.True(result.Response.IsSuccessStatusCode);
        var principal = tokenHandler.ValidateToken(result.Result.AccessToken, validationParameters, out _);
        principal.IsInRole(Constants.Role);
        
        Assert.False(await _dbContext.RefreshTokens.AnyAsync(x => x.UserId == _seededToken.UserId && x.Token == _seededToken.Token, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task Should_return_bad_request_on_invalid_credentials()
    {
        // Arrange
        var request = new TokenRequest
        {
            UserId = "incorrect",
            RefreshToken = "incorrect",
        };

        // Act
        var result = await _app.Client.POSTAsync<TokenService, TokenRequest, TokenResponse>(request);

        // Assert
        Assert.False(result.Response.IsSuccessStatusCode);
        Assert.NotNull(result.ErrorContent);
        Assert.True(await _dbContext.RefreshTokens.AnyAsync(x => x.UserId == _seededToken.UserId && x.Token == _seededToken.Token, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task Should_return_bad_request_on_expired_token()
    {
        // Arrange
        var request = new TokenRequest
        {
            UserId = _seededToken.UserId,
            RefreshToken = _seededToken.Token,
        };

        ((FakeTimeProvider)_time).AdjustTime(new DateTimeOffset(3000, 1, 1, 0, 0, 0, TimeSpan.Zero));

        // Act
        var result = await _app.Client.POSTAsync<TokenService, TokenRequest, TokenResponse>(request);

        // Assert
        Assert.False(result.Response.IsSuccessStatusCode);
        Assert.NotNull(result.ErrorContent);
        Assert.True(await _dbContext.RefreshTokens.AnyAsync(x => x.UserId == _seededToken.UserId && x.Token == _seededToken.Token, TestContext.Current.CancellationToken));
    }

    protected override async ValueTask SetupAsync()
        => _seededToken = await _seeder.SeedAsync();

    protected override async ValueTask TearDownAsync()
    {
        await _seeder.UnseedAsync();
        _scope.Dispose();
        await _dbContext.DisposeAsync();
        _userManager.Dispose();
    }
}
