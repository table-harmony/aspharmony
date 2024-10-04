using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Utils.Services {
    public interface IEventTracker {
        Task<List<Event>> GetEventsAsync();
        Task<bool> TrackEventAsync(string key);
    }

    /// <summary>
    /// Provides functionality to track events and retrieve event data from a remote API.
    /// </summary>
    public class EventTracker : IEventTracker {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string BaseUrl = "https://www.devharmony.io/api/";
        private const string SchoolId = "j974jktgez2xkhmm8x5resd9m1710q15";

        public async Task<List<Event>> GetEventsAsync() {
            try {
                string apiUrl = $"{BaseUrl}schools/{SchoolId}";
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode) {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic apiResponse = JsonConvert.DeserializeObject(jsonResponse);

                    List<Event> events = new List<Event>();

                    foreach (var eventItem in apiResponse.events) {
                        DateTime creationTime = DateTimeOffset.FromUnixTimeMilliseconds((long)eventItem._creationTime).DateTime;
                        string key = eventItem.key;

                        events.Add(new Event() { CreationTime = creationTime, Key = key });
                    }

                    return events;
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error in GetEventsAsync: {ex.Message}");
            }

            return new List<Event>();
        }

        public async Task<bool> TrackEventAsync(string key) {
            var eventPayload = new {
                key,
                objectId = SchoolId
            };

            try {
                string apiUrl = $"{BaseUrl}schools/{SchoolId}/events";
                var content = new StringContent(JsonConvert.SerializeObject(eventPayload), System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content);

                return response.IsSuccessStatusCode;
            } catch (Exception ex) {
                Console.WriteLine($"Error in TrackEventAsync: {ex.Message}");
                return false;
            }
        }
    }

    /// <summary>
    /// Represents an event with its creation time and key.
    /// </summary>
    public class Event {
        public DateTime CreationTime { get; set; }
        public string Key { get; set; }
    }
}