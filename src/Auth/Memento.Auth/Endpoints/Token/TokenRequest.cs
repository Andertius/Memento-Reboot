namespace Memento.Auth.Endpoints.Token;

public sealed class TokenRequest
{
    public string Username { get; set; } = "";

    public string Password { get; set; } = "";
}
