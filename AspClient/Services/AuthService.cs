using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AspClient.Services {
    public class User {
        public string Id { get; set; } = "";
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
    }

    public class AuthService {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:7137";

        public AuthService() {
            _httpClient = new HttpClient {
                BaseAddress = new Uri(BaseUrl)
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<User> LoginAsync(string email, string password) {
            var payload = new {
                email,
                password
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(payload),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("/api/auth/login", content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(jsonResponse);
        }

        public async Task<User> RegisterAsync(string email, string password) {
            var payload = new {
                email,
                password
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(payload),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("/api/auth/register", content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(jsonResponse);
        }

        public async Task LogoutAsync() {
            var response = await _httpClient.PostAsync("/api/auth/logout", null);
            response.EnsureSuccessStatusCode();
        }
    }
}