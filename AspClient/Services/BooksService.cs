using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AspClient.Services {
    public class BookResponse {
        public int Id { get; set; }
        public BookMetadata Metadata { get; set; } = new BookMetadata();

        public UserResponse Author { get; set; }
        public int Server { get; set; }
    }

    public class BookMetadata {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; } = "";
    }

    public class UserResponse {
        public string Id { get; set; } = "";
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
    }

    public class BookService {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:7137";

        public BookService() {
            _httpClient = new HttpClient {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        public async Task<List<BookResponse>> GetAllAsync(string serverType = null) {
            string url = "/api/books";
            if (!string.IsNullOrEmpty(serverType)) {
                url += $"?serverType={serverType}";
            }

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<BookResponse>>(json) ?? new List<BookResponse>();
        }

        public async Task<BookResponse> GetBookAsync(int id) {
            var response = await _httpClient.GetAsync($"/api/books/{id}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BookResponse>(json);
        }

        public async Task<BookResponse> CreateAsync(string title, string description, string imageUrl, int server, string authorId) {
            var content = new StringContent(
                JsonConvert.SerializeObject(new {
                    server,
                    author_id = authorId,
                    metadata = new {
                        title,
                        description,
                        image_url = imageUrl
                    }
                }),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("/api/books", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BookResponse>(json);
        }

        public async Task UpdateAsync(int id, string authorId, int server, string title, string description, string imageUrl) {
            var content = new StringContent(
                JsonConvert.SerializeObject(new {
                    id,
                    server,
                    author_id = authorId,
                    metadata = new {
                        id,
                        chapters = new List<object>(),
                        title,
                        description,
                        image_url = imageUrl
                    }
                }),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PutAsync($"/api/books/{id}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id) {
            var response = await _httpClient.DeleteAsync($"/api/books/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}