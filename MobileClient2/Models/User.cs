using System.Text.Json.Serialization;

namespace MobileClient2.Models;

public class User {
    public required string Id { get; set; }
    [JsonPropertyName("username")]
    public required string Name { get; set; }
    public required string Email { get; set; }
}

public class LoginRequest {
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class RegisterRequest {
    public required string Email { get; set; }
    public required string Password { get; set; }
}