using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Presentation;
using System.Text.RegularExpressions;

namespace BusinessLogicLayer.Servers.Books.Documents {

    public class PowerPointStorage(string filePath) : IDocumentStorage {

        // Create or open the PowerPoint presentation file
        private PresentationDocument CreateOrOpenPresentation() {
            if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0) {
                return PresentationDocument.Create(filePath, PresentationDocumentType.Presentation);
            }
            return PresentationDocument.Open(filePath, true);
        }

        // Ensure slide master and layout are present
        private static void EnsureSlideMasterAndLayout(PresentationPart presentationPart) {
            if (presentationPart.SlideMasterParts.Count() == 0) {
                var slideMasterPart = presentationPart.AddNewPart<SlideMasterPart>();
                var slideMaster = new SlideMaster(new CommonSlideData(new ShapeTree()));
                slideMaster.Save(slideMasterPart);

                var slideLayoutPart = slideMasterPart.AddNewPart<SlideLayoutPart>();
                var slideLayout = new SlideLayout(new CommonSlideData(new ShapeTree()));
                slideLayout.Save(slideLayoutPart);

                if (presentationPart.Presentation.SlideMasterIdList == null) {
                    presentationPart.Presentation.SlideMasterIdList = new SlideMasterIdList(
                        new SlideMasterId() {
                            Id = 2147483648U,
                            RelationshipId = presentationPart.GetIdOfPart(slideMasterPart)
                        }
                    );
                }
            }
        }

        // Save book data to a PowerPoint file
        public void Save(List<Book> books) {
            using var presentation = CreateOrOpenPresentation();
            var presentationPart = presentation.PresentationPart ?? presentation.AddPresentationPart();

            presentationPart.Presentation ??= new Presentation();

            EnsureSlideMasterAndLayout(presentationPart);

            // Remove existing slides to prevent duplicates
            if (presentationPart.Presentation.SlideIdList != null) {
                presentationPart.Presentation.SlideIdList.RemoveAllChildren();
            }

            uint slideId = 256;
            foreach (var book in books) {
                AddBookSlide(presentationPart, book, ref slideId);
            }

            // Save the entire presentation after modifications
            presentationPart.Presentation.Save();
        }

        // Add book details to a slide
        private void AddBookSlide(PresentationPart presentationPart, Book book, ref uint slideId) {
            var slidePart = presentationPart.AddNewPart<SlidePart>();
            var slide = new Slide(new CommonSlideData(new ShapeTree()));
            slide.Save(slidePart);

            if (presentationPart.Presentation.SlideIdList == null) {
                presentationPart.Presentation.SlideIdList = new SlideIdList();
            }

            presentationPart.Presentation.SlideIdList.Append(new SlideId() {
                Id = slideId,
                RelationshipId = presentationPart.GetIdOfPart(slidePart)
            });

            AddTextToSlide(slidePart, $"Book ID: {book.Id}", 0);
            AddTextToSlide(slidePart, $"Title: {book.Title}", 1);
            AddTextToSlide(slidePart, $"Description: {book.Description}", 2);
            AddTextToSlide(slidePart, $"Image URL: {book.ImageUrl}", 3);

            if (book.Chapters != null && book.Chapters.Any()) {
                AddTextToSlide(slidePart, "Chapters:", 4);
                int index = 5;
                foreach (var chapter in book.Chapters) {
                    AddTextToSlide(slidePart, $"Chapter {chapter.Index}: {chapter.Title}", index++);
                    AddTextToSlide(slidePart, $"Content: {chapter.Content}", index++);
                }
            }

            // Save the slide content back to the slide part
            slide.Save(slidePart);
            slideId++;
        }

        // Load book data from a PowerPoint file
        public List<Book> Load() {
            var books = new List<Book>();

            if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0) {
                return books;
            }

            using var presentation = PresentationDocument.Open(filePath, false);
            var presentationPart = presentation.PresentationPart;
            if (presentationPart == null) return books;

            foreach (var slidePart in presentationPart.SlideParts) {
                var book = ParseSlideToBook(slidePart);
                if (book != null) {
                    books.Add(book);
                }
            }

            return books;
        }

        // Parse a slide back to a Book object
        private Book? ParseSlideToBook(SlidePart slidePart) {
            var book = new Book { Chapters = new List<Chapter>() };

            foreach (var paragraph in slidePart.Slide.Descendants<DocumentFormat.OpenXml.Drawing.Paragraph>()) {
                var text = paragraph.InnerText;
                if (text.StartsWith("Book ID:")) {
                    book.Id = SafeParseInt(Regex.Match(text, @"\d+").Value);
                } else if (text.StartsWith("Title:")) {
                    book.Title = text.Replace("Title:", "").Trim();
                } else if (text.StartsWith("Description:")) {
                    book.Description = text.Replace("Description:", "").Trim();
                } else if (text.StartsWith("Image URL:")) {
                    book.ImageUrl = text.Replace("Image URL:", "").Trim();
                } else if (text.StartsWith("Chapter")) {
                    var chapterIndex = SafeParseInt(Regex.Match(text, @"\d+").Value);
                    var chapterTitle = text.Split(":").Length > 1 ? text.Split(":")[1].Trim() : "";
                    var chapter = new Chapter {
                        Index = chapterIndex,
                        Title = chapterTitle
                    };
                    book.Chapters.Add(chapter);
                } else if (text.StartsWith("Content:")) {
                    book.Chapters.LastOrDefault()!.Content = text.Replace("Content:", "").Trim();
                }
            }

            return book.Chapters.Count > 0 ? book : null;
        }

        // Helper method to add text to a slide
        private static void AddTextToSlide(SlidePart slidePart, string text, int position) {
            var slide = slidePart.Slide;
            var shapeTree = slide.CommonSlideData.ShapeTree;

            // Create a new shape (text box)
            var shape = new Shape();
            shape.Append(new NonVisualShapeProperties(
                new NonVisualDrawingProperties() { Id = (UInt32Value)(uint)(position + 1), Name = $"TextBox {position}" },
                new NonVisualShapeDrawingProperties(new DocumentFormat.OpenXml.Drawing.ShapeLocks() { NoGrouping = true }),
                new ApplicationNonVisualDrawingProperties()
            ));

            shape.Append(new ShapeProperties());

            // Create the text body for the shape
            var textBody = new TextBody(new DocumentFormat.OpenXml.Drawing.BodyProperties(), new DocumentFormat.OpenXml.Drawing.ListStyle());

            var paragraph = new DocumentFormat.OpenXml.Drawing.Paragraph();
            var run = new DocumentFormat.OpenXml.Drawing.Run();
            run.Append(new DocumentFormat.OpenXml.Drawing.Text(text));

            paragraph.Append(run);
            textBody.Append(paragraph);

            shape.Append(textBody);
            shapeTree.Append(shape);
        }

        // Helper method to safely parse integers, returning 0 if invalid
        private static int SafeParseInt(string? text) {
            return int.TryParse(text, out var result) ? result : 0;
        }
    }
}
