using OfficeOpenXml;
using System.Text.RegularExpressions;

namespace BusinessLogicLayer.Servers.Books.Documents {

    public class ExcelDocumentStorage : IDocumentStorage {
        private readonly string filePath;

        public ExcelDocumentStorage(string filePath) {
            this.filePath = filePath;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public void Save(List<Book> books) {
            using ExcelPackage package = new();
            var worksheet = package.Workbook.Worksheets.Add("Books");

            int row = 1;
            foreach (var book in books) {
                SaveBookToWorksheet(worksheet, ref row, book);
                row += 2;
            }

            package.SaveAs(new FileInfo(filePath));
        }

        private static void SaveBookToWorksheet(ExcelWorksheet worksheet, ref int row, Book book) {
            AddCell(worksheet, row, 1, "Book ID", book.Id);
            AddCell(worksheet, ++row, 1, "Title", book.Title);
            AddCell(worksheet, ++row, 1, "Description", book.Description);
            AddCell(worksheet, ++row, 1, "Image URL", book.ImageUrl);

            if (book.Chapters != null && book.Chapters.Count != 0) {
                worksheet.Cells[++row, 1].Value = "Chapters";
                foreach (var chapter in book.Chapters) {
                    AddCell(worksheet, ++row, 1, $"Chapter {chapter.Index}: {chapter.Title}", chapter.Content);
                }
            }
        }

        private static void AddCell(ExcelWorksheet worksheet, int row, int col, string label, object value) {
            worksheet.Cells[row, col].Value = label;
            worksheet.Cells[row, col + 1].Value = value;
        }

        public List<Book> Load() {
            List<Book> books = [];

            if (!File.Exists(filePath)) return books;

            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets[0];

            int row = 1;
            Book? currentBook = null;

            while (!string.IsNullOrEmpty(worksheet.Cells[row, 1].Text)) {
                ProcessRow(worksheet, row, ref currentBook, books);
                row++;
            }

            if (currentBook != null) books.Add(currentBook);

            return books;
        }

        private static void ProcessRow(ExcelWorksheet worksheet, int row, ref Book? currentBook, List<Book> books) {
            string cell = worksheet.Cells[row, 1].Text;
            string value = worksheet.Cells[row, 2].Text;

            switch (cell) {
                case "Book ID":
                    if (currentBook != null) books.Add(currentBook);
                    currentBook = new Book {
                        Id = SafeParseInt(value),
                        Chapters = []
                    };
                    break;
                case "Title":
                    if (currentBook != null) currentBook.Title = value;
                    break;
                case "Description":
                    if (currentBook != null) currentBook.Description = value;
                    break;
                case "Image URL":
                    if (currentBook != null) currentBook.ImageUrl = value;
                    break;
                case string s when s.StartsWith("Chapter"):
                    if (currentBook != null && !string.IsNullOrWhiteSpace(value))
                        AddChapterToBook(currentBook, cell, value);
                    break;
            }
        }

        private static void AddChapterToBook(Book book, string chapterInfo, string content) {
            var chapterParts = chapterInfo.Split(":");
            int chapterIndex = SafeParseInt(Regex.Match(chapterParts[0], @"\d+").Value);

            Chapter chapter = new() {
                Index = chapterIndex,
                Title = chapterParts.Length > 1 ? chapterParts[1].Trim() : "",
                Content = content
            };

            book.Chapters?.Add(chapter);
        }

        private static int SafeParseInt(string? text) {
            return int.TryParse(text, out var result) ? result : 0;
        }
    }
}
