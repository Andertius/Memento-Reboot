using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Testing;
using Memento.Auth.Database;
using Memento.Auth.Endpoints.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Xunit;
using TokenRequest = Memento.Auth.Endpoints.Token.TokenRequest;

namespace Memento.Auth.Tests.Integration.AccessToken;

[Collection<AuthCollection>]
public sealed class AccessTokenEndpointTests : TestBase<AuthApp>
{
    private readonly AuthApp _app;
    private readonly IServiceScope _scope;
    private readonly DataSeeder _seeder;
    private readonly AuthorizationDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    public AccessTokenEndpointTests(AuthApp app)
    {
        _app = app;

        _scope = app.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>();
        _userManager = _scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        _seeder = new DataSeeder(
            _dbContext,
            _userManager,
            _scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>());
    }

    [Fact]
    public async Task Should_return_access_token_on_valid_credentials()
    {
        // Arrange
        var request = new TokenRequest
        {
            Username = "test",
            Password = "test",
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
        var result = await _app.Client.POSTAsync<AccessTokenEndpoint, TokenRequest, TokenResponse>(request);

        // Assert
        Assert.True(result.Response.IsSuccessStatusCode);
        var principal = tokenHandler.ValidateToken(result.Result.AccessToken, validationParameters, out _);
        principal.IsInRole(Constants.Role);

        var user = await _userManager.FindByNameAsync(request.Username);
        Assert.True(await _dbContext.RefreshTokens.AnyAsync(x => x.UserId == user!.Id, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task Should_return_unauthorized_on_invalid_credentials()
    {
        // Arrange
        var request = new TokenRequest
        {
            Username = "incorrect",
            Password = "incorrect",
        };

        // Act
        var result = await _app.Client.POSTAsync<AccessTokenEndpoint, TokenRequest>(request);

        // Assert
        Assert.False(result.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        Assert.False(await _dbContext.RefreshTokens.AnyAsync(TestContext.Current.CancellationToken));
    }

    protected override async ValueTask SetupAsync()
        => await _seeder.SeedAsync();

    protected override async ValueTask TearDownAsync()
    {
        await _seeder.UnseedAsync();
        _scope.Dispose();
        await _dbContext.DisposeAsync();
        _userManager.Dispose();
    }
}
