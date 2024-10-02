using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using AspHarmonyServiceReference;

namespace PresentationLayer.Controllers
{
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly AspHarmonyPortTypeClient _soapClient;

        public HomeController(ILogger<HomeController> logger, AspHarmonyPortTypeClient soapClient) {
            _logger = logger;
            _soapClient = soapClient;
        }

        public async Task<ActionResult> Index() {
            GenerateJokeRequest request = new();
            var response = await _soapClient.GenerateJokeAsync(request);

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
            AddNumbersRequest request = new() { a = a, b = b };
            var response = await _soapClient.AddNumbersAsync(request);

            int result = response.AddNumbersResponse.result;

            ViewData["Result"] = result;
            return View("Calculator");
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