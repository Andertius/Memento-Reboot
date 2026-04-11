namespace Memento.Auth.Options;

public sealed class JwtOptions
{
    public string Issuer { get; set; } = "";
    public string Audience { get; set; } = "";
    public int AccessExpirationInMinutes { get; set; }
    public int RefreshExpirationInMinutes { get; set; }
    public string SigningKey { get; set; } = "";
}
