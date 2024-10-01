using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;

namespace PresentationLayer.Controllers
{
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        public async Task<ActionResult> Index() {
            //AddNumbersRequest request = new() {
            //    a = 1,
            //    b = 2,
            //};

            //var response = await _soapClient.AddNumbersAsync(request);
            //throw new Exception(response.AddNumbersResponse.AddResult.ToString());
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            
            return View(new ErrorViewModel { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Exception = exceptionFeature?.Error
            });
        }

        public IActionResult AccessDenied() {
            return View();
        }
    }
}