﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Utils.Services {
    public interface IFileUploader {
        Task<string> UploadFileAsync(IFormFile file);
    }
    
    /// <summary>
    /// File uploader using convex file storage.
    /// </summary>
    public class FileUploader : IFileUploader {
        private readonly HttpClient _httpClient;
        private readonly string API_URL = "https://colorless-shrimp-958.convex.site";

        public FileUploader() {
            _httpClient = new HttpClient();
        }

        // Uploads a file and returns a URL
        public async Task<string> UploadFileAsync(IFormFile file) {
            string uploadUrl = await GenerateUploadUrlAsync();
            string storageId = await UploadToUrlAsync(uploadUrl, file);
            string fileUrl = await GetFileUrlAsync(storageId);

            return fileUrl;
        }

        // Generating an upload url for a file
        private async Task<string> GenerateUploadUrlAsync() {
            var response = await _httpClient.PostAsync($"{API_URL}/generateUploadUrl", null);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(content);
            return result.uploadUrl;
        }

        // Uploading file to an upload url
        private async Task<string> UploadToUrlAsync(string uploadUrl, IFormFile file) {
            using var content = new StreamContent(file.OpenReadStream());
            content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            var response = await _httpClient.PostAsync(uploadUrl, content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(responseContent);
            return result.storageId;
        }

        // Get file url by storage id from convex
        private async Task<string> GetFileUrlAsync(string storageId) {
            var response = await _httpClient.GetAsync($"{API_URL}/getFileUrl?storageId={storageId}");
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