namespace Utils.Books {

    public interface IBooksWebService {
        Task<Book?> GetBookAsync(int id);
        Task<List<Book>> GetAllBooksAsync();
        Task CreateBookAsync(Book newBook);
        Task UpdateBookAsync(Book updatedBook);
        Task DeleteBookAsync(int id);
    }

    public class Book {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public List<Chapter> Chapters { get; set; } = [];
    }

    public class Chapter {
        public int Index { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}