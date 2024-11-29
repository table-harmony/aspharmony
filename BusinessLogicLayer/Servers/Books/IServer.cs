using System.Text.Json.Serialization;

namespace BusinessLogicLayer.Servers.Books {
    public interface IBookServer {
        Task<Book?> GetBookAsync(int id);
        Task<List<Book>> GetAllBooksAsync();
        Task CreateBookAsync(Book newBook);
        Task UpdateBookAsync(Book updatedBook);
        Task DeleteBookAsync(int id);
    }

    public class Book {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("description")]
        public string Description { get; set; } = "";

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; } = "";

        [JsonPropertyName("chapters")]
        public List<Chapter> Chapters { get; set; } = [];
    }

    public class Chapter {
        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("content")]
        public string Content { get; set; } = "";
    }
}
