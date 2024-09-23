using Microsoft.AspNetCore.Mvc;
using Utils.Exceptions;

namespace PresentationLayer.Controllers
{
    public class BaseController : Controller
    {
        protected IActionResult HandleException(Exception ex)
        {
            if (ex is PublicException publicEx) {
                TempData["ErrorMessage"] = publicEx.Message;
            }
            else {
                TempData["ErrorMessage"] = "Something went wrong. Please try again later.";
            }

            return RedirectToAction("Index", "Home");
        }
    }
}