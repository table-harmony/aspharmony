using MobileClient2.Models;
using System.Net.Http.Json;

namespace MobileClient2.Services;

public interface IAuthService {
    bool IsAuthenticated();
    Task<User?> LoginAsync(string email, string password);
    Task<User?> RegisterAsync(string email, string password);
    Task LogoutAsync();
}

public class AuthService : IAuthService {
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://localhost:7137/";
    private bool isAuthenticated = false;

    public AuthService() {
        _httpClient = new HttpClient {
            BaseAddress = new Uri(BaseUrl)
        };
    }

    public async Task<User?> LoginAsync(string email, string password) {
        var request = new LoginRequest {
            Email = email,
            Password = password
        };

        var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
        response.EnsureSuccessStatusCode();

        User user = await response.Content.ReadFromJsonAsync<User>() 
            ?? throw new Exception("Invalid credentials");
        isAuthenticated = true;

        return user;
    }

    public async Task<User?> RegisterAsync(string email, string password) {
        var request = new RegisterRequest {
            Email = email,
            Password = password
        };

        var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);
        response.EnsureSuccessStatusCode();

        User user = await response.Content.ReadFromJsonAsync<User>()
            ?? throw new Exception("Something wrong happened");
        isAuthenticated = true;

        return user;
    }

    public async Task LogoutAsync() {
        var response = await _httpClient.PostAsync("api/auth/logout", null);
        response.EnsureSuccessStatusCode();

        isAuthenticated = false;
    }

    public bool IsAuthenticated() {
        return isAuthenticated;
    }
}