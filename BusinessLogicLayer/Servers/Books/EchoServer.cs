using System.Data;
using Utils;

namespace BusinessLogicLayer.Servers.Books
{
    public class EchoServer : IBookServer {
        private readonly DataSet data = new();
        private static readonly string filePath = PathManager.GetPath(FolderType.Books, "Echo.xml");

        public EchoServer() {
            ReadData();
        }

        private void ReadData() {
            data.ReadXml(filePath, XmlReadMode.ReadSchema);
        }

        private void SaveData() {
            data.WriteXml(filePath, XmlWriteMode.WriteSchema);
        }

        private bool IsDataSetInitialized() {
            DataTable? books = data.Tables["Books"];
            DataTable? chapters = data.Tables["Chapters"];

            return !(books == null || chapters == null);
        }

        public Task<Book?> GetBookAsync(int id) {
            DataRow? bookRow = data.Tables["Books"]?.AsEnumerable()
                .FirstOrDefault(row => row.Field<int>("Id") == id);

            if (bookRow == null) return Task.FromResult<Book?>(null);

            Book book = ConvertRowToBook(bookRow);

            var chapterRows = data.Tables["Chapters"]?.AsEnumerable()
                .Where(row => row.Field<int>("BookId") == id).ToList();

            if (chapterRows != null) {
                book.Chapters = chapterRows.Select(ConvertRowToChapter).ToList();
            }

            return Task.FromResult<Book?>(book);
        }

        public async Task<List<Book>> GetAllBooksAsync() {
            List<Book> books = data.Tables["Books"]?.AsEnumerable()
                .Select(ConvertRowToBook)
                .ToList() ?? [];

            books = (await Task.WhenAll(books.Select(book => GetBookAsync(book.Id))))
                        .Where(book => book != null)
                        .Cast<Book>()
                        .ToList();

            return books;
        }

        public Task CreateBookAsync(Book newBook) {
            if (!IsDataSetInitialized())
                throw new InvalidOperationException("DataSet is not properly initialized.");

            DataTable books = data.Tables["Books"]!;
            DataTable chapters = data.Tables["Chapters"]!;

            DataRow newBookRow = books.NewRow();
            UpdateRow(newBookRow, newBook);
            books.Rows.Add(newBookRow);

            foreach (Chapter chapter in newBook.Chapters) {
                DataRow newChapterRow = chapters.NewRow();
                UpdateRow(newChapterRow, chapter, newBook.Id);
                chapters.Rows.Add(newChapterRow);
            }

            SaveData();

            return Task.CompletedTask;
        }

        public Task UpdateBookAsync(Book updatedBook) {
            if (!IsDataSetInitialized())
                throw new InvalidOperationException("DataSet is not properly initialized.");

            DataTable books = data.Tables["Books"]!;
            DataTable chapters = data.Tables["Chapters"]!;

            DataRow? bookRow = books.AsEnumerable()
                .FirstOrDefault(row => row.Field<int>("Id") == updatedBook.Id);

            if (bookRow == null)
                return Task.CompletedTask;

            UpdateRow(bookRow, updatedBook);

            chapters.AsEnumerable()
                .Where(row => row.Field<int>("BookId") == updatedBook.Id)
                .ToList().ForEach(row => row.Delete());

            foreach (var chapter in updatedBook.Chapters) {
                DataRow newChapterRow = chapters.NewRow();
                UpdateRow(newChapterRow, chapter, updatedBook.Id);
                chapters.Rows.Add(newChapterRow);
            }

            SaveData();

            return Task.CompletedTask;
        }

        public Task DeleteBookAsync(int id) {
            if (!IsDataSetInitialized())
                throw new InvalidOperationException("DataSet is not properly initialized.");

            DataTable books = data.Tables["Books"]!;
            DataTable chapters = data.Tables["Chapters"]!;

            DataRow? bookRow = books.AsEnumerable()
                .FirstOrDefault(row => row.Field<int>("Id") == id);

            if (bookRow == null)
                return Task.CompletedTask;

            bookRow.Delete();

            chapters.AsEnumerable()
                .Where(row => row.Field<int>("BookId") == id)
                .ToList().ForEach(row => row.Delete());


            SaveData();

            return Task.CompletedTask;
        }


        private static Book ConvertRowToBook(DataRow row) {
            return new Book {
                Id = row.Field<int>("Id"),
                Title = row.Field<string>("Title") ?? "",
                Description = row.Field<string>("Description") ?? "",
                ImageUrl = row.Field<string>("ImageUrl") ?? "",
                Chapters = []
            };
        }

        private static Chapter ConvertRowToChapter(DataRow row) {
            return new Chapter {
                Index = row.Field<int>("Index"),
                Title = row.Field<string>("Title") ?? "",
                Content = row.Field<string>("Content") ?? ""
            };
        }

        private static void UpdateRow(DataRow row, Book book) {
            row["Id"] = book.Id;
            row["Title"] = book.Title;
            row["Description"] = book.Description;
            row["ImageUrl"] = book.ImageUrl;
        }

        private static void UpdateRow(DataRow row, Chapter chapter, int bookId) {
            row["BookId"] = bookId;
            row["Index"] = chapter.Index;
            row["Title"] = chapter.Title;
            row["Content"] = chapter.Content;
        }
    }
}
