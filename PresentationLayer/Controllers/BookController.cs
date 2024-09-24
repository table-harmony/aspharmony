using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BusinessLogicLayer.Services;
using PresentationLayer.Models;
using Microsoft.AspNetCore.Authorization;
using DataAccessLayer.Entities;
using System.Security.Claims;
using System.Threading.Tasks;
using Utils.Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace PresentationLayer.Controllers
{
    [Authorize]
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
            var book = await _bookService.GetBookAsync(id);
            if (book == null)
                return NotFound();
            return View(book);
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookViewModel model) {
            if (!ModelState.IsValid)
                return View(model);

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _bookService.CreateAsync(model.Title, model.Description, model.Content, userId);
            
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id) {
            var book = await _bookService.GetBookAsync(id);
            if (book == null)
                return NotFound();

            var viewModel = new EditBookViewModel {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Content = book.Content
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditBookViewModel model) {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid) 
                return View(model);

            try {
                Book book = await _bookService.GetBookAsync(id);
                if (book == null)
                    return NotFound();

                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!User.IsInRole("Admin") && book.AuthorId != userId)
                    return Forbid();

                book.Title = model.Title;
                book.Description = model.Description;
                book.Content = model.Content;

                await _bookService.UpdateAsync(book);
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                ModelState.AddModelError("", "An error occurred while updating the book.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id) {
            var book = await _bookService.GetBookAsync(id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            Book book = await _bookService.GetBookAsync(id);
            if (book == null)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Admin") && book.AuthorId != userId)
                return Forbid();

            await _bookService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}