using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Services;
using PresentationLayer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DataAccessLayer.Entities;
using Utils.Exceptions;
using Utils;
using System.Text;

using ServerBook = BusinessLogicLayer.Servers.Books.Book;
using Chapter = BusinessLogicLayer.Servers.Books.Chapter;
using Book = BusinessLogicLayer.Services.Book;
using Image = Utils.IImageModelService.Image;
using SpeechRequest = Utils.ITextToSpeechService.SpeechRequest;
using System.Net.Http;
using System.Text.Json.Serialization;

namespace PresentationLayer.Controllers {

    [Authorize]
    public class BookController(IBookService bookService,
                                    IEventTracker eventTracker,
                                    IFileUploader fileUploader,
                                    IImageModelService imageGenerator,
                                    ITextToSpeechService speechGenerator,
                                    IConfiguration configuration) : Controller {
        public async Task<IActionResult> Index(string searchString, int pageSize = 10, int pageIndex = 1) {
            var books = await bookService.GetAllAsync();

            if (!string.IsNullOrEmpty(searchString)) {
                books = books.Where(b =>
                    b.Metadata.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    (b.Author?.UserName?.Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? false));
            }

            var paginatedBooks = PaginatedList<Book>.Create(
                books.AsQueryable(),
                pageIndex,
                pageSize
            );

            return View(new BookIndexViewModel {
                Books = paginatedBooks,
                SearchString = searchString,
                PageSize = pageSize
            });
        }

        [HttpGet]
        public IActionResult Create() {
            GetServers();

            return View(new CreateBookViewModel { Server = ServerType.Nimbus1 });
        }

        public async Task<IActionResult> Details(int id) {
            var book = await bookService.GetBookAsync(id);
            if (book == null)
                return NotFound();

            ViewBag.Voices = await GetVoices();
            return View(book);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookViewModel model) {
            if (!ModelState.IsValid) {
                GetServers();
                return View(model);
            }

            try {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                string imageUrl = "https://via.placeholder.com/400x600?text=No+Cover";

                if (model.Image != null) {
                    imageUrl = await fileUploader.UploadFileAsync(model.Image);
                } else if (model.GenerateImage) {
                    string prompt = GenerateImagePrompt(model.Title, model.Description);
                    using var imageStream = await imageGenerator.GenerateImageAsync(new Image(prompt));

                    imageUrl = await fileUploader.UploadFileAsync(imageStream);
                }

                Book book = new() {
                    AuthorId = userId,
                    Server = model.Server,
                    Metadata = new ServerBook {
                        Title = model.Title,
                        Description = model.Description,
                        ImageUrl = imageUrl,
                        Chapters = model.Chapters.Select(c => new Chapter {
                            Index = c.Index,
                            Title = c.Title,
                            Content = c.Content
                        }).ToList()
                    }
                };

                await bookService.CreateAsync(book);
                await eventTracker.TrackEventAsync("Book created");

                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                GetServers(model.Server);

                string message = ex is PublicException ? ex.Message : "An error occurred while creating the book.";
                ModelState.AddModelError("", ex.Message);

                return View(model);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id) {
            var book = await bookService.GetBookAsync(id);
            if (book == null)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (book.AuthorId != userId)
                return RedirectToAction(nameof(Details), new { id });

            GetServers(book.Server);

            EditBookViewModel model = new() {
                Id = book.Id,
                Server = book.Server,
                Title = book.Metadata?.Title ?? "Unknown",
                Description = book.Metadata?.Description ?? "Unknown",
                Chapters = book.Metadata?.Chapters?.Select(c => new ChapterViewModel {
                    Index = c.Index,
                    Title = c.Title,
                    Content = c.Content
                }).ToList() ?? []
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditBookViewModel model) {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid) {
                GetServers(model.Server);
                return View(model);
            }

            var book = await bookService.GetBookAsync(id);

            if (book == null)
                return NotFound();

            try {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                if (!User.IsInRole("Admin") && book.AuthorId != userId)
                    return Forbid();

                book.Server = model.Server;

                book.Metadata ??= new ServerBook() { Id = book.Id };

                book.Metadata.Title = model.Title;
                book.Metadata.Description = model.Description;

                if (model.NewImage != null) {
                    book.Metadata.ImageUrl = await fileUploader.UploadFileAsync(model.NewImage);
                } else if (model.GenerateImage) {
                    string prompt = GenerateImagePrompt(model.Title, model.Description);
                    using var stream = await imageGenerator.GenerateImageAsync(new Image(prompt));

                    book.Metadata.ImageUrl = await fileUploader.UploadFileAsync(stream);
                }

                book.Metadata.Chapters = model.Chapters.Select(c => new Chapter {
                    Index = c.Index,
                    Title = c.Title,
                    Content = c.Content
                }).ToList();

                await bookService.UpdateAsync(book);
                await eventTracker.TrackEventAsync("Book edited");

                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                GetServers(book.Server);

                string message = ex is PublicException ? ex.Message : "An error occurred while updating the book.";
                ModelState.AddModelError("", ex.Message);

                return View(model);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id) {
            var book = await bookService.GetBookAsync(id);
            if (book == null)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (!User.IsInRole("Admin") && book.AuthorId != userId)
                return RedirectToAction(nameof(Details), new { id });

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var book = await bookService.GetBookAsync(id);
            if (book == null)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (!User.IsInRole("Admin") && book.AuthorId != userId)
                return Forbid();

            await bookService.DeleteAsync(id);
            await eventTracker.TrackEventAsync("Book deleted");

            return RedirectToAction(nameof(Index));
        }

        private void GetServers(ServerType selectedServer = ServerType.Nimbus1) {
            ViewBag.Servers = Enum.GetValues(typeof(ServerType))
                .Cast<ServerType>()
                .Select(e => new SelectListItem {
                    Value = ((int)e).ToString(),
                    Text = e.GetDisplayName(),
                    Selected = e == selectedServer
                })
                .ToList();
        }

        private static string GenerateImagePrompt(string title, string description) {
            if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(description)) {
                return "Generate a generic book cover image";
            }
            return $"Book cover for '{title}'. {description}";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateAudioBook(int id, string voiceId) {
            var book = await bookService.GetBookAsync(id);
            if (book == null)
                return NotFound();

            try {
                string fullText = GenerateAudiobookPrompt(book);

                var request = new SpeechRequest(
                    Text: fullText,
                    VoiceId: voiceId
                );
                
                var audioStream = await speechGenerator.GenerateSpeechAsync(request);
                
                using var memoryStream = new MemoryStream();
                await audioStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                
                return File(memoryStream.ToArray(), "audio/mpeg", $"{book.Metadata.Title} audiobook.mp3");
            }
            catch {
                return StatusCode(500, new { 
                    error = "Failed to generate audio book. Please try again later." 
                });
            }
        }

        private static string GenerateAudiobookPrompt(Book book) {
            StringBuilder builder = new();

            builder.AppendLine($"<speak>");
            builder.AppendLine($"<prosody pitch=\"+10%\" rate=\"medium\" volume=\"loud\">Welcome to the audiobook of <emphasis level=\"strong\">\"{book.Metadata.Title}\"</emphasis>.</prosody>");

            if (!string.IsNullOrWhiteSpace(book.Metadata.Description)) {
                builder.AppendLine($"<break time=\"1s\" />");
                builder.AppendLine($"<prosody pitch=\"-5%\" rate=\"slow\" volume=\"medium\">{book.Metadata.Description}</prosody>");
            }

            foreach (var chapter in book.Metadata.Chapters.OrderBy(c => c.Index)) {
                builder.AppendLine($"<break time=\"1s\" />");
                builder.AppendLine($"<prosody pitch=\"+5%\" rate=\"medium\" volume=\"loud\">Chapter {chapter.Index + 1}: {chapter.Title}</prosody>");
                builder.AppendLine($"<break time=\"0.5s\" />");
                builder.AppendLine($"<prosody pitch=\"0%\" rate=\"medium\" volume=\"default\">{chapter.Content}</prosody>");
                builder.AppendLine($"<break time=\"1s\" />");
            }

            builder.AppendLine($"<prosody pitch=\"-5%\" rate=\"medium\" volume=\"soft\">Thank you for listening to this audiobook.</prosody>");
            builder.AppendLine($"<prosody pitch=\"-10%\" rate=\"slow\" volume=\"soft\">We hope you enjoyed it.</prosody>");
            builder.AppendLine($"</speak>");

            return builder.ToString();
        }

        private async Task<List<Voice>?> GetVoices() {
            HttpClient client = new() {
                BaseAddress = new Uri("https://api.elevenlabs.io/")
            };
            client.DefaultRequestHeaders.Add("xi-api-key", configuration["ElevenLabs:ApiKey"]);

            try {
                var response = await client.GetAsync("v1/voices");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<VoicesResponse>();

                return result?.Voices?
                    .OrderBy(_ => Guid.NewGuid())
                    .Take(5)
                    .ToList();
            } catch {
                return [
                    new Voice {
                        VoiceId = "9BWtsMINqrJLrRacOk9x",
                        Name = "Aria",
                        PreviewUrl = "https://storage.googleapis.com/eleven-public-prod/premade/voices/9BWtsMINqrJLrRacOk9x/405766b8-1f4e-4d3c-aba1-6f25333823ec.mp3",
                        Labels = new Dictionary<string, string> {
                            { "Accent", "American" },
                            { "Description", "Expressive" },
                            { "Age", "Middle-aged" },
                            { "Gender", "Female" },
                            { "Use Case", "Social Media" }
                        }
                    }
                ];
            }
        }

        public class VoicesResponse {
            public List<Voice>? Voices { get; set; }
        }

        public class Voice {

            [JsonPropertyName("voice_id")]
            public required string VoiceId { get; set; }

            [JsonPropertyName("name")]
            public required string Name { get; set; }

            [JsonPropertyName("preview_url")]
            public string? PreviewUrl { get; set; }

            [JsonPropertyName("labels")]
            public Dictionary<string, string>? Labels { get; set; }
        }
    }
}