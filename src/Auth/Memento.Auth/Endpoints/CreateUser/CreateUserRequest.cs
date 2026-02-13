namespace Memento.Auth.Endpoints.CreateUser;

public sealed class CreateUserRequest
{
    public string Username { get; set; } = "";

    public string Password { get; set; } = "";
}
