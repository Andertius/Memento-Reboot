using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FastEndpoints;
using FastEndpoints.Testing;
using Memento.Auth.Database;
using Memento.Auth.Endpoints.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Memento.Auth.Tests.Integration.Token;

[Collection<AuthCollection>]
public sealed class TokenEndpointTests : TestBase<AuthApp>
{
    private readonly AuthApp _app;
    private readonly DataSeeder _seeder;

    public TokenEndpointTests(AuthApp app)
    {
        _app = app;

        var scope = app.Services.CreateScope();

        _seeder = new DataSeeder(
            scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>(),
            scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>(),
            scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>());
    }

    [Fact]
    public async Task Should_return_access_token_on_valid_credentials()
    {
        // Arrange
        var request = new TokenRequest()
        {
            Username = "test",
            Password = "test",
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters()
        {
            ValidateLifetime = false,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidIssuer = Constants.Issuer,
            ValidAudience = Constants.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.SigningKey))
        };

        // Act
        var result = await _app.Client.POSTAsync<TokenEndpoint, TokenRequest, TokenResponse>(request);

        // Assert
        Assert.True(result.Response.IsSuccessStatusCode);
        var principal = tokenHandler.ValidateToken(result.Result.AccessToken, validationParameters, out _);
        principal.IsInRole(Constants.Role);
    }

    [Fact]
    public async Task Should_return_unauthorized_on_invalid_credentials()
    {
        // Arrange
        var request = new TokenRequest()
        {
            Username = "incorrect",
            Password = "incorrect",
        };

        // Act
        var result = await _app.Client.POSTAsync<TokenEndpoint, TokenRequest>(request);

        // Assert
        Assert.False(result.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }

    protected override async ValueTask SetupAsync()
        => await _seeder.SeedAsync();

    protected override async ValueTask TearDownAsync()
        => await _seeder.UnseedAsync();
}
