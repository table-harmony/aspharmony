using Xceed.Words.NET;
using HyperLink = Xceed.Document.NET.Hyperlink;

namespace BusinessLogicLayer.Servers.Books.Documents {
    public partial class WordDocumentStorage(string filePath) : IDocumentStorage {
        public void Save(List<Book> books) {
            using var doc = DocX.Create(filePath);
            doc.InsertParagraph("Books List").FontSize(18).Bold();

            foreach (var book in books) {
                doc.InsertParagraph($"Book ID: {book.Id}").FontSize(14).Bold();
                doc.InsertParagraph($"Title: {book.Title}").FontSize(12);
                doc.InsertParagraph($"Description: {book.Description}").FontSize(12);

                HyperLink link = doc.AddHyperlink(book.ImageUrl, new Uri(book.ImageUrl));
                doc.InsertParagraph($"Image URL: ").AppendHyperlink(link).FontSize(12);

                if (book.Chapters != null && book.Chapters.Count != 0) {
                    doc.InsertParagraph("Chapters:").Bold();
                    foreach (var chapter in book.Chapters) {
                        doc.InsertParagraph($"Chapter: {chapter.Index}").FontSize(12).Italic();
                        doc.InsertParagraph($"  Chapter Title: {chapter.Title}").FontSize(10);
                        doc.InsertParagraph($"  Chapter Content: {chapter.Content}").FontSize(10);
                    }
                }
                doc.InsertParagraph();
            }

            doc.Save();
        }

        public List<Book> Load() {
            if (!File.Exists(filePath)) return [];

            using var doc = DocX.Load(filePath);
            List<Book> books = [];

            Book? currentBook = null;
            Chapter? currentChapter = null;

            void AddChapterToBook() {
                if (currentBook != null && currentChapter != null) {
                    currentBook.Chapters!.Add(currentChapter);
                }
            }

            foreach (var paragraph in doc.Paragraphs) {
                var text = paragraph.Text.Trim();
                var (key, value) = ExtractKeyValue(text);

                switch (key) {
                    case "Book ID":
                        currentBook = new Book { Id = SafeParseInt(value), Chapters = [] };
                        books.Add(currentBook);
                        currentChapter = null;
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
                    case "Chapter":
                        currentChapter = new Chapter { Index = SafeParseInt(value) };
                        AddChapterToBook();
                        break;
                    case "Chapter Title":
                        if (currentChapter != null) currentChapter.Title = value;
                        break;
                    case "Chapter Content":
                        if (currentChapter != null) currentChapter.Content = value;
                        break;
                }
            }

            return books;
        }

        private static (string key, string value) ExtractKeyValue(string text) {
            var parts = text.Split([':'], 2);
            return parts.Length > 1 ? (parts[0].Trim(), parts[1].Trim()) : (parts[0].Trim(), string.Empty);
        }

        private static int SafeParseInt(string? text) {
            return int.TryParse(text, out var result) ? result : 0;
        }
    }
}
