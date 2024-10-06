using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using AspHarmonyServiceReference;
using System.Xml.Linq;
using System.Net.Http;

namespace PresentationLayer.Controllers
{
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly AspHarmonyPortTypeClient _soapClient;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, AspHarmonyPortTypeClient soapClient) {
            _logger = logger;
            _soapClient = soapClient;
            _httpClient = new HttpClient();
        }

        public async Task<ActionResult> Index() {
            GenerateJokeRequest request = new();
            var response = await _soapClient.GenerateJokeAsync(request);

            string joke = response.GenerateJokeResponse.joke;
            ViewBag.Joke = joke;

            // Add the URL for the More Jokes page
            ViewBag.MoreJokesUrl = "https://aspharmony-production.up.railway.app/more-jokes?count=20";

            return View();
        }

        [HttpGet]
        public IActionResult Calculator() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Calculate(int a, int b) {
            AddNumbersRequest request = new() { a = a, b = b };
            var response = await _soapClient.AddNumbersAsync(request);

            int result = response.AddNumbersResponse.result;
            
            ViewData["Result"] = result;
            return View("Calculator");
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