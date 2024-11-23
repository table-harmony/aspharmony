using System.Drawing;
using System.Text.Json;
using Utils.Encryption;
using Utils;
using BusinessLogicLayer.Services.Stegan;
using DataAccessLayer.Entities.Stegan;

namespace BusinessLogicLayer.Servers.Books {
    public class Stegan1Server : IBookServer {
        private readonly string folderPath = PathManager.GetPath(FolderType.Books, "Stegan");
        private readonly HttpClient httpClient = new();

        public Task<Book?> GetBookAsync(int id) {
            string imagePath = Path.Combine(folderPath, $"{id}.png");
            if (!File.Exists(imagePath)) return Task.FromResult<Book?>(null);

            using Bitmap image = new(imagePath);
            string data = Steganography.Decode(image);

            Book? book = DeserializeBook(data);
            return Task.FromResult(book);
        }

        public Task<List<Book>> GetAllBooksAsync() {
            List<Book> books = [];

            string[] files = Directory.GetFiles(folderPath, "*.png");

            foreach (string imagePath in files) {
                using Bitmap image = new(imagePath);
                string bookData = Steganography.Decode(image);

                Book? book = DeserializeBook(bookData);
                if (book != null) books.Add(book);
            }

            return Task.FromResult(books);
        }

        public async Task CreateBookAsync(Book newBook) {
            string data = SerializeBook(newBook);

            using Bitmap image = await GetRandomImageAsync(800, 600);
            Steganography.Encode(data, image);

            string imagePath = Path.Combine(folderPath, $"{newBook.Id}.png");
            image.Save(imagePath, System.Drawing.Imaging.ImageFormat.Png);
        }

        public async Task UpdateBookAsync(Book updatedBook) {
            await DeleteBookAsync(updatedBook.Id);
            await CreateBookAsync(updatedBook);
        }

        public Task DeleteBookAsync(int id) {
            string imagePath = Path.Combine(folderPath, $"{id}.png");

            if (File.Exists(imagePath)) {
                File.Delete(imagePath);
            }

            return Task.CompletedTask;
        }

        private async Task<Bitmap> GetRandomImageAsync(int width, int height) {
            var response = await httpClient.GetAsync($"https://picsum.photos/{width}/{height}");
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            Bitmap bitmap = new(stream);

            return bitmap;
        }

        private static string SerializeBook(Book book) {
            return JsonSerializer.Serialize(book);
        }

        private static Book? DeserializeBook(string data) {
            try {
                return JsonSerializer.Deserialize<Book>(data);
            } catch (JsonException) {
                return null;
            }
        }
    }

    public class Stegan2Server(IBookMetadataService metadataService, IFileUploader fileUploader) : IBookServer {
        private readonly HttpClient _httpClient = new();

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

            string imageUrl = await fileUploader.UploadFileAsync(new IFileUploader.File(memoryStream));

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