using DocumentFormat.OpenXml.Wordprocessing;
using Syncfusion.Presentation;
using Presentation = Syncfusion.Presentation.Presentation;

namespace BusinessLogicLayer.Servers.Books.Documents {
    public class PowerPointStorage(string filePath) : IDocumentStorage {
        public List<Book> Load() {
            List<Book> books = [];

            if (!File.Exists(filePath)) {
                return books;
            }

            using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using IPresentation pptxDoc = Presentation.Open(fileStream);

            foreach (ISlide slide in pptxDoc.Slides) { 
                    Book book = new();

                foreach (IShape shape in slide.Shapes.Cast<IShape>()) {
                    var textBody = shape.TextBody;

                    if (textBody == null)
                        continue;

                    string text = textBody.Text;

                    if (text.StartsWith("Title: "))
                        book.Title = text[7..];
                    if (text.StartsWith("Id: "))
                        book.Id = int.Parse(text[4..]);
                    else if (text.StartsWith("Description: "))
                        book.Description = text[13..];
                    else if (text.StartsWith("ImageUrl: "))
                        book.ImageUrl = text[10..];
                    else if (text.StartsWith("Chapter: ")) {
                        string[] parts = text[9..].Split('|');
                        if (parts.Length == 3) {
                            book.Chapters.Add(new Chapter {
                                Index = int.Parse(parts[0]),
                                Title = parts[1],
                                Content = parts[2]
                            });
                        }
                    }
                }

                books.Add(book);
            }

            return books;
        }

        public void Save(List<Book> books) {
            using IPresentation pptxDoc = Presentation.Create();
            foreach (Book book in books) {
                ISlide slide = pptxDoc.Slides.Add(SlideLayoutType.Blank);

                AddTextShape(slide, $"Id: {book.Id}", 50, 50);
                AddTextShape(slide, $"Title: {book.Title}", 50, 100);
                AddTextShape(slide, $"Description: {book.Description}", 50, 150);
                AddTextShape(slide, $"ImageUrl: {book.ImageUrl}", 50, 200);

                float y = 250;
                foreach (Chapter chapter in book.Chapters) {
                    AddTextShape(slide, $"Chapter: {chapter.Index}|{chapter.Title}|{chapter.Content}", 50, y);
                    y += 50;
                }
            }

            using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write);
            pptxDoc.Save(fileStream);
        }

        private static void AddTextShape(ISlide slide, string text, float x, float y) {
            IShape shape = slide.AddTextBox(x, y, 500, 50);
            ITextBody textBody = shape.TextBody;
            IParagraph paragraph = textBody.AddParagraph();
            ITextPart textPart = paragraph.AddTextPart(text);
            textPart.Font.FontSize = 12;
        }
    }
}