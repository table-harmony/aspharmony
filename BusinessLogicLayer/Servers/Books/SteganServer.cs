using System.Drawing;
using System.Text.Json;
using Utils.Encryption;
using Utils;
using BusinessLogicLayer.Services.Stegan;
using DataAccessLayer.Entities.Stegan;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;


namespace BusinessLogicLayer.Servers.Books {
    public class SteganServer(IBookMetadataService metadataService, IFileUploader fileUploader) : IBookServer {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<Book?> GetBookAsync(int id) {
            var metadata = await metadataService.GetAsync(id);
            if (metadata == null) return null;

            using var imageStream = await _httpClient.GetStreamAsync(metadata.Url);
            using Bitmap image = new(imageStream);
            string data = Steganography.Decode(image);

            return DeserializeBook(data);
        }

        public async Task<List<Book>> GetAllBooksAsync() {
            var allMetadata = await metadataService.GetAllAsync();
            List<Book> books = [];  

            foreach (var metadata in allMetadata) {
                using var imageStream = await _httpClient.GetStreamAsync(metadata.Url);
                using Bitmap image = new(imageStream);
                string data = Steganography.Decode(image);

                Book? book = DeserializeBook(data);
                if (book != null) books.Add(book);
            }

            return books;
        }

        public async Task CreateBookAsync(Book newBook) {
            string data = SerializeBook(newBook);

            using Bitmap image = await GetRandomImageAsync(400, 300);
            Steganography.Encode(data, image);

            string imageUrl = await UploadImageAsync(image);

            await metadataService.CreateAsync(new BookMetadata {
                BookId = newBook.Id,
                Url = imageUrl
            });
        }

        public async Task UpdateBookAsync(Book updatedBook) {
            var existingMetadata = await metadataService.GetAsync(updatedBook.Id);

            if (existingMetadata == null) return;

            string data = SerializeBook(updatedBook);

            using var imageStream = await _httpClient.GetStreamAsync(existingMetadata.Url);
            using Bitmap image = new(imageStream);
            Steganography.Encode(data, image);

            string imageUrl = await UploadImageAsync(image);

            existingMetadata.Url = imageUrl;
            await metadataService.UpdateAsync(existingMetadata);
        }

        public async Task DeleteBookAsync(int id) {
            await metadataService.DeleteAsync(id);
        }

        private async Task<Bitmap> GetRandomImageAsync(int width, int height) {
            var response = await _httpClient.GetAsync($"https://picsum.photos/{width}/{height}");
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            return new Bitmap(stream);
        }

        private async Task<string> UploadImageAsync(Bitmap image) {
            using MemoryStream memoryStream = new();
            image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            memoryStream.Seek(0, SeekOrigin.Begin);

            string imageUrl = await fileUploader.UploadFileAsync(memoryStream);

            return imageUrl;
        }

        private static string SerializeBook(Book book) {
            return JsonSerializer.Serialize(book);
        }

        private static Book? DeserializeBook(string data) {
            return JsonSerializer.Deserialize<Book>(data);
        }
    }
}
