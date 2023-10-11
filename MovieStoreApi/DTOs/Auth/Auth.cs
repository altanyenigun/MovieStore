using System.Text.Json.Serialization;

namespace MovieStoreApi.DTOs;

public class CustomerLoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    public string Token { get; set; }
}

public class CustomerRegisterRequest
{
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Password { get; set; }
    [JsonIgnore]
    public string Role { get; set; } = "Customer";
}