using System;
using Microsoft.AspNetCore.Identity;

namespace Memento.Auth.Entities;

public class RefreshTokenEntity
{
    public string Token { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public DateTime RefreshExpiry { get; set; }
    
    public virtual IdentityUser User { get; set; } = null!;
}
