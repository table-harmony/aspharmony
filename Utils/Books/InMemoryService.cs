using System.Data;

namespace Utils.Books {
    public class InMemoryBooksService : IBooksWebService, IDisposable {
        private readonly DataSet data = new();
        private static readonly string xmlFilePath = Path.Combine(Directory.GetCurrentDirectory(),
            "..", "Storage", "App_Data", "Books", "InMemory.xml");

        public InMemoryBooksService() {
            ReadData();
        }

        public void Dispose() {
            SaveData();
        }

        private void ReadData() {
            data.ReadXml(xmlFilePath, XmlReadMode.ReadSchema);
        }

        private void SaveData() {
            data.WriteXml(xmlFilePath, XmlWriteMode.WriteSchema);
        }

        public Task<Book?> GetBookAsync(int id) {
            DataRow? bookRow = data.Tables["Book"]?.AsEnumerable()
                .FirstOrDefault(row => row.Field<int>("Id") == id);

            if (bookRow == null) return Task.FromResult<Book?>(null);

            Book book = ConvertRowToBook(bookRow);

            DataTable? chapters = data.Tables["Chapter"];
            if (chapters != null) {
                book.Chapters = [.. 
                    chapters.AsEnumerable()
                        .Where(row => row.Field<int>("BookId") == id)
                        .Select(ConvertRowToChapter)
                ];
            }

            return Task.FromResult<Book?>(book);
        }

        public async Task<List<Book>> GetAllBooksAsync() {
            List<Book> books = data.Tables["Book"]?.AsEnumerable()
                .Select(ConvertRowToBook)
                .ToList() ?? [];

            books = (await Task.WhenAll(books.Select(book => GetBookAsync(book.Id))))
                        .Where(book => book != null)
                        .Cast<Book>()
                        .ToList();

            return books;
        }

        public Task CreateBookAsync(Book newBook) {
            DataTable? booksTable = data.Tables["Book"];
            DataTable? chaptersTable = data.Tables["Chapter"];
            if (booksTable == null || chaptersTable == null) return Task.CompletedTask;

            DataRow newRow = booksTable.NewRow();
            UpdateRow(newRow, newBook);
            booksTable.Rows.Add(newRow);

            foreach (Chapter chapter in newBook.Chapters) {
                DataRow newChapterRow = chaptersTable.NewRow();
                UpdateChapterRow(newChapterRow, newBook.Id, chapter);
                chaptersTable.Rows.Add(newChapterRow);
            }

            return Task.CompletedTask;
        }

        public Task UpdateBookAsync(Book updatedBook) {
            DataTable? booksTable = data.Tables["Book"];
            DataTable? chaptersTable = data.Tables["Chapter"];
            if (booksTable == null || chaptersTable == null) return Task.CompletedTask;

            DataRow? bookRow = booksTable.AsEnumerable()
                .FirstOrDefault(row => row.Field<int>("Id") == updatedBook.Id);

            if (bookRow != null) {
                UpdateRow(bookRow, updatedBook);

                var existingChapters = chaptersTable.AsEnumerable()
                    .Where(row => row.Field<int>("BookId") == updatedBook.Id);
                foreach (var chapterRow in existingChapters) {
                    chapterRow.Delete();
                }

                foreach (var chapter in updatedBook.Chapters) {
                    DataRow newChapterRow = chaptersTable.NewRow();
                    UpdateChapterRow(newChapterRow, updatedBook.Id, chapter);
                    chaptersTable.Rows.Add(newChapterRow);
                }
            }

            return Task.CompletedTask;
        }

        public Task DeleteBookAsync(int id) {
            DataTable? booksTable = data.Tables["Book"];
            DataTable? chaptersTable = data.Tables["Chapter"];
            if (booksTable == null || chaptersTable == null) return Task.CompletedTask;

            DataRow? bookRow = booksTable.AsEnumerable()
                .FirstOrDefault(row => row.Field<int>("Id") == id);

            if (bookRow != null) {
                bookRow.Delete();

                chaptersTable.AsEnumerable()
                    .Where(row => row.Field<int>("BookId") == id)
                    .ToList()
                    .ForEach(chapter => chapter.Delete());
            }

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

        private static void UpdateChapterRow(DataRow row, int bookId, Chapter chapter) {
            row["BookId"] = bookId;
            row["Index"] = chapter.Index;
            row["Title"] = chapter.Title;
            row["Content"] = chapter.Content;
        }
    }
}
