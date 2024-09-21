using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Services;
using PresentationLayer.Models;
using Microsoft.AspNetCore.Authorization;

namespace PresentationLayer.Controllers
{
    public class BookController : Controller {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService) {
            _bookService = bookService;
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

            int authorId = 3;

            await _bookService.CreateAsync(model.Title, model.Description, model.Content, authorId);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}