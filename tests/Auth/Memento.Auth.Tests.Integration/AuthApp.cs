using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FastEndpoints.Testing;
using Memento.Auth.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using Testcontainers.PostgreSql;

namespace Memento.Auth.Tests.Integration;

public sealed class AuthApp : AppFixture<Program>
{
    private readonly PostgreSqlContainer _databaseContainer = new PostgreSqlBuilder("postgres:latest")
        .WithDatabase("test")
        .WithUsername("postgres")
        .WithPassword("password")
        .Build();

    protected override async ValueTask PreSetupAsync()
        => await _databaseContainer.StartAsync();

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AuthorizationDbContext>(opts => opts.UseNpgsql(_databaseContainer.GetConnectionString()));
        services.AddSingleton<TimeProvider, FakeTimeProvider>();
    }

    protected override void ConfigureApp(IWebHostBuilder app)
    {
        app.ConfigureAppConfiguration(builder => builder.AddInMemoryCollection(new Dictionary<string, string?>()
        {
            ["Jwt:SigningKey"] = "3a6531a32696767377a75988685a602312a7a4b8877cad9d88ac7bc0874ebf33",
            ["Jwt:Issuer"] = "test_issuer",
            ["Jwt:Audience"] = "test_audience",
        }));

        app.UseEnvironment("Development");
    }

    protected override async ValueTask TearDownAsync()
        => await _databaseContainer.StopAsync();
}
