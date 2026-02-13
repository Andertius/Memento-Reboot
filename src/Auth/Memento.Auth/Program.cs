using FastEndpoints;
using Memento.Auth.Database;
using Memento.Auth.Extensions;
using Memento.Auth.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddOpenApi();

builder.Services.AddFastEndpoints();
builder.Services.AddAuthorization();
builder.Services
    .AddAuthentication(IdentityConstants.BearerScheme)
    .AddJwtBearer();

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthorizationDbContext>();

builder.Services.AddDbContext<AuthorizationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 1;
    options.Password.RequiredUniqueChars = 0;
});

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    await app.ApplyMigrations();
}

app.UseHttpsRedirection();
app.UseFastEndpoints();

app.Run();
