using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PresentationLayer.Models;
using System.Diagnostics;

namespace PresentationLayer.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly ApiService _apiService;
        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
            _apiService = new ApiService();
        }

        public async Task<ActionResult> Index() {
            await _apiService.TrackEventAsync("User hit landing page");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}