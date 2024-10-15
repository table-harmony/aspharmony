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
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Books");

            int row = 1;
            foreach (var book in books) {
                worksheet.Cells[row, 1].Value = "Book ID";
                worksheet.Cells[row, 2].Value = book.Id;
                worksheet.Cells[++row, 1].Value = "Title";
                worksheet.Cells[row, 2].Value = book.Title;
                worksheet.Cells[++row, 1].Value = "Description";
                worksheet.Cells[row, 2].Value = book.Description;
                worksheet.Cells[++row, 1].Value = "Image URL";
                worksheet.Cells[row, 2].Value = book.ImageUrl;

                if (book.Chapters != null && book.Chapters.Any()) {
                    worksheet.Cells[++row, 1].Value = "Chapters";
                    foreach (var chapter in book.Chapters) {
                        worksheet.Cells[++row, 1].Value = $"Chapter {chapter.Index}: {chapter.Title}";
                        worksheet.Cells[row, 2].Value = chapter.Content;
                    }
                }

                row += 2; // Add space between books
            }

            package.SaveAs(new FileInfo(filePath));
        }

        public List<Book> Load() {
            var books = new List<Book>();

            if (!File.Exists(filePath)) return books;

            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets[0];
            int row = 1;
            Book? currentBook = null;

            while (worksheet.Cells[row, 1].Text != "") {
                if (worksheet.Cells[row, 1].Text == "Book ID") {
                    if (currentBook != null) books.Add(currentBook);

                    currentBook = new Book {
                        Id = SafeParseInt(worksheet.Cells[row, 2].Text),
                        Chapters = new List<Chapter>()
                    };
                } else if (worksheet.Cells[row, 1].Text == "Title") {
                    currentBook!.Title = worksheet.Cells[row, 2].Text;
                } else if (worksheet.Cells[row, 1].Text == "Description") {
                    currentBook!.Description = worksheet.Cells[row, 2].Text;
                } else if (worksheet.Cells[row, 1].Text == "Image URL") {
                    currentBook!.ImageUrl = worksheet.Cells[row, 2].Text;
                } else if (worksheet.Cells[row, 1].Text.StartsWith("Chapter")) {
                    var chapterParts = worksheet.Cells[row, 1].Text.Split(":");
                    var chapterIndex = SafeParseInt(Regex.Match(chapterParts[0], @"\d+").Value);
                    var chapter = new Chapter {
                        Index = chapterIndex,
                        Title = chapterParts.Length > 1 ? chapterParts[1].Trim() : "",
                        Content = worksheet.Cells[row, 2].Text
                    };
                    currentBook!.Chapters!.Add(chapter);
                }

                row++;
            }

            if (currentBook != null) books.Add(currentBook);

            return books;
        }

        // Helper method to safely parse integers, returning 0 if invalid
        private int SafeParseInt(string? text) {
            if (int.TryParse(text, out var result)) {
                return result;
            }

            // Default value or handle the invalid case here
            return 0;
        }
    }

}
