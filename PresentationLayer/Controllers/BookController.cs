using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Services;
using PresentationLayer.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Utils.Services;
using DataAccessLayer.Entities;

using Chapter = BookServiceReference.Chapter;
using Book = BusinessLogicLayer.Services.Book;

namespace PresentationLayer.Controllers {

    [Authorize]
    public class BookController : Controller {
        private readonly IBookService _bookService;
        private readonly IFileUploader _fileUploader;
        private readonly UserManager<User> _userManager;

        public BookController(IBookService bookService, IFileUploader fileUploader, UserManager<User> userManager) {
            _bookService = bookService;
            _fileUploader = fileUploader;
            _userManager = userManager;
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
            if (ModelState.IsValid) {
                try {
                    string imageUrl = null;
                    if (model.Image != null) {
                        imageUrl = await _fileUploader.UploadFileAsync(model.Image);
                    }

                    var user = await _userManager.GetUserAsync(User);
                    var book = new Book {
                        Title = model.Title,
                        Description = model.Description,
                        AuthorId = user.Id,
                        ImageUrl = imageUrl
                    };

                    foreach (var chapterViewModel in model.Chapters) {
                        book.Chapters.Add(new Chapter {
                            Title = chapterViewModel.Title,
                            Content = chapterViewModel.Content,
                            Index = chapterViewModel.Index
                        });
                    }

                    await _bookService.CreateAsync(book);
                    return RedirectToAction(nameof(Index));
                } catch (Exception ex) {
                    ModelState.AddModelError("", "An error occurred while creating the book: " + ex.Message);
                }
            }
            return View(model);
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
                CurrentImageUrl = book.ImageUrl,
                Chapters = book.Chapters.Select(c => new ChapterViewModel {
                    Index = c.Index,
                    Title = c.Title,
                    Content = c.Content
                }).ToList()
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
                var book = await _bookService.GetBookAsync(id);
                if (book == null)
                    return NotFound();

                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!User.IsInRole("Admin") && book.AuthorId != userId)
                    return Forbid();

                book.Title = model.Title;
                book.Description = model.Description;

                if (model.NewImage != null) {
                    book.ImageUrl = await _fileUploader.UploadFileAsync(model.NewImage);
                }
                else {
                    book.ImageUrl = model.CurrentImageUrl;
                }

                book.Chapters = model.Chapters.Select(c => new Chapter {
                    Index = c.Index,
                    Title = c.Title,
                    Content = c.Content
                }).ToList();

                await _bookService.UpdateAsync(book);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) {
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
            var book = await _bookService.GetBookAsync(id);
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