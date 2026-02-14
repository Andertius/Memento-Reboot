using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Memento.Auth.Database;

public sealed class AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options)
    : IdentityDbContext<IdentityUser>(options)
{
}
