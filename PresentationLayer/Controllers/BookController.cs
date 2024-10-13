using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Services;
using PresentationLayer.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using DataAccessLayer.Entities;
using Utils.Exceptions;
using Utils;

using ServerBook = BusinessLogicLayer.Servers.Books.Book;
using Chapter = BusinessLogicLayer.Servers.Books.Chapter;
using Book = BusinessLogicLayer.Services.Book;

namespace PresentationLayer.Controllers {

    [Authorize]
    public class BookController(IBookService bookService, IFileUploader fileUploader, 
                                    UserManager<User> userManager) : Controller {

        public async Task<IActionResult> Index(string searchString) {
            var books = await bookService.GetAllAsync();

            if (!string.IsNullOrEmpty(searchString)) {
                books = books.Where(book => 
                    book.Metadata.Title.ToLower().Contains(searchString.ToLower()) ||                 
                    book.Metadata.Description.ToLower().Contains(searchString.ToLower()));
            }

            return View(books);
        }
        public async Task<IActionResult> Details(int id) {
            var book = await bookService.GetBookAsync(id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookViewModel model) {
            if (!ModelState.IsValid)
                return View(model);

            try {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                
                string imageUrl = "https://birkhauser.com/product-not-found.png";
                if (model.Image != null)
                    imageUrl = await fileUploader.UploadFileAsync(model.Image);

                Book book = new() {
                    AuthorId = userId,
                    Metadata = new ServerBook {
                        Title = model.Title,
                        Description = model.Description,
                        ImageUrl = imageUrl,
                        Chapters = model.Chapters.Select(c => new Chapter {
                            Index = c.Index,
                            Title = c.Title,
                            Content = c.Content
                        }).ToList()
                    }
                };
                await bookService.CreateAsync(book);

                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                ModelState.AddModelError("", "An error occurred while creating the book: " + ex.Message);
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id) {
            var book = await bookService.GetBookAsync(id);
            if (book == null)
                return NotFound();

            EditBookViewModel model = new() {
                Id = book.Id,
                Title = book.Metadata?.Title ?? "Unknown",
                Description = book.Metadata?.Description ?? "Unknown",
                CurrentImageUrl = book.Metadata?.ImageUrl ?? "Unknown",
                Chapters = book.Metadata?.Chapters.Select(c => new ChapterViewModel {
                    Index = c.Index,
                    Title = c.Title,
                    Content = c.Content
                }).ToList() ?? []
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditBookViewModel model) {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid) 
                return View(model);

            try {
                var book = await bookService.GetBookAsync(id);
                if (book == null)
                    return NotFound();

                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                if (!User.IsInRole("Admin") && book.AuthorId != userId)
                    return Forbid();

                if (book.Metadata == null)
                    throw new PublicException("Metadata not found");

                book.Metadata.Title = model.Title;
                book.Metadata.Description = model.Description;

                if (model.NewImage != null) {
                    book.Metadata.ImageUrl = await fileUploader.UploadFileAsync(model.NewImage);
                }

                book.Metadata.Chapters = model.Chapters.Select(c => new Chapter {
                    Index = c.Index,
                    Title = c.Title,
                    Content = c.Content
                }).ToList();

                await bookService.UpdateAsync(book);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) {
                ModelState.AddModelError("", "An error occurred while updating the book.");
                return View(model);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id) {
            var book = await bookService.GetBookAsync(id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var book = await bookService.GetBookAsync(id);
            if (book == null)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (!User.IsInRole("Admin") && book.AuthorId != userId)
                return Forbid();

            await bookService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
