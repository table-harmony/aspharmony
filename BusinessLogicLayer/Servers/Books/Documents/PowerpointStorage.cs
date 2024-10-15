using P = DocumentFormat.OpenXml.Presentation;
using D = DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using System.IO;

namespace BusinessLogicLayer.Servers.Books.Documents {

    public class PowerPointDocumentSaver : IDocumentStorage {
        private readonly string filePath;

        public PowerPointDocumentSaver(string filePath) {
            this.filePath = filePath;
        }

        // Create or open the presentation
        private PresentationDocument CreateOrOpenPresentation() {
            if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0) {
                return PresentationDocument.Create(filePath, PresentationDocumentType.Presentation);
            }
            return PresentationDocument.Open(filePath, true);
        }

        // Ensure slide master and layout
        private void EnsureSlideMasterAndLayout(PresentationPart presentationPart) {
            if (presentationPart.SlideMasterParts.Count() == 0) {
                var slideMasterPart = presentationPart.AddNewPart<SlideMasterPart>();
                var slideMaster = new P.SlideMaster(new P.CommonSlideData(new P.ShapeTree()));
                slideMaster.Save(slideMasterPart);

                var slideLayoutPart = slideMasterPart.AddNewPart<SlideLayoutPart>();
                var slideLayout = new P.SlideLayout(new P.CommonSlideData(new P.ShapeTree()));
                slideLayout.Save(slideLayoutPart);

                if (presentationPart.Presentation.SlideMasterIdList == null) {
                    presentationPart.Presentation.SlideMasterIdList = new P.SlideMasterIdList(
                        new P.SlideMasterId() {
                            Id = 2147483648U,
                            RelationshipId = presentationPart.GetIdOfPart(slideMasterPart)
                        }
                    );
                }
            }
        }

        public void Save(List<Book> books) {
            using var presentation = CreateOrOpenPresentation();
            var presentationPart = presentation.PresentationPart ?? presentation.AddPresentationPart();

            if (presentationPart.Presentation == null)
                presentationPart.Presentation = new P.Presentation();

            EnsureSlideMasterAndLayout(presentationPart);

            var existingSlides = presentationPart.SlideParts.ToList();
            var slideIdList = presentationPart.Presentation.SlideIdList ?? new P.SlideIdList();

            uint slideId = 256;

            foreach (var book in books) {
                // Check if a slide for the current book already exists, and remove it for replacement
                var slidePartToRemove = FindSlidePartForBook(existingSlides, book.Id);

                if (slidePartToRemove != null) {
                    // Remove the old slide and its reference in SlideIdList
                    RemoveSlide(slidePartToRemove, slideIdList);
                }

                // Add a new slide for the book
                AddBookSlide(presentationPart, book, ref slideId, slideIdList);
            }

            presentationPart.Presentation.Save();
        }

        // Find slide by book ID (based on text content)
        private SlidePart? FindSlidePartForBook(List<SlidePart> slideParts, int bookId) {
            foreach (var slidePart in slideParts) {
                var texts = slidePart.Slide.Descendants<D.Text>().Select(t => t.Text).ToList();
                if (texts.Any(text => text.Contains($"Book ID: {bookId}"))) {
                    return slidePart;
                }
            }
            return null;
        }

        // Remove slide and its reference in the SlideIdList
        private void RemoveSlide(SlidePart slidePart, P.SlideIdList slideIdList) {
            var relationshipId = slidePart.GetIdOfPart(slidePart);

            // Remove the SlideId from SlideIdList in the presentation
            var slideToRemove = slideIdList.Elements<P.SlideId>().FirstOrDefault(s => s.RelationshipId == relationshipId);
            if (slideToRemove != null) {
                slideToRemove.Remove();
            }

            // Remove the slide part from the presentation
            var presentationPart = (PresentationPart)slidePart.OpenXmlPackage.GetPartById(relationshipId); // Get PresentationPart
            presentationPart.DeletePart(slidePart); // Delete slide part
        }


        private void AddBookSlide(PresentationPart presentationPart, Book book, ref uint slideId, P.SlideIdList slideIdList) {
            var slidePart = presentationPart.AddNewPart<SlidePart>();
            var slide = new P.Slide(new P.CommonSlideData(new P.ShapeTree()));
            slide.Save(slidePart);

            slideIdList.Append(new P.SlideId() {
                Id = (UInt32Value)slideId, // Convert int to UInt32Value
                RelationshipId = presentationPart.GetIdOfPart(slidePart)
            });

            // Add book details
            AddTextToSlide(slidePart, $"Book ID: {book.Id}", 0);
            AddTextToSlide(slidePart, $"Title: {book.Title}", 1);
            AddTextToSlide(slidePart, $"Description: {book.Description}", 2);
            AddTextToSlide(slidePart, $"Image URL: {book.ImageUrl}", 3);

            // Add chapters
            if (book.Chapters != null && book.Chapters.Any()) {
                AddTextToSlide(slidePart, "Chapters:", 4);
                int index = 5;
                foreach (var chapter in book.Chapters) {
                    AddTextToSlide(slidePart, $"Chapter {chapter.Index}: {chapter.Title}", index++);
                    AddTextToSlide(slidePart, $"Content: {chapter.Content}", index++);
                }
            }

            slideId++;
        }

        // Helper method to add text to a slide
        private static void AddTextToSlide(SlidePart slidePart, string text, int position) {
            var slide = slidePart.Slide;
            var shapeTree = slide.CommonSlideData.ShapeTree;

            // Create a new shape (text box)
            var shape = new P.Shape();
            shape.Append(new P.NonVisualShapeProperties(
                new P.NonVisualDrawingProperties() { Id = (UInt32Value)(uint)(position + 1), Name = $"TextBox {position}" },
                new P.NonVisualShapeDrawingProperties(new D.ShapeLocks() { NoGrouping = true }),
                new P.ApplicationNonVisualDrawingProperties()
            ));

            shape.Append(new P.ShapeProperties());

            // Create the text body for the shape
            var textBody = new P.TextBody(new D.BodyProperties(), new D.ListStyle());

            var paragraph = new D.Paragraph();
            var run = new D.Run();
            run.Append(new D.Text(text));

            paragraph.Append(run);
            textBody.Append(paragraph);

            shape.Append(textBody);
            shapeTree.Append(shape);
        }
        public List<Book> Load() {
            var books = new List<Book>();

            if (!File.Exists(filePath)) return books;

            using var presentation = PresentationDocument.Open(filePath, false);
            var presentationPart = presentation.PresentationPart;
            if (presentationPart == null) return books;

            Book? currentBook = null;
            Chapter? currentChapter = null;

            foreach (var slidePart in presentationPart.SlideParts) {
                var texts = slidePart.Slide.Descendants<P.Text>().Select(t => t.Text).ToList();

                foreach (var text in texts) {
                    if (text.StartsWith("Book ID:")) {
                        if (currentBook != null) {
                            if (currentChapter != null) currentBook.Chapters!.Add(currentChapter);
                            books.Add(currentBook);
                        }

                        currentBook = new Book {
                            Id = int.TryParse(text.Replace("Book ID:", "").Trim(), out var id) ? id : 0,
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
                        var chapterParts = text.Split(new[] { ':' }, 2);
                        var chapterIndexMatch = Regex.Match(chapterParts[0], @"\d+");
                        var chapterIndex = chapterIndexMatch.Success ? int.Parse(chapterIndexMatch.Value) : 0;
                        currentChapter = new Chapter {
                            Index = chapterIndex,
                            Title = chapterParts.Length > 1 ? chapterParts[1].Trim() : ""
                        };
                    } else if (text.StartsWith("Content:")) {
                        currentChapter!.Content = text.Replace("Content:", "").Trim();
                        if (currentBook != null && currentChapter != null) {
                            currentBook.Chapters!.Add(currentChapter);
                            currentChapter = null;
                        }
                    }
                }
            }

            if (currentBook != null) books.Add(currentBook);
            return books;
        }
    }
}
