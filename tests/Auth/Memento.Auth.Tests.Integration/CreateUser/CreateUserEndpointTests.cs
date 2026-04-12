using System.Threading.Tasks;
using FastEndpoints;
using FastEndpoints.Testing;
using Memento.Auth.Constants;
using Memento.Auth.Database;
using Memento.Auth.Endpoints.CreateUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Memento.Auth.Tests.Integration.CreateUser;

[Collection<AuthCollection>]
public sealed class CreateUserEndpointTests : TestBase<AuthApp>
{
    private readonly AuthApp _app;
    private readonly IServiceScope _scope;
    private readonly DataSeeder _seeder;
    private readonly AuthorizationDbContext _dbContext;

    public CreateUserEndpointTests(AuthApp app)
    {
        _app = app;

        _scope = app.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>();

        _seeder = new DataSeeder(
            _dbContext,
            _scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>(),
            _scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>());
    }

    [Fact]
    public async Task Should_create_user_on_valid_credentials()
    {
        // Arrange
        var request = new CreateUserRequest()
        {
            Username = "test",
            Password = "test",
        };

        // Act
        var result = await _app.Client.POSTAsync<CreateUserEndpoint, CreateUserRequest>(request);

        // Assert
        Assert.True(result.IsSuccessStatusCode);

        Assert.Single(_dbContext.Users);
        Assert.Single(_dbContext.Roles);
        Assert.Single(_dbContext.UserRoles);

        var user = await _dbContext.Users.FirstAsync(TestContext.Current.CancellationToken);
        var role = await _dbContext.Roles.FirstAsync(TestContext.Current.CancellationToken);
        var userRole = await _dbContext.UserRoles.FirstAsync(TestContext.Current.CancellationToken);

        Assert.Equal("test", user.UserName);
        Assert.Equal(RoleNames.Learner, role.Name);
        Assert.Equal(user.Id, userRole.UserId);
        Assert.Equal(role.Id, userRole.RoleId);
    }

    [Fact]
    public async Task Should_create_user_on_duplicate_credentials()
    {
        // Arrange
        var request = new CreateUserRequest()
        {
            Username = "test",
            Password = "test",
        };

        // Act
        var result = await _app.Client.POSTAsync<CreateUserEndpoint, CreateUserRequest>(request);
        var result2 = await _app.Client.POSTAsync<CreateUserEndpoint, CreateUserRequest>(request);

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        Assert.False(result2.IsSuccessStatusCode);
    }

    protected override async ValueTask TearDownAsync()
    {
        await _seeder.UnseedAsync();
        _scope.Dispose();
        await _dbContext.DisposeAsync();
    }
}
