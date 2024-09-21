using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Services;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers
{
    public class LibraryController : Controller {
        private readonly ILibraryService _libraryService;

        public LibraryController(ILibraryService libraryService) {
            _libraryService = libraryService;
        }

        public async Task<IActionResult> Index() {
            var libraries = await _libraryService.GetAllAsync();
            return View(libraries);
        }

        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLibraryViewModel model) {

            if (!ModelState.IsValid)
                return View(model);

            int userId = 3; //TODO: get current user id
            
            await _libraryService.CreateAsync(model.Name, userId);
            return RedirectToAction(nameof(Index));
        }
    }
}