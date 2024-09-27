using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using System.Security.Claims;
using PresentationLayer.Models;
using Utils.Exceptions;

namespace PresentationLayer.Controllers
{
    [Authorize]
    public class LibraryController : Controller
    {
        private readonly ILibraryService _libraryService;
        private readonly ILibraryMembershipService _libraryMembershipService;
        private readonly ILibraryBookService _libraryBookService;
        private readonly IBookService _bookService;
        private readonly IBookLoanService _bookLoanService;

        public LibraryController(
            ILibraryService libraryService,
            ILibraryMembershipService libraryMembershipService,
            ILibraryBookService libraryBookService,
            IBookService bookService,
            IBookLoanService bookLoanService) {
            _libraryService = libraryService;
            _libraryMembershipService = libraryMembershipService;
            _libraryBookService = libraryBookService;
            _bookService = bookService;
            _bookLoanService = bookLoanService;
        }

        public async Task<IActionResult> Index() {
            var libraries = await _libraryService.GetAllAsync();
            return View(libraries);
        }

        public async Task<IActionResult> Details(int id) {
            Library library = await _libraryService.GetLibraryAsync(id);
            if (library == null)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.Membership = await _libraryMembershipService.GetMembershipAsync(id, userId);

            if (ViewBag.Membership.Role == MembershipRole.Manager)  {
                ViewBag.Members = _libraryMembershipService.GetLibraryMembers(id);

                var allBooks = await _bookService.GetAllAsync();
                var libraryBooks = await _libraryBookService.GetLibraryBooksAsync(library.Id);

                ViewBag.AvailableBooks = allBooks
                    .Where(book => !libraryBooks.Select(lb => lb.BookId).Contains(book.Id))
                    .ToList();
            }

            return View(library);
        }

        [Authorize]
        public IActionResult Create() {
            return View(new LibraryViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLibraryViewModel model) {
            if (!ModelState.IsValid)
                return View(model);

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Library library = new() {
                Name = model.Name
            };

            await _libraryService.CreateAsync(library);

            LibraryMembership membership = new() {
                LibraryId = library.Id,
                UserId = userId,
            };

            await _libraryMembershipService.CreateAsync(membership);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(int id) {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            LibraryMembership membership = new() {
                UserId = userId,
                LibraryId = id,
                Role = MembershipRole.Member,
            };


            await _libraryMembershipService.CreateAsync(membership);

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMember(int libraryId, string userId)  {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            LibraryMembership membership = await _libraryMembershipService.GetMembershipAsync(libraryId, userId);

            if (membership.Role != MembershipRole.Manager && userId != currentUserId)
                await _libraryMembershipService.DeleteAsync(libraryId, userId);
            
            return RedirectToAction(nameof(Details), new { id = libraryId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBook(int libraryId, int bookId) {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            LibraryMembership membership = await _libraryMembershipService.GetMembershipAsync(libraryId, userId);

            if (membership.Role == MembershipRole.Manager)
                await _libraryBookService.CreateAsync(libraryId, bookId);
            
            return RedirectToAction(nameof(Details), new { id = libraryId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveBook(int libraryId, int libraryBookId) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            LibraryMembership membership = await _libraryMembershipService.GetMembershipAsync(libraryId, userId);

            if (membership.Role == MembershipRole.Manager)
                await _libraryBookService.DeleteAsync(libraryBookId);
            
            return RedirectToAction(nameof(Details), new { id = libraryId });
        }

        public async Task<IActionResult> BookDetails(int libraryBookId) {
            LibraryBook libraryBook = await _libraryBookService.GetLibraryBookAsync(libraryBookId);
            if (libraryBook == null) return NotFound();

            BookDetailsViewModel viewModel = new() {
                LibraryBook = libraryBook,
                Book = libraryBook.Book,
                CurrentLoan = await _bookLoanService.GetCurrentBookLoanAsync(libraryBookId),
                PastLoans = _bookLoanService.GetBookLoans(libraryBookId),
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BorrowBook(int libraryBookId, DateTime dueDate) {
            LibraryBook libraryBook = await _libraryBookService.GetLibraryBookAsync(libraryBookId);

            if (libraryBook == null)
                throw new NotFoundException();
            
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            LibraryMembership membership = await _libraryMembershipService.GetMembershipAsync(libraryBook.LibraryId, userId);

            await _bookLoanService.CreateAsync(libraryBook.Id, membership.Id, dueDate);
            return RedirectToAction(nameof(BookDetails), new { libraryBookId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnBook(int bookLoanId) {
            await _bookLoanService.ReturnBookAsync(bookLoanId);
            BookLoan bookLoan = await _bookLoanService.GetBookLoanAsync(bookLoanId);

            return RedirectToAction(nameof(BookDetails), new { libraryBookId = bookLoan.LibraryBookId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id) {
            Library library = await _libraryService.GetLibraryAsync(id);
            if (library == null) return NotFound();

            LibraryViewModel viewModel = new() {
                Id = library.Id,
                Name = library.Name
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LibraryViewModel model) {
            if (!ModelState.IsValid)
                return View(model);

            await _libraryService.UpdateAsync(model.Id, model.Name);
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id) {
            Library library = await _libraryService.GetLibraryAsync(id);
            if (library == null) 
                return NotFound();

            return View(library);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            await _libraryService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}