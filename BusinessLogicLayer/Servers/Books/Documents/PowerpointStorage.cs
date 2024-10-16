using Syncfusion.Presentation;
using System.Drawing;
using System.Security.Cryptography;

namespace BusinessLogicLayer.Servers.Books.Documents {
    public enum SlideType {
        Book,
        Chapter,
        Unknown
    }

    public class PowerPointStorage(string filePath) : IDocumentStorage {

        public List<Book> Load() {
            List<Book> books = [];

            if (!File.Exists(filePath)) {
                return books;
            }

            using IPresentation presentation = Presentation.Open(filePath);

            foreach (ISlide slide in presentation.Slides) {
                SlideType slideType = GetType(slide);

                switch (slideType) {
                    case SlideType.Book:
                        books.Add(ExtractBook(slide)!);
                        break;
                    case SlideType.Chapter:
                        PresentationChapter chapter = ExtractChapter(slide)!;
                        Book? book = books.FirstOrDefault(book => book.Id == chapter.BookId);
                        book?.Chapters.Add(chapter);
                        break;
                }
            }

            return books;
        }

        public void Save(List<Book> books) {
            using IPresentation presentation = Presentation.Create();

            foreach (Book book in books) {
                CreateSlide(presentation, book);

                book.Chapters?.ForEach(chapter =>
                    CreateSlide(presentation, new PresentationChapter() {
                        BookId = book.Id,
                        Index = chapter.Index,
                        Title = chapter.Title,
                        Content = chapter.Content,
                    })
                );
            }

            presentation.Save(filePath);
        }


        private static SlideType GetType(ISlide slide) {
            if (slide.Shapes.Count > 0 && slide.Shapes[0] is IShape shape && shape.TextBody != null) {
                string text = shape.TextBody.Text;
            
                if (text.StartsWith("Book")) return SlideType.Book;
                if (text.StartsWith("Chapter")) return SlideType.Chapter;
            }

            return SlideType.Unknown;
        }

        private static Book? ExtractBook(ISlide slide) {
            SlideType slideType = GetType(slide);

            if (slideType != SlideType.Book)
                return null;

            Book book = new() {
                Chapters = []
            };

            foreach (IShape shape in slide.Shapes.Cast<IShape>()) {
                if (shape.TextBody == null)
                    continue;

                string[] parts = shape.TextBody.Text.Split(':', 2);

                if (parts.Length != 2)
                    continue;

                string key = parts[0].Trim();
                string value = parts[1].Trim();

                switch (key) {
                    case "Id": book.Id = int.Parse(value); break;
                    case "Title": book.Title = value; break;
                    case "Description": book.Description = value; break;
                    case "ImageUrl": book.ImageUrl = value; break;
                }
            }

            return book;
        }

        private static PresentationChapter? ExtractChapter(ISlide slide) {
            SlideType slideType = GetType(slide);

            if (slideType != SlideType.Chapter)
                return null;

            PresentationChapter chapter = new();

            foreach (IShape shape in slide.Shapes.Cast<IShape>()) {
                if (shape.TextBody == null)
                    continue;

                string[] parts = shape.TextBody.Text.Split(':', 2);

                if (parts.Length != 2)
                    continue;

                string key = parts[0].Trim();
                string value = parts[1].Trim();

                switch (key) {
                    case "Book Id": chapter.BookId = int.Parse(value); break;
                    case "Index": chapter.Index = int.Parse(value); break;
                    case "Title": chapter.Title = value; break;
                    case "Content": chapter.Content = value; break;
                }
            }

            return chapter;
        }


        private static void CreateSlide(IPresentation presentation, Book book) {
            ISlide slide = presentation.Slides.Add(SlideLayoutType.Blank);

            CreateShape(slide, new TextConfiguration {
                Text = "Book",
                Style = new TextStyle { FontSize = 28, Bold = true },
                Position = (50, 50)
            });
            CreateShape(slide, new TextConfiguration {
                Text = $"Id: {book.Id}",
                Style = new TextStyle { FontColor = GenerateColor(book.Id) },
                Position = (50, 100)
            });
            CreateShape(slide, new TextConfiguration {
                Text = $"Title: {book.Title}",
                Style = new TextStyle { FontSize = 24, Bold = true },
                Position = (50, 150)
            });
            CreateShape(slide, new TextConfiguration {
                Text = $"Description: {book.Description}",
                Position = (50, 200)
            });
            CreateShape(slide, new TextConfiguration {
                Text = $"ImageUrl: {book.ImageUrl}",
                Style = new TextStyle { Hyperlink = book.ImageUrl },
                Position = (50, 250)
            });
        }

        private static void CreateSlide(IPresentation presentation, PresentationChapter chapter) {
            ISlide slide = presentation.Slides.Add(SlideLayoutType.Blank);

            CreateShape(slide, new TextConfiguration {
                Text = $"Chapter: {chapter.Index}",
                Style = new TextStyle { FontSize = 28, Bold = true },
                Position = (50, 50)
            });
            CreateShape(slide, new TextConfiguration {
                Text = $"Book Id: {chapter.BookId}",
                Style = new TextStyle { FontColor = GenerateColor(chapter.BookId) },
                Position = (50, 100)
            });
            CreateShape(slide, new TextConfiguration {
                Text = $"Title: {chapter.Title}",
                Style = new TextStyle { FontSize = 24, Bold = true },
                Position = (50, 150)
            });
            CreateShape(slide, new TextConfiguration {
                Text = $"Content: {chapter.Content}",
                Position = (50, 200)
            });
        }

        private static void CreateShape(ISlide slide, TextConfiguration configuration) {
            (double x, double y) = configuration.Position;
            (double width, double height) = configuration.Dimensions;

            IShape shape = slide.AddTextBox(x, y, width, height);

            ITextBody textBody = shape.TextBody;
            IParagraph paragraph = textBody.AddParagraph();

            var text = paragraph.AddTextPart(configuration.Text);

            text.Font.FontSize = configuration.Style.FontSize;
            text.Font.Bold = configuration.Style.Bold;
            text.Font.Italic = configuration.Style.Italic;
            text.Font.Color = ColorObject.FromArgb(configuration.Style.FontColor.R, configuration.Style.FontColor.G, configuration.Style.FontColor.B);

            if (!string.IsNullOrEmpty(configuration.Style.Hyperlink)) {
                text.SetHyperlink(configuration.Style.Hyperlink);
            }
        }

        private static Color GenerateColor(int number) {
            byte[] hash = SHA256.HashData(BitConverter.GetBytes(number));

            int r = hash[0];    
            int g = hash[8];   
            int b = hash[16];

            return Color.FromArgb(r, g, b); 
        }
    }

    class PresentationChapter : Chapter {
        public int BookId { get; set; }
    }

    public class TextStyle {
        public int FontSize { get; set; } = 18;
        public bool Bold { get; set; } = false;
        public bool Italic { get; set; } = false;
        public Color FontColor { get; set; } = Color.Black;
        public string? Hyperlink { get; set; } = null;
    }

    public class TextConfiguration {
        public TextStyle Style { get; set; } = new TextStyle();
        public string Text { get; set; } = "";
        public (double, double) Dimensions { get; set; } = (500, 50);
        public (double, double) Position { get; set; }
    }
}