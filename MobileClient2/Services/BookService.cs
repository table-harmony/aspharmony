using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using MobileClient2.Models;

namespace MobileClient2.Services;

public interface IBookService {
    Task<List<Book>> GetBooksAsync();
    Task<Book?> GetBookAsync(int id);
    Task CreateBookAsync(Book book);
    Task UpdateBookAsync(Book book);
    Task DeleteBookAsync(int id);
}

public class BookService : IBookService {
    private readonly HttpClient _httpClient = new();
    private JsonSerializerOptions _jsonOptions;

    public BookService() {
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

    public async Task<List<Book>> GetBooksAsync() {
        var response = await _httpClient.GetAsync("/api/books");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Book>>(content, _jsonOptions) ?? [];
    }

    public async Task<Book?> GetBookAsync(int id) {
        var response = await _httpClient.GetAsync($"/api/books/{id}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Book>();
    }

    public async Task CreateBookAsync(Book book) {
        var response = await _httpClient.PostAsJsonAsync($"/api/books", book);
        response.EnsureSuccessStatusCode();

        await response.Content.ReadFromJsonAsync<Book>();
    }

    public async Task UpdateBookAsync(Book book) {
        var response = await _httpClient.PutAsJsonAsync($"/api/books/{book.Id}", book);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteBookAsync(int id) {
        var response = await _httpClient.DeleteAsync($"/api/books/{id}");
        response.EnsureSuccessStatusCode();
    }
} 