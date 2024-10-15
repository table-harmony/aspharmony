using Xceed.Words.NET;
using System.Text.RegularExpressions;

namespace BusinessLogicLayer.Servers.Books.Documents {
    public class WordDocumentStorage(string filePath) : IDocumentStorage {
        public void Save(List<Book> books) {
            using var doc = DocX.Create(filePath);
            doc.InsertParagraph("Books List").FontSize(18).Bold();

            foreach (var book in books) {
                doc.InsertParagraph($"Book ID: {book.Id}").FontSize(14).Bold();
                doc.InsertParagraph($"Title: {book.Title}").FontSize(12);
                doc.InsertParagraph($"Description: {book.Description}").FontSize(12);
                doc.InsertParagraph($"Image URL: {book.ImageUrl}").FontSize(12);

                if (book.Chapters != null && book.Chapters.Any()) {
                    doc.InsertParagraph("Chapters:").Bold();
                    foreach (var chapter in book.Chapters) {
                        doc.InsertParagraph($"  Chapter {chapter.Index}: {chapter.Title}")
                            .FontSize(12).Italic();
                        doc.InsertParagraph($"  Content: {chapter.Content}").FontSize(12);
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

            foreach (var paragraph in doc.Paragraphs) {
                var text = paragraph.Text.Trim();

                if (text.StartsWith("Book ID:")) {
                    if (currentBook != null) {
                        if (currentChapter != null) currentBook.Chapters!.Add(currentChapter);
                        books.Add(currentBook);
                    }

                    currentBook = new Book {
                        Id = int.Parse(Regex.Match(text, @"\d+").Value),
                        Chapters = new List<Chapter>()
                    };
                    currentChapter = null;
                } else if (text.StartsWith("Title:")) {
                    currentBook!.Title = text.Replace("Title:", "").Trim();
                } else if (text.StartsWith("Description:")) {
                    currentBook!.Description = text.Replace("Description:", "").Trim();
                } else if (text.StartsWith("Image URL:")) {
                    currentBook!.ImageUrl = text.Replace("Image URL:", "").Trim();
                } else if (text.StartsWith("Chapter")) {
                    var match = Regex.Match(text, @"\d+");
                    if (match.Success) {
                        if (currentChapter != null) currentBook!.Chapters!.Add(currentChapter);

                        var chapterTitle = text.Split(new[] { ':' }, 2).Last().Trim();
                        currentChapter = new Chapter {
                            Index = int.Parse(match.Value),
                            Title = chapterTitle
                        };
                    }
                } else if (text.StartsWith("Content:")) {
                    currentChapter!.Content = text.Replace("Content:", "").Trim();
                }
            }

            if (currentBook != null) {
                if (currentChapter != null) currentBook.Chapters!.Add(currentChapter);
                books.Add(currentBook);
            }

            return books;
        }
    }

}
