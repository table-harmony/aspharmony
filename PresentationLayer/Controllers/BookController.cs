using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Services;
using PresentationLayer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DataAccessLayer.Entities;
using Utils.Exceptions;
using Utils;

using ServerBook = BusinessLogicLayer.Servers.Books.Book;
using Chapter = BusinessLogicLayer.Servers.Books.Chapter;
using Book = BusinessLogicLayer.Services.Book;

namespace PresentationLayer.Controllers
{

    [Authorize]
    public class BookController(IBookService bookService,
                                    IEventTracker eventTracker,
                                    IFileUploader fileUploader) : Controller {

        public async Task<IActionResult> Index(string searchString = "") {
            var books = await bookService.GetAllAsync();

            if (!string.IsNullOrEmpty(searchString)) {
                books = books.Where(book =>
                    book.Metadata?.Title.Contains(searchString, StringComparison.CurrentCultureIgnoreCase) == true ||
                    book.Metadata?.Description.Contains(searchString, StringComparison.CurrentCultureIgnoreCase) == true);
            }

            BookIndexViewModel viewModel = new() {
                Books = books,
                SearchString = searchString
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create() {
            GetServers();

            return View(new CreateBookViewModel { Server = ServerType.Nimbus });
        }

        public async Task<IActionResult> Details(int id) {
            var book = await bookService.GetBookAsync(id);
            if (book == null)
                return NotFound();

            return View(book);
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
                    Server = model.Server,
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
                await eventTracker.TrackEventAsync("Book created");
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                GetServers(model.Server);

                string message = ex is PublicException ? ex.Message : "An error occurred while creating the book.";
                ModelState.AddModelError("", message);

                return View(model);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id) {
            var book = await bookService.GetBookAsync(id);
            if (book == null)
                return NotFound();

            GetServers(book.Server);

            EditBookViewModel model = new() {
                Id = book.Id,
                Server = book.Server,
                Title = book.Metadata?.Title ?? "Unknown",
                Description = book.Metadata?.Description ?? "Unknown",
                CurrentImageUrl = book.Metadata?.ImageUrl ?? "https://birkhauser.com/product-not-found.png",
                Chapters = book.Metadata?.Chapters?.Select(c => new ChapterViewModel {
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

            var book = await bookService.GetBookAsync(id);

            if (book == null)
                return NotFound();

            try {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                if (!User.IsInRole("Admin") && book.AuthorId != userId)
                    return Forbid();

                book.Server = model.Server;

                book.Metadata ??= new ServerBook() { Id = book.Id };

                book.Metadata.Title = model.Title;
                book.Metadata.Description = model.Description;
                book.Metadata.ImageUrl = model.CurrentImageUrl;

                if (model.NewImage != null) {
                    book.Metadata.ImageUrl = await fileUploader.UploadFileAsync(model.NewImage);
                }
                
                book.Metadata.Chapters = model.Chapters.Select(c => new Chapter {
                    Index = c.Index,
                    Title = c.Title,
                    Content = c.Content
                }).ToList();

                await bookService.UpdateAsync(book);
                await eventTracker.TrackEventAsync("Book edited");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) {
                GetServers(book.Server);

                string message = ex is PublicException ? ex.Message : "An error occurred while updating the book.";
                ModelState.AddModelError("", message);

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
            await eventTracker.TrackEventAsync("Book deleted");

            return RedirectToAction(nameof(Index));
        }

        private void GetServers(ServerType selectedServer = ServerType.Nimbus) {
            ViewBag.Servers = Enum.GetValues(typeof(ServerType))
                .Cast<ServerType>()
                .Select(e => new SelectListItem {
                    Value = ((int)e).ToString(),
                    Text = e.ToString(),
                    Selected = e == selectedServer
                })
                .ToList();
        }
    }
}
