using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using JokesServiceReference;
using System.Xml.Linq;
using System.Net.WebSockets;
using System.Text;

namespace PresentationLayer.Controllers {
    public class HomeController(JokesServicePortTypeClient jokesService) : Controller {
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
    }
}