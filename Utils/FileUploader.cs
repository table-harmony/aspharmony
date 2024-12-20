﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.IO;

namespace Utils {
    public interface IFileUploader {
        public record struct File(Stream Stream, string ContentType = "image/png");
        Task<string> UploadFileAsync(File file);
        Task<string> UploadFileAsync(IFormFile file);
    }

    public class FileUploader : IFileUploader {
        private readonly HttpClient _httpClient = new();
        private readonly string API_URL = "https://hearty-sardine-346.convex.site";

        public async Task<string> UploadFileAsync(IFileUploader.File file) {
            string uploadUrl = await GenerateUploadUrlAsync();
            string storageId = await UploadToUrlAsync(uploadUrl, file.Stream, file.ContentType);
            string fileUrl = await GetFileUrlAsync(storageId);

            return fileUrl;
        }
        
        public async Task<string> UploadFileAsync(IFormFile file) {
            string uploadUrl = await GenerateUploadUrlAsync();
            string storageId = await UploadToUrlAsync(uploadUrl, file);
            string fileUrl = await GetFileUrlAsync(storageId);

            return fileUrl;
        }

        private async Task<string> GenerateUploadUrlAsync() {
            var response = await _httpClient.PostAsync($"{API_URL}/generateUploadUrl", null);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(content);
            return result.uploadUrl;
        }

        private async Task<string> UploadToUrlAsync(string uploadUrl, Stream fileStream, string contentType) {
            using var content = new StreamContent(fileStream);
            content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            var response = await _httpClient.PostAsync(uploadUrl, content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(responseContent);
            return result.storageId;
        }

        private async Task<string> UploadToUrlAsync(string uploadUrl, IFormFile file) {
            using var content = new StreamContent(file.OpenReadStream());
            content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            var response = await _httpClient.PostAsync(uploadUrl, content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(responseContent);
            return result.storageId;
        }

        private async Task<string> GetFileUrlAsync(string storageId) {
            var response = await _httpClient.GetAsync($"{API_URL}/getFileUrl?storageId={storageId}");
            var responseContent = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            dynamic result = JsonConvert.DeserializeObject(responseContent);
            if (result?.fileUrl == null) {
                throw new InvalidOperationException($"File URL not found in the response. Response: {responseContent}");
            }

            return result.fileUrl;
        }
    }
}
