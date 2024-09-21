using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BusinessLogicLayer.Services;
using PresentationLayer.Models;
using DataAccessLayer.Entities;

namespace PresentationLayer.Controllers
{
    public class LibraryController : Controller {
        private readonly ILibraryService _libraryService;
        private readonly UserManager<User> _userManager;

        public LibraryController(ILibraryService libraryService, UserManager<User> userManager) {
            _libraryService = libraryService;
            _userManager = userManager;
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

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();
           
            await _libraryService.CreateAsync(model.Name, user.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}