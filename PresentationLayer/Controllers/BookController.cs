using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BusinessLogicLayer.Services;
using PresentationLayer.Models;
using Microsoft.AspNetCore.Authorization;
using DataAccessLayer.Entities;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    public class BookController : Controller {
        private readonly IBookService _bookService;
        private readonly IUserService _userService;

        public BookController(IBookService bookService, IUserService userService) {
            _bookService = bookService;
            _userService = userService;
        }

        public async Task<IActionResult> Index() {
            var books = await _bookService.GetAllAsync();
            return View(books);
        }

        public async Task<IActionResult> Details(int id) {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null) {
                return NotFound();
            }
            return View(book);
        }

        [Authorize]
        public IActionResult Create() {
            return View(new BookViewModel());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel model) {
            if (ModelState.IsValid) {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _bookService.CreateAsync(model.Title, model.Description, model.Content, userId);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id) {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null) {
                return NotFound();
            }

            var viewModel = new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Content = book.Content,
                AuthorName = book.Author?.UserName ?? "Unknown"
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookViewModel model) {
            if (id != model.Id) {
                return NotFound();
            }

            var book = await _bookService.GetByIdAsync(id);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!User.IsInRole("Admin") && book.AuthorId != userId) {
                return Forbid();
            }

            if (ModelState.IsValid) {
                book.Title = model.Title;
                book.Description = model.Description;
                book.Content = model.Content;

                await _bookService.UpdateAsync(book);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id) {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null) {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!User.IsInRole("Admin") && book.AuthorId != userId) {
                return Forbid();
            }

            return View(book);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null) {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!User.IsInRole("Admin") && book.AuthorId != userId) {
                return Forbid();
            }

            await _bookService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}