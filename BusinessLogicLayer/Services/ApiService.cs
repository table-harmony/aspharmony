using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services {

    public class ApiService {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string BaseUrl = "https://www.devharmony.io/api/";
        private const string SchoolId = "j974jktgez2xkhmm8x5resd9m1710q15";

        public async Task<string> GetSchoolDataAsync() {
            try {
                string apiUrl = $"{BaseUrl}schools/{SchoolId}";
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode) {
                    return await response.Content.ReadAsStringAsync();
                }

            } catch (Exception ex) {
                Console.WriteLine($"Error fetching school data: {ex.Message}");
            }

            return null;
        }

        public async Task<bool> TrackEventAsync(string eventKey) {
            var eventPayload = new {
                key = eventKey,
                objectId = SchoolId
            };

            try {
                string apiUrl = $"{BaseUrl}events";
                var content = new StringContent(JsonConvert.SerializeObject(eventPayload), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content);

                return response.IsSuccessStatusCode;
            } catch (Exception ex) {
                Console.WriteLine($"Error tracking event: {ex.Message}");
                return false;
            }
        }
    }
}
