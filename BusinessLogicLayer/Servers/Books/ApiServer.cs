using System.Net.Http.Json;
using System.Text.Json;

namespace BusinessLogicLayer.Servers.Books {
    public class ApiServer : IBookServer {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions? _jsonOptions;

        public ApiServer(string baseUri, JsonSerializerOptions? jsonOptions = null) {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUri) };
            _jsonOptions = jsonOptions;
        }

        public async Task<Book?> GetBookAsync(int id) {
            try {
                var response = await _httpClient.GetAsync($"/api/books/{id}");
                response.EnsureSuccessStatusCode();
                
                return await response.Content.ReadFromJsonAsync<Book>(_jsonOptions);
            } catch (HttpRequestException) {
                return null;
            }
        }

        public async Task<List<Book>> GetAllBooksAsync() {
            try {
                var response = await _httpClient.GetAsync("/api/books");
                response.EnsureSuccessStatusCode();
                
                return await response.Content.ReadFromJsonAsync<List<Book>>(_jsonOptions) ?? [];
            } catch (HttpRequestException) {
                return [];
            }
        }

        public async Task CreateBookAsync(Book newBook) {
            await _httpClient.PostAsJsonAsync("/api/books", newBook, _jsonOptions);
        }

        public async Task UpdateBookAsync(Book updatedBook) {
            await _httpClient.PutAsJsonAsync("/api/books", updatedBook, _jsonOptions);
        }

        public async Task DeleteBookAsync(int id) {
            await _httpClient.DeleteAsync($"/api/books/{id}");
        }
    }
}
