using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AspClient.Services {
    public class LibraryResponse {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        [JsonProperty("allow_copies")]
        public bool AllowCopies { get; set; }
        public List<LibraryBookResponse> Books { get; set; } = new List<LibraryBookResponse>();
        public List<MembershipResponse> Members { get; set; } = new List<MembershipResponse>();
    }

    public class LibraryBookResponse {
        public int Id { get; set; }
        [JsonProperty("book_id")]
        public int BookId { get; set; }
        public BookResponse Book { get; set; } = new BookResponse();
    }

    public class MembershipResponse {
        public int Id { get; set; }
        public string Role { get; set; } = "";
        public UserResponse User { get; set; } = new UserResponse();
    }

    public class JoinLibraryRequest {
        public string UserId { get; set; }
        public string Role { get; set; }
    }
    public class LibraryService {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:7137";

        public LibraryService() {
            _httpClient = new HttpClient {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        public async Task<List<LibraryResponse>> GetAllAsync() {
            var response = await _httpClient.GetAsync("/api/libraries");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<LibraryResponse>>(json) ?? new List<LibraryResponse>();
        }

        public async Task<LibraryResponse> GetLibraryAsync(int id) {
            var response = await _httpClient.GetAsync($"/api/libraries/{id}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LibraryResponse>(json);
        }

        public async Task<LibraryResponse> CreateAsync(string name, bool allowCopies) {
            var content = new StringContent(
                JsonConvert.SerializeObject(new { name, allow_copies = allowCopies }),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("/api/libraries", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LibraryResponse>(json);
        }

        public async Task UpdateAsync(int id, string name, bool allowCopies) {
            var content = new StringContent(
                JsonConvert.SerializeObject(new {
                    id,
                    name,
                    allow_copies = allowCopies
                }),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PutAsync($"/api/libraries/{id}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id) {
            var response = await _httpClient.DeleteAsync($"/api/libraries/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task AddBookAsync(int libraryId, int bookId) {
            var content = new StringContent(
                JsonConvert.SerializeObject(new { book_id = bookId }),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync($"/api/libraries/{libraryId}/books", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveBookAsync(int libraryId, int bookId) {
            var response = await _httpClient.DeleteAsync($"/api/libraries/{libraryId}/books/{bookId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task JoinAsync(int libraryId, string userId, string role) {
            var content = new StringContent(
                JsonConvert.SerializeObject(new { user_id = userId, role }),
                Encoding.UTF8,
                "application/json"
            );

            await _httpClient.PostAsync($"/api/libraries/{libraryId}/members", content);
        }
    }
}