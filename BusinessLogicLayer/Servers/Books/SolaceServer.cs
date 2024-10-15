using BusinessLogicLayer.Servers.Books.Documents;

namespace BusinessLogicLayer.Servers.Books {
    public class SolaceServer : IBookServer {
        private readonly IDocumentStorage storage;
        private readonly List<Book> books = [];

        public SolaceServer() {
            storage = DocumentFactory.CreateDocumentStorage(DocumentType.Word);
            LoadData();
        }

        private void LoadData() {
            books.Clear();
            books.AddRange(storage.Load());
        }

        private void SaveData() {
            storage.Save(books);
        }

        public Task<Book?> GetBookAsync(int id) {
            var book = books.FirstOrDefault(b => b.Id == id);
            return Task.FromResult(book);
        }

        public Task<List<Book>> GetAllBooksAsync() {
            return Task.FromResult(books);
        }

        public Task CreateBookAsync(Book newBook) {
            books.Add(newBook);
            SaveData();

            return Task.CompletedTask;
        }

        public Task UpdateBookAsync(Book updatedBook) {
            var book = books.FirstOrDefault(b => b.Id == updatedBook.Id);

            if (book == null)
                return Task.CompletedTask;

            book.Title = updatedBook.Title;
            book.Description = updatedBook.Description;
            book.Chapters = updatedBook.Chapters;

            SaveData();
            return Task.CompletedTask;
        }

        public Task DeleteBookAsync(int id) {
            var book = books.FirstOrDefault(b => b.Id == id);

            if (book == null)
                return Task.CompletedTask;

            books.Remove(book);
            SaveData();

            return Task.CompletedTask;
        }
    }
}
