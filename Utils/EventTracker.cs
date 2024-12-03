using Newtonsoft.Json;

namespace Utils {
    public interface IEventTracker {
        Task<List<Event>> GetEventsAsync();
        Task TrackEventAsync(string key);
    }

    public class EventTracker : IEventTracker {
        private static readonly HttpClient _httpClient = new();
        private const string API_URL = "https://www.devharmony.io/api/";
        private const string SCHOOL_ID = "j974jktgez2xkhmm8x5resd9m1710q15";

        /// <summary>
        /// Fetches a list of events from the API and converts them to a list of Event objects.
        /// </summary>
        public async Task<List<Event>> GetEventsAsync() {

            try {
                List<Event> events = [];

                var response = await _httpClient.GetAsync($"{API_URL}schools/{SCHOOL_ID}");

                string content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ApiResponse>(content) ?? 
                    throw new InvalidOperationException("Failed to deserialize API response.");

                foreach (var eventItem in data.Events) {
                    events.Add(new Event {
                        CreationTime = DateTimeOffset.FromUnixTimeMilliseconds((long)eventItem.CreationTime).DateTime,
                        Key = eventItem.Key
                    });
                }

                return events;
            } catch {
                return [];
            }
          }

        /// <summary>
        /// Sends a POST request to track an event.
        /// </summary>
        public async Task TrackEventAsync(string key) {
            var eventPayload = new {
                key,
                objectId = SCHOOL_ID
            };

            StringContent content = new(JsonConvert.SerializeObject(eventPayload),
                System.Text.Encoding.UTF8, "application/json");

            await _httpClient.PostAsync($"{API_URL}/events", content);
        }
    }

    /// <summary>
    /// Represents the response structure from the API.
    /// </summary>
    public class ApiResponse {
        [JsonProperty("school")]
        public required School School { get; set; }

        [JsonProperty("events")]
        public List<ApiEvent> Events { get; set; } = [];
    }

    /// <summary>
    /// Represents an event from the API.
    /// </summary>
    public class ApiEvent {
        [JsonProperty("_creationTime")]
        public double CreationTime { get; set; }

        [JsonProperty("key")]
        public required string Key { get; set; }
    }

    public class School {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public bool IsPublic { get; set; }
    }

    /// <summary>
    /// Represents the Event class for public use in tracking.
    /// </summary>
    public class Event {
        public DateTime CreationTime { get; set; }
        public required string Key { get; set; }
    }
}
