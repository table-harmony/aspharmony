using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using System.Security.Claims;
using PresentationLayer.Models;
using Utils.Exceptions;
using BusinessLogicLayer.Events;

namespace PresentationLayer.Controllers
{
    [Authorize]
    public class LibraryController : Controller
    {
        private readonly ILibraryService _libraryService;
        private readonly ILibraryMembershipService _libraryMembershipService;
        private readonly IBookService _bookService;
        private readonly IUserService _userService;
        private readonly IBookLoanService _bookLoanService;

        public LibraryController(ILibraryService libraryService, 
                                    IBookService bookService, 
                                    IUserService userService,
                                    ILibraryMembershipService libraryMembershipService,
                                    IBookLoanService bookLoanService) {
            _libraryService = libraryService;
            _bookService = bookService;
            _userService = userService;
            _libraryMembershipService = libraryMembershipService;
            _bookLoanService = bookLoanService;
        }

        public async Task<IActionResult> Index() {
            var libraries = await _libraryService.GetAllAsync();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.UserId = userId;
            return View(libraries);
        }

        public async Task<IActionResult> Details(int id) {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isMember = await _libraryMembershipService.IsMemberAsync(id, userId);
            if (!isMember)
                return Forbid();
            

            try {
                var library = await _libraryService.GetByIdAsync(id);
                if (library == null) {
                    return NotFound();
                }

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
            catch (PublicException ex) {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex) {
                string error = "Something went wrong";
                if (ex is PublicException)
                    error = ex.Message;
                ModelState.AddModelError("", error);
            }

            return View();
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
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _libraryService.UpdateAsync(id, model.Name);
                    return RedirectToAction(nameof(Details), new { id = model.Id });
                }
                catch (Exception)
                {
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
            try {
                var library = await _libraryService.GetByIdAsync(libraryId);
                if (library == null) {
                    return NotFound();
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!User.IsInRole("Admin") && !library.Memberships.Any(m => m.UserId == userId && m.Role == MembershipRole.Manager))
                {
                    return Forbid();
                }

                await _libraryService.AddBookToLibraryAsync(libraryId, bookId);
                var book = await _bookService.GetByIdAsync(bookId);
                UserEvents.OnBookAddedToLibrary(book.AuthorId, book.Title, libraryId, library.Name);

                return RedirectToAction(nameof(Details), new { id = libraryId });
            }
            catch (PublicException ex) {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex) {
                string error = "Something went wrong";
                if (ex is PublicException)
                    error = ex.Message;
                ModelState.AddModelError("", error);
            }
            return RedirectToAction(nameof(Details), new { id = libraryId });
        }

        public async Task<IActionResult> BookDetails(int libraryBookId)
        {
            var libraryBook = await _libraryService.GetLibraryBookByIdAsync(libraryBookId);
            if (libraryBook == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isMember = await _libraryMembershipService.IsMemberAsync(libraryBook.LibraryId, userId);
            if (!isMember)
            {
                return Forbid();
            }

            // Fetch the book with author information
            var book = await _bookService.GetByIdAsync(libraryBook.BookId);
            if (book == null)
            {
                return NotFound();
            }

            var pastLoans = await _bookLoanService.GetPastLoansByLibraryBookIdAsync(libraryBookId);
            var currentLoan = await _bookLoanService.GetCurrentLoanByLibraryBookIdAsync(libraryBookId);

            var viewModel = new BookDetailsViewModel {
                LibraryBook = libraryBook,
                Book = book,
                PastLoans = pastLoans,
                CurrentLoan = currentLoan,
                CanBeBorrowed = currentLoan == null
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoanBook(int libraryBookId, int libraryId, DateTime dueDate) {
            if (dueDate.Date < DateTime.Now.Date) {
                ModelState.AddModelError("", "Due date must be today or a future date.");
                return RedirectToAction(nameof(BookDetails), new { libraryBookId });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var libraryBook = await _libraryService.GetLibraryBookByIdAsync(libraryBookId);
            
            if (libraryBook == null)
                return NotFound();
            

            var isMember = await _libraryMembershipService.IsMemberAsync(libraryId, userId);
            if (!isMember)
                return Forbid();
            

            var currentLoan = await _bookLoanService.GetCurrentLoanByLibraryBookIdAsync(libraryBookId);
            if (currentLoan != null) {
                ModelState.AddModelError("", "This book is already on loan.");
                return RedirectToAction(nameof(BookDetails), new { libraryBookId = libraryBookId });
            }

            await _bookLoanService.CreateLoanAsync(libraryBookId, userId, libraryId, dueDate);
            return RedirectToAction(nameof(BookDetails), new { libraryBookId = libraryBookId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JoinLibrary(int libraryId) {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            var isMember = await _libraryMembershipService.IsMemberAsync(libraryId, userId);
            if (isMember) {
                return RedirectToAction(nameof(Details), new { id = libraryId });
            }

            var library = await _libraryService.GetByIdAsync(libraryId);
            if (library == null) {
                return NotFound();
            }

            await _libraryService.AddMemberAsync(libraryId, userId);
            UserEvents.OnUserJoinedLibrary(userId, libraryId, library.Name);
            return RedirectToAction(nameof(Details), new { id = libraryId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMember(int libraryId, string userId) {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isManager = await _libraryMembershipService.IsManagerAsync(libraryId, currentUserId);
            
            if (!isManager && currentUserId != userId) {
                return Forbid();
            }

            await _libraryService.RemoveMemberAsync(libraryId, userId);
            return RedirectToAction(nameof(Details), new { id = libraryId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnBook(int bookLoanId) {
            var bookLoan = await _bookLoanService.GetByIdAsync(bookLoanId);
            if (bookLoan == null) {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (bookLoan.UserId != userId) {
                return Forbid();
            }

            await _bookLoanService.ReturnBookAsync(bookLoanId);
            return RedirectToAction(nameof(BookDetails), new { libraryBookId = bookLoan.LibraryBookId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveBook(int libraryId, int bookId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _libraryService.RemoveBookFromLibraryAsync(libraryId, bookId, userId);
                return RedirectToAction(nameof(Details), new { id = libraryId });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (PublicException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went wrong. Please try again later.");
            }
            return RedirectToAction(nameof(Details), new { id = libraryId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLibrary(int libraryId) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try {
                await _libraryService.DeleteLibraryAsync(libraryId, userId);
                return RedirectToAction(nameof(Index));
            }
            catch (UnauthorizedAccessException) {
                return Forbid();
            }
            catch (PublicException ex) {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception) {
                ModelState.AddModelError("", "Something went wrong. Please try again later.");
            }
            return RedirectToAction(nameof(Details), new { id = libraryId });
        }
    }
}