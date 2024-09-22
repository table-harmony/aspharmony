using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using System.Security.Claims;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers
{
    [Authorize]
    public class LibraryController : Controller {
        private readonly ILibraryService _libraryService;
        private readonly ILibraryMembershipService _libraryMembershipService;
        private readonly IBookService _bookService;
        private readonly IUserService _userService;

        public LibraryController(ILibraryService libraryService, 
                                    IBookService bookService, 
                                    IUserService userService,
                                    ILibraryMembershipService libraryMembershipService) {
            _libraryService = libraryService;
            _bookService = bookService;
            _userService = userService;
            _libraryMembershipService = libraryMembershipService;
        }

        public async Task<IActionResult> Index() {
            var libraries = await _libraryService.GetAllAsync();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.UserId = userId;
            return View(libraries);
        }

        public async Task<IActionResult> Details(int id) {
            var library = await _libraryService.GetByIdAsync(id);
            if (library == null) {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            ViewBag.IsManager = library.Memberships != null && 
                library.Memberships.Any(m => m.UserId == userId && m.Role == MembershipRole.Manager);
            ViewBag.IsMember = library.Memberships != null &&
                library.Memberships.Any(m => m.UserId == userId);

            var availableBooks = await _bookService.GetAllAsync();
            ViewBag.AvailableBooks = availableBooks.Where(b => !library.Books.Any(lb => lb.BookId == b.Id));

            if (ViewBag.IsManager) {
                ViewBag.Members = await _libraryMembershipService.GetMembersByLibraryIdAsync(id);
            }

            return View(library);
        }

        [Authorize]
        public IActionResult Create() {
            return View(new LibraryViewModel());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LibraryViewModel model) {
            if (ModelState.IsValid) {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _libraryService.CreateAsync(model.Name, userId);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id) {
            var library = await _libraryService.GetByIdAsync(id);
            if (library == null) {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!User.IsInRole("Admin") && !library.Memberships.Any(m => m.UserId == userId && m.Role == MembershipRole.Manager)) {
                return Forbid();
            }

            var viewModel = new LibraryViewModel
            {
                Id = library.Id,
                Name = library.Name
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LibraryViewModel model) {
            if (id != model.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    await _libraryService.UpdateAsync(id, model.Name);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception) {
                    // Handle the exception appropriately
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var library = await _libraryService.GetByIdAsync(id);
            if (library == null) {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!User.IsInRole("Admin") && !library.Memberships.Any(m => m.UserId == userId && m.Role == MembershipRole.Manager)) {
                return Forbid();
            }

            return View(library);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var library = await _libraryService.GetByIdAsync(id);
            if (library == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!User.IsInRole("Admin") && !library.Memberships.Any(m => m.UserId == userId && m.Role == MembershipRole.Manager)) {
                return Forbid();
            }

            await _libraryService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBook(int libraryId, int bookId) {
            var library = await _libraryService.GetByIdAsync(libraryId);
            if (library == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!User.IsInRole("Admin") && !library.Memberships.Any(m => m.UserId == userId && m.Role == MembershipRole.Manager))
            {
                return Forbid();
            }

            await _libraryService.AddBookToLibraryAsync(libraryId, bookId);

            return RedirectToAction(nameof(Details), new { id = libraryId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JoinLibrary(int libraryId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userService.GetByIdAsync(userId);
            var library = await _libraryService.GetByIdAsync(libraryId);

            if (library == null || user == null)
            {
                return NotFound();
            }

            await _libraryMembershipService.CreateAsync(user, library, MembershipRole.Member);
            return RedirectToAction(nameof(Details), new { id = libraryId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMember(int libraryId, string userId)
        {
            var library = await _libraryService.GetByIdAsync(libraryId);
            if (library == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!User.IsInRole("Admin") && !library.Memberships.Any(m => m.UserId == currentUserId && m.Role == MembershipRole.Manager))
            {
                return Forbid();
            }

            await _libraryMembershipService.DeleteAsync(libraryId, userId);
            return RedirectToAction(nameof(Details), new { id = libraryId });
        }
    }
}