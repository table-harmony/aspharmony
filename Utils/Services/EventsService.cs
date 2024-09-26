using Newtonsoft.Json;
using System.Text;

namespace Utils.Services {

    public interface IEventsService {
        Task<List<Event>> GetEventsAsync();
        Task<bool> TrackEventAsync(string key);
    }

    public class EventsService : IEventsService {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string BaseUrl = "https://www.devharmony.io/api/";
        private const string SchoolId = "j974jktgez2xkhmm8x5resd9m1710q15";

        public async Task<List<Event>> GetEventsAsync() {
            try {
                string apiUrl = $"{BaseUrl}schools/{SchoolId}";
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode) {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);
                    return apiResponse.events;
                }

            } catch (Exception ex) {
                Console.WriteLine($"Error fetching events: {ex.Message}");
            }

            return new List<Event>();
        }

        public async Task<bool> TrackEventAsync(string key) {
            var eventPayload = new {
                key,
                objectId = SchoolId
            };

            try {
                string apiUrl = $"{BaseUrl}events";
                var content = new StringContent(
                    JsonConvert.SerializeObject(eventPayload), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content);

                return response.IsSuccessStatusCode;
            } catch (Exception ex) {
                Console.WriteLine($"Error tracking event: {ex.Message}");
                return false;
            }
        }
    }

    public class ApiResponse {
        public School school { get; set; }
        public List<Event> events { get; set; }
    }

    public class School {
        public long _creationTime { get; set; }
        public string _id { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public bool isPublic { get; set; }
        public string name { get; set; }
    }

    public class Event {
        public long _creationTime { get; set; }
        public string _id { get; set; }
        public string key { get; set; }
        public string objectId { get; set; }
    }
}