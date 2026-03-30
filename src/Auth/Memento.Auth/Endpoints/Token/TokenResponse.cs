namespace Memento.Auth.Endpoints.Token;

public sealed class TokenResponse
{
    public string AccessToken { get; set; } = "";
    
    public string RefreshToken { get; set; } = "";
}
