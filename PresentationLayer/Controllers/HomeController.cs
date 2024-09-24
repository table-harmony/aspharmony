using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PresentationLayer.Models;
using System.Diagnostics;
using Utils.Exceptions;
using Utils.Services;

namespace PresentationLayer.Controllers
{
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly IEventsService _eventsService;

        public HomeController(ILogger<HomeController> logger, IEventsService eventsService) {
            _logger = logger;
            _eventsService = eventsService;
        }

        public async Task<ActionResult> Index() {
            await _eventsService.TrackEventAsync("User hit landing page");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}