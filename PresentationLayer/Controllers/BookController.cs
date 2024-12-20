using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Services;
using PresentationLayer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Utils.Exceptions;
using Utils;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using DataAccessLayer.Entities;

using ServerBook = BusinessLogicLayer.Servers.Books.Book;
using Chapter = BusinessLogicLayer.Servers.Books.Chapter;
using Book = BusinessLogicLayer.Services.Book;
using Image = Utils.IImageModelService.Image;
using SpeechRequest = Utils.ITextToSpeechService.SpeechRequest;
using static Utils.ITranslationService;
using System.Text.Json.Serialization;

namespace PresentationLayer.Controllers {

    [Authorize]
    public class BookController(IBookService bookService,
                                    IEventTracker eventTracker,
                                    IFileUploader fileUploader,
                                    IImageModelService imageGenerator,
                                    ITextToSpeechService speechGenerator,
                                    IConfiguration configuration,
                                    ITextModelService textGenerator,
                                    ITranslationService translationService) : Controller {

        public async Task<IActionResult> Index(string searchString, int pageSize = 10, int pageIndex = 1) {
            var books = await bookService.GetAllAsync();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            books = books.Where(book => book.AuthorId == userId);

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

            ViewBag.Voices = await GetVoices(4);
            ViewBag.Languages = await GetLanguages();

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
                    imageUrl = await GenerateImage(prompt);
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
                ModelState.AddModelError("", message);

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
            ViewBag.Languages = await GetLanguages();

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
                    book.Metadata.ImageUrl = await GenerateImage(prompt);
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
                ModelState.AddModelError("", message);

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

        private async Task<string> GenerateImage(string prompt) {
            using var stream = await imageGenerator.GenerateImageAsync(new Image(prompt))
                ?? throw new PublicException("Failed to generate image.");

            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            string imageUrl = await fileUploader.UploadFileAsync(new IFileUploader.File(memoryStream));
            return imageUrl;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAudioBook(int id) {
            try {
                var audioBook = await bookService.GetAudioBookAsync(id);
                if (audioBook == null)
                    return NotFound(new { error = "Audio book not found" });

                var book = await bookService.GetBookAsync(audioBook.BookId);
                if (book == null)
                    return NotFound(new { error = "Book not found" });

                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                if (book.AuthorId != userId)
                    return Forbid();

                await bookService.DeleteAudioBookAsync(id);

                return Ok();
            } catch (Exception ex) {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateAudioBook(int id, string voiceId, string targetLanguage = "") {
            var book = await bookService.GetBookAsync(id);
            if (book == null)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (book.AuthorId != userId)
                return Forbid();

            try {
                string ssml = GenerateSSML(book);

                if (!string.IsNullOrEmpty(ssml)) {
                    ssml = await translationService.TranslateHtmlAsync(
                        new TranslationRequest(ssml, targetLanguage)
                    );
                }

                using var audioStream = await speechGenerator.GenerateSpeechAsync(
                    new SpeechRequest(Text: ssml, VoiceId: voiceId)
                );

                string audioUrl = await fileUploader.UploadFileAsync(new IFileUploader.File(audioStream, ContentType: "audio/mpeg"));

                await bookService.CreateAudioBookAsync(new AudioBook {
                    BookId = book.Id,
                    AudioUrl = audioUrl,
                });

                return Ok();
            } catch (Exception ex) {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPatch]
        public async Task<IActionResult> TranslateBook([FromBody] TranslateBookRequest request) {

            try {
                string translation = await translationService.TranslateHtmlAsync(
                    new TranslationRequest(JsonSerializer.Serialize(request), request.TargetLanguage)
                );

                var response = JsonSerializer.Deserialize<TranslateBookResponse>(CleanJsonResponse(translation));

                return Ok(response);
            } catch (Exception ex) {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPatch] 
        public async Task<IActionResult> PreviewTranslation([FromBody] PreviewTranslateRequest request) {
            try {
                string translation = await translationService.TranslateHtmlAsync(
                    new TranslationRequest(JsonSerializer.Serialize(request), request.TargetLanguage)
                );

                var response = JsonSerializer.Deserialize<dynamic>(CleanJsonResponse(translation));

                return Ok(response);
            } catch (Exception ex) {
                return BadRequest(new { error = ex.Message });
            }
        }

        private static string GenerateSSML(Book book) {
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

            string result = builder.ToString();

            return result;
        }

        private async Task<List<Voice>?> GetVoices(int number = 5) {
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
                    .Take(number)
                    .ToList();
            } catch {
                return [];
            }
        }

        private async Task<List<Language>> GetLanguages(string modelId = "eleven_multilingual_v2") {
            HttpClient client = new() {
                BaseAddress = new Uri("https://api.elevenlabs.io/")
            };
            client.DefaultRequestHeaders.Add("xi-api-key", configuration["ElevenLabs:ApiKey"]);

            try {
                var response = await client.GetAsync("v1/models");
                response.EnsureSuccessStatusCode();

                var data = await response.Content.ReadAsStringAsync();
                var models = JsonSerializer.Deserialize<List<Model>>(data);

                var model = (models?.Find(model => model.Id == modelId))
                    ?? throw new Exception($"Model {modelId} not found.");

                return [.. model.Languages.OrderBy(language => language.Name)];
            } catch {
                return [];
            }
        }

        public class Model {
            [JsonPropertyName("model_id")]
            public required string Id { get; set; }

            [JsonPropertyName("languages")]
            public required List<Language> Languages { get; set; } = [];
        }

        public class Language {
            [JsonPropertyName("language_id")]
            public required string Id { get; set; }

            [JsonPropertyName("name")]
            public required string Name { get; set; }
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateChapters([FromBody] GenerateChaptersRequest request) {
            try {
                string prompt = @$"You are an AI assistant helping to generate book chapters.
                    Given the following book details:
                    Title: {request.Title}
                    Description: {request.Description}
                    
                    Generate 3-5 chapters that would make sense for this book.
                    
                    IMPORTANT: Respond ONLY with a valid JSON array of chapter objects.
                    Each chapter must have these exact properties:
                    - title (string): The chapter title
                    - content (string): A 2-3 paragraph summary of the chapter
                    - index (integer): The chapter number starting from ${request.Chapters.Count + 1}";

                string response = await textGenerator.GetResponseAsync(prompt);
                string x = CleanJsonResponse(response);
                var chapters = JsonSerializer.Deserialize<List<ChapterViewModel>>(CleanJsonResponse(response))
                    ?? throw new Exception("Generated response was empty");

                return Json(chapters);
            } catch (JsonException) {
                return BadRequest(new {
                    error = "Failed to parse AI response. Please try again.",
                });
            } catch {
                return BadRequest(new {
                    error = "Failed to generate chapters",
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RefineBook([FromBody] RefineBookRequest request) {
            try {
                string prompt = @$"You are an expert book editor helping to refine and enhance a book's content.
                    Current Book:
                    Title: {request.Title}
                    Description: {request.Description}
                    Chapters: {JsonSerializer.Serialize(request.Chapters)}

                    Your task is to improve this book while maintaining its core story themes and language.

                    Guidelines:
                    1. Title: Make it more engaging and marketable while preserving the main concept
                    2. Description: A compelling 1-2 sentences which hook readers
                    3. Chapters: For each chapter:
                       - Enhance titles to be more intriguing
                       - Expand and improve content while maintaining the original plot points
                       - Ensure consistent tone and style throughout
                       - Maintain narrative flow between chapters
                       - Keep the same chapter count and index numbers

                    IMPORTANT: Respond ONLY with a valid JSON object in this exact format:
                    {{
                        ""title"": ""Enhanced and engaging title"",
                        ""description"": ""Compelling description that draws readers in..."",
                        ""chapters"": [
                            {{
                                ""title"": ""Refined chapter title"",
                                ""content"": ""Improved chapter content with better detail and flow"",
                                ""index"": ""Keep the Index of each chapter""
                            }}
                        ]
                    }}";

                string response = await textGenerator.GetResponseAsync(prompt);

                var refinedBook = JsonSerializer.Deserialize<RefineBookResponse>(CleanJsonResponse(response))
                    ?? throw new Exception("Generated response was empty");

                return Json(refinedBook);
            } catch (JsonException) {
                return BadRequest(new {
                    error = "Failed to parse AI response. Please try again.",
                });
            } catch {
                return BadRequest(new {
                    error = "Failed to refine book",
                });
            }
        }

        private static string CleanJsonResponse(string response) {
            if (string.IsNullOrWhiteSpace(response))
                return string.Empty;

            response = response.Replace("\\n", " ").Replace("\\t", " ").Replace("\\r", " ").Trim();
            response = Regex.Replace(response, @"\s{2,}", " ");

            int startIndex = response.IndexOfAny(['[', '{']);
            if (startIndex < 0)
                throw new Exception("Invalid JSON response: No valid start character found");

            int endIndex = response.LastIndexOfAny([']', '}']);
            if (endIndex < 0)
                throw new Exception("Invalid JSON response: No valid end character found");

            response = response.Substring(startIndex, endIndex - startIndex + 1);

            return response;
        }
        public class GenerateChaptersRequest {
            [JsonPropertyName("title")]
            public string Title { get; set; } = "";

            [JsonPropertyName("description")]
            public string Description { get; set; } = "";

            [JsonPropertyName("chapters")]
            public List<ChapterViewModel> Chapters { get; set; } = [];
        }


        public class PreviewTranslateRequest {
            [JsonPropertyName("target_language")]
            public string TargetLanguage { get; set; } = "";

            [JsonPropertyName("title")]
            public string Title { get; set; } = "";

            [JsonPropertyName("description")]
            public string Description { get; set; } = "";
        }

        public class TranslateBookRequest {
            [JsonPropertyName("target_language")]
            public string TargetLanguage { get; set; } = "";

            [JsonPropertyName("title")]
            public string Title { get; set; } = "";

            [JsonPropertyName("description")]
            public string Description { get; set; } = "";

            [JsonPropertyName("chapters")]
            public List<ChapterViewModel> Chapters { get; set; } = [];
        }

        public class TranslateBookResponse {
            [JsonPropertyName("title")]
            public string Title { get; set; } = "";

            [JsonPropertyName("description")]
            public string Description { get; set; } = "";

            [JsonPropertyName("chapters")]
            public List<ChapterViewModel> Chapters { get; set; } = [];
        }

        public class RefineBookRequest {
            [JsonPropertyName("title")]
            public string Title { get; set; } = "";

            [JsonPropertyName("description")]
            public string Description { get; set; } = "";

            [JsonPropertyName("chapters")]
            public List<ChapterViewModel> Chapters { get; set; } = [];
        }

        public class RefineBookResponse {
            [JsonPropertyName("title")]
            public string Title { get; set; } = "";

            [JsonPropertyName("description")]
            public string Description { get; set; } = "";

            [JsonPropertyName("chapters")]
            public List<ChapterViewModel> Chapters { get; set; } = [];
        }
    }
}