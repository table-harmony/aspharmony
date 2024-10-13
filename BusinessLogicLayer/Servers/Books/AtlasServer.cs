using System.Net.Http.Json;
using System.Text.Json;

namespace BusinessLogicLayer.Servers.Books {
    public class AtlasServer : IBookServer {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public AtlasServer() {
            _httpClient = new() { BaseAddress = new Uri("https://localhost:7137/") };
            _jsonOptions = new() { PropertyNameCaseInsensitive = true };
        }

        public async Task<Book?> GetBookAsync(int id) {
            var response = await _httpClient.GetAsync($"api/Books/{id}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Book>(_jsonOptions);
        }

        public async Task<List<Book>> GetAllBooksAsync() {
            var response = await _httpClient.GetAsync("api/Books");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<Book>>(_jsonOptions);
        }

        public async Task CreateBookAsync(Book newBook) {
            var response = await _httpClient.PostAsJsonAsync("api/Books", newBook, _jsonOptions);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateBookAsync(Book updatedBook) {
            var response = await _httpClient.PutAsJsonAsync($"api/Books/{updatedBook.Id}", updatedBook, _jsonOptions);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteBookAsync(int id) {
            var response = await _httpClient.DeleteAsync($"api/Books/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
