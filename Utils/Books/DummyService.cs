
using Xceed.Words.NET;

namespace Utils.Books {
    public class DummyBooksService : IBooksWebService, IDisposable {
        private static readonly List<Book> books = []; 
        private static readonly string filePath = Path.Combine(Directory.GetCurrentDirectory(),
            "..", "Storage", "App_Data", "Books", "Dummy.docx");

        public void Dispose() {
            SaveData();
        }

        private static void SaveData() {
            using DocX doc = DocX.Load(filePath);

            doc.InsertParagraph("Books List")
                .FontSize(18)
                .Bold();

            foreach (var book in books) {
                doc.InsertParagraph($"Book ID: {book.Id}")
                    .FontSize(14).Bold();
                doc.InsertParagraph($"Title: {book.Title}")
                    .FontSize(12);
                doc.InsertParagraph($"Description: {book.Description}")
                    .FontSize(12);
                doc.InsertParagraph($"Image URL: {book.ImageUrl}")
                    .FontSize(12);

                if (book.Chapters != null && book.Chapters.Any()) {
                    doc.InsertParagraph("Chapters:").Bold();
                    foreach (var chapter in book.Chapters) {
                        doc.InsertParagraph($"  Chapter {chapter.Index}: {chapter.Title}")
                            .FontSize(12).Italic();
                        doc.InsertParagraph($"  Content: {chapter.Content}")
                            .FontSize(12);
                    }
                }
                doc.InsertParagraph();
            }

            doc.Save();
        }

        public Task<Book?> GetBookAsync(int id) {
            return Task.FromResult(books.FirstOrDefault(book => book.Id == id));
        }

        public Task<List<Book>> GetAllBooksAsync() {
            return Task.FromResult(books);
        }

        public Task CreateBookAsync(Book newBook) {
            books.Add(newBook);

            return Task.CompletedTask;
        }

        public Task UpdateBookAsync(Book updatedBook) {
            Book? book = books.FirstOrDefault(book => book.Id == updatedBook.Id);

            if (book != null) {
                book.Title = updatedBook.Title;
                book.Description = updatedBook.Description;
                book.Chapters = updatedBook.Chapters;
            }

            return Task.CompletedTask;
        }

        public Task DeleteBookAsync(int id) {
            Book? book = books.FirstOrDefault(book => book.Id == id);

            if (book != null)
                books.Remove(book);
            
            return Task.CompletedTask;
        }
    }
}
