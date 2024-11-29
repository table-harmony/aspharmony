using System.Net.Http.Json;
using System.Text.Json;

namespace BusinessLogicLayer.Servers.Books {
    public class ApiServer : IBookServer {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiServer(string baseUri, JsonSerializerOptions? jsonOptions = null) {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUri) };
            _jsonOptions = jsonOptions ?? new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<Book?> GetBookAsync(int id) {
            try {
                var response = await _httpClient.GetAsync($"{id}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Book>(content, _jsonOptions);
            } catch (HttpRequestException) {
                return null;
            }
        }

        public async Task<List<Book>> GetAllBooksAsync() {
            try {
                var response = await _httpClient.GetAsync("");
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<List<Book>>(_jsonOptions) ?? [];
            } catch (HttpRequestException) {
                return [];
            }
        }

        public async Task CreateBookAsync(Book newBook) {
            await _httpClient.PostAsJsonAsync("", newBook, _jsonOptions);
        }

        public async Task UpdateBookAsync(Book updatedBook) {
            await _httpClient.PutAsJsonAsync("", updatedBook, _jsonOptions);
        }

        public async Task DeleteBookAsync(int id) {
            await _httpClient.DeleteAsync($"{id}");
        }
    }
}