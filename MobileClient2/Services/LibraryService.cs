using MobileClient2.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace MobileClient2.Services;

public interface ILibraryService {
    Task<List<Library>> GetLibrariesAsync();
    Task<Library?> GetLibraryAsync(int id);
    Task CreateLibraryAsync(Library library);
    Task UpdateLibraryAsync(Library library);
    Task DeleteLibraryAsync(int id);
    Task AddBookToLibraryAsync(int libraryId, int bookId);
    Task RemoveBookFromLibraryAsync(int libraryId, int bookId);
}

public class LibraryService : ILibraryService {
    private readonly HttpClient _httpClient;
    private JsonSerializerOptions _jsonOptions;

    public LibraryService() {
        _httpClient = new HttpClient {
            BaseAddress = new Uri("http://localhost:7137/")
        };
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        _jsonOptions = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    public async Task<List<Library>> GetLibrariesAsync() {
        var response = await _httpClient.GetAsync($"/api/libraries");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Library>>(content, _jsonOptions) ?? [];
    }

    public async Task<Library?> GetLibraryAsync(int id) {
        var response = await _httpClient.GetAsync($"/api/libraries/{id}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Library>(content, _jsonOptions);
    }

    public async Task CreateLibraryAsync(Library library) {
        var response = await _httpClient.PostAsJsonAsync($"/api/libraries", library);
        response.EnsureSuccessStatusCode();

        await response.Content.ReadFromJsonAsync<Library>();
    }

    public async Task UpdateLibraryAsync(Library library) {
        var response = await _httpClient.PutAsJsonAsync($"/api/libraries/{library.Id}", library);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteLibraryAsync(int id) {
        var response = await _httpClient.DeleteAsync($"/api/libraries/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task AddBookToLibraryAsync(int libraryId, int bookId) {
        var response = await _httpClient.PostAsJsonAsync($"/api/libraries/ {libraryId}", new { BookId = bookId });
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveBookFromLibraryAsync(int libraryId, int bookId) {
        var response = await _httpClient.DeleteAsync($"/api/libraries/ {libraryId}");
        response.EnsureSuccessStatusCode();
    }
}
