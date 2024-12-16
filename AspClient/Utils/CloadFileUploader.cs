using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AspClient.Utils {
    /// <summary>
    /// מחלקה המממשת העלאת קבצים לשירות Convex
    /// </summary>
    public class CloadFileUploader : IFileUploader {
        private readonly HttpClient _httpClient;  // לקוח HTTP לביצוע בקשות לשרת
        private readonly string API_URL = "https://colorless-shrimp-958.convex.site";  //  כתובת ה-API של השירות

        public CloadFileUploader() {
            _httpClient = new HttpClient() {
                BaseAddress = new Uri(API_URL)
            };
        }

        /// <summary>
        /// מעלה קובץ לשרת ומחזיר את כתובת הגישה אליו
        /// </summary>
        /// <param name="file">הקובץ להעלאה</param>
        /// <returns>כתובת URL לגישה לקובץ</returns>
        public async Task<string> UploadFileAsync(File file) {
            string uploadUrl = await GenerateUploadUrlAsync();
            string storageId = await UploadToUrlAsync(uploadUrl, file);
            string fileUrl = await GetFileUrlAsync(storageId);

            return fileUrl;
        }

        /// <summary>
        /// מייצר כתובת URL זמנית להעלאת הקובץ
        /// </summary>
        /// <returns>כתובת URL להעלאה</returns>
        private async Task<string> GenerateUploadUrlAsync() {
            var response = await _httpClient.PostAsync($"generateUploadUrl", null);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(content);

            return result.uploadUrl;
        }

        /// <summary>
        /// מעלה את הקובץ לכתובת שנוצרה
        /// </summary>
        /// <param name="uploadUrl">כתובת ההעלאה</param>
        /// <param name="file">הקובץ להעלאה</param>
        /// <returns>מזהה הקובץ במערכת האחסון</returns>
        private async Task<string> UploadToUrlAsync(string uploadUrl, File file) {
            using (var content = new StreamContent(file.Stream)) {
                content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

                var response = await _httpClient.PostAsync(uploadUrl, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(responseContent);

                return result.storageId;
            }
        }

        /// <summary>
        /// מקבל את כתובת הגישה הסופית לקובץ
        /// </summary>
        /// <param name="storageId">מזהה הקובץ במערכת האחסון</param>
        /// <returns>כתובת URL סופית לגישה לקובץ</returns>
        private async Task<string> GetFileUrlAsync(string storageId) {
            var response = await _httpClient.GetAsync($"getFileUrl?storageId={storageId}");
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(responseContent);

            if (result?.fileUrl == null) {
                throw new InvalidOperationException($"File URL not found in the response. Response: {responseContent}");
            }

            return result.fileUrl;
        }
    }
}