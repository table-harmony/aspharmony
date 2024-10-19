using System.Drawing;
using System.Text.Json;
using Utils.Encryption;
using Utils;

namespace BusinessLogicLayer.Servers.Books {
    public class SteganographyServer : IBookServer {
        private readonly string folderPath = PathManager.GetPath(FolderType.Books, "Stegan");
        private readonly HttpClient httpClient = new();

        public async Task<Book?> GetBookAsync(int id) {
            string imagePath = Path.Combine(folderPath, $"{id}.png");
            if (!File.Exists(imagePath)) return null;

            using Bitmap image = new(imagePath);
            string data = Steganography.Decode(image);

            Book? book = DeserializeBook(data);
            return book;
        }

        public async Task<List<Book>> GetAllBooksAsync() {
            List<Book> books = [];

            string[] files = Directory.GetFiles(folderPath, "*.png");

            foreach (string imagePath in files) {
                using Bitmap image = new(imagePath);
                string bookData = Steganography.Decode(image);

                Book? book = DeserializeBook(bookData);
                if (book != null) books.Add(book);
            }

            return books;
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

        public async Task DeleteBookAsync(int id) {
            string imagePath = Path.Combine(folderPath, $"{id}.png");

            if (File.Exists(imagePath)) {
                File.Delete(imagePath);
            }
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
}
