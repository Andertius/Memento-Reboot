namespace Memento.Auth.Options;

public sealed class JwtOptions
{
    public string Issuer { get; set; } = "";
    public string Audience { get; set; } = "";
    public int ExpirationInMinutes { get; set; }
    public string SigningKey { get; set; } = "";
}
