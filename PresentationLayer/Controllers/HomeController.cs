using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using JokesServiceReference;
using System.Xml.Linq;
using Utils;
using FileIO = System.IO.File;

namespace PresentationLayer.Controllers {
    public class HomeController(JokesServicePortTypeClient jokesService, IAiService aiService) : Controller {
        private readonly HttpClient _httpClient = new();

        public async Task<ActionResult> Index() {
            GenerateJokeRequest request = new();
            var response = await jokesService.GenerateJokeAsync(request);

            string joke = response.GenerateJokeResponse.joke;
            ViewBag.Joke = joke;

            return View();
        }

        [HttpGet]
        public IActionResult Calculator() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Calculate(int a, int b) {
            AddNumbersRequest request = new() {
                a = a,
                b = b
            };

            var response = await jokesService.AddNumbersAsync(request);
            int result = response.AddNumbersResponse.sum;

            ViewData["Result"] = result;
            return View("Calculator");
        }

        [HttpGet]
        public IActionResult Chat() {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MoreJokes(int count = 5) {
            var xmlContent = await _httpClient.GetStringAsync(
                $"https://aspharmony-production.up.railway.app/jokes.xml?count={count}");

            var xdoc = XDocument.Parse(xmlContent);
            var jokes = xdoc.Descendants("joke").Select(j => j.Value).ToList();

            return View(jokes);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            return View(new ErrorViewModel {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Exception = exceptionFeature?.Error
            });
        }

        [HttpPost]
        public async Task<IActionResult> GenerateContentAI(string question) {
            if (string.IsNullOrWhiteSpace(question)) {
                return BadRequest("Please enter a question");
            }

            string prompt = BuildPrompt(question);
            string answer = await aiService.GetResponseAsync(prompt);
            
            return Json(new {
                question,
                answer,
            });
        }

        private static string BuildPrompt(string question) {
            string filePath = PathManager.GetPath(FolderType.Site, "Context.txt");
            string context = FileIO.ReadAllText(filePath);

            return @$"You are an AI assistant for the AspHarmony website. 
              Website Context: {context}
              User Question: {question}
              
              Please answer the question based on the provided website context. 
              Be concise and specific. If the information isn't in the context, 
              say you don't have that information rather than making assumptions.";
        }
    }
}