using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BusinessLogicLayer.Services;
using PresentationLayer.Models;
using Microsoft.AspNetCore.Authorization;
using DataAccessLayer.Entities;

namespace PresentationLayer.Controllers
{
    [Authorize]
    public class BookController : Controller {
        private readonly IBookService _bookService;
        private readonly UserManager<User> _userManager;

        public BookController(IBookService bookService, UserManager<User> userManager) {
            _bookService = bookService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index() {
            var books = await _bookService.GetAllAsync();
            return View(books);
        }

        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookViewModel model) {
            if (!ModelState.IsValid) 
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            await _bookService.CreateAsync(model.Title, model.Description, model.Content, user.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}