using System.IO.Abstractions;
using FastEndpoints;
using FastEndpoints.Security;
using Memento.API.Extensions;
using Memento.API.Options;
using Memento.Services.Services;
using Memento.Infrastructure.Database;
using Memento.Infrastructure.Repositories;
using Memento.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddOpenApi();

var jwtOptions = new JwtOptions();
builder.Configuration.GetSection("Jwt").Bind(jwtOptions);

builder.Services
   .AddAuthenticationJwtBearer(s => s.SigningKey = jwtOptions.SigningKey)
   .AddAuthorization()
   .AddFastEndpoints();

builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITagService, TagService>();

builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();

builder.Services.AddScoped<IFileSystem, FileSystem>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<WebRootPathProvider>(sp => new WebRootPathProvider { ImageRootPath = $"{sp.GetRequiredService<IWebHostEnvironment>().WebRootPath}/images" });

builder.Services.AddDbContext<CardDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication()
   .UseAuthorization()
   .UseFastEndpoints();

await app.ApplyMigrations();
app.Run();
