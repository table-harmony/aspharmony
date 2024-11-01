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
    public class LibraryController(
        ILibraryService libraryService,
        ILibraryMembershipService libraryMembershipService,
        ILibraryBookService libraryBookService,
        IBookService bookService,
        IBookLoanService bookLoanService,
        IEventPublisher eventPublisher) : Controller {

        public async Task<IActionResult> Index(string searchString) {
            var libraries = await libraryService.GetAllAsync();
            if (!string.IsNullOrEmpty(searchString)) {
                libraries = libraries.Where(library => 
                                    library.Name.ToLower().Contains(searchString.ToLower()));
            }
            return View(libraries);
        }

        public async Task<IActionResult> Details(int id, string searchString) {
            Library library = await libraryService.GetLibraryAsync(id);
            if (library == null)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.Membership = await libraryMembershipService.GetMembershipAsync(id, userId);

            if (ViewBag.Membership.Role == MembershipRole.Manager)  {
                ViewBag.Members = libraryMembershipService.GetLibraryMembers(id);

                var allBooks = await bookService.GetAllAsync();
                var libraryBooks = await libraryBookService.GetLibraryBooksAsync(library.Id);

                var availableBooks = allBooks;

                if (!string.IsNullOrEmpty(searchString)) {
                    availableBooks = availableBooks.Where(book =>
                        book.Metadata.Title.Contains(searchString, StringComparison.CurrentCultureIgnoreCase) ||
                        book.Author.UserName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));
                }

                if (!library.AllowCopies) {
                    availableBooks = allBooks
                        .Where(book => !libraryBooks.Select(lb => lb.BookId).Contains(book.Id));
                }

                ViewBag.AvailableBooks = availableBooks.ToList();
                ViewBag.SearchString = searchString;
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
        public async Task<IActionResult> Create(CreateLibraryViewModel model) {
            if (!ModelState.IsValid)
                return View(model);

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            Library library = new() {
                Name = model.Name
            };

            await libraryService.CreateAsync(library);
            LibraryMembership membership = new() {
                LibraryId = library.Id,
                UserId = userId,
            };

            await libraryMembershipService.CreateAsync(membership);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(int id) {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var library = await libraryService.GetLibraryAsync(id);

            if (library == null)
                return NotFound();

            LibraryMembership membership = new() {
                UserId = userId,
                LibraryId = id,
                Role = MembershipRole.Member,
            };

            await libraryMembershipService.CreateAsync(membership);

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMember(int libraryId, string userId)  {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var membership = await libraryMembershipService.GetMembershipAsync(libraryId, userId);

            if (membership == null)
                return NotFound();

            if (membership.Role != MembershipRole.Manager && userId != currentUserId)
                await libraryMembershipService.DeleteAsync(libraryId, userId);
            
            return RedirectToAction(nameof(Details), new { id = libraryId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBook(int libraryId, int bookId) {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var membership = await libraryMembershipService.GetMembershipAsync(libraryId, userId);

            if (membership == null) 
                return NotFound();

            if (membership.Role == MembershipRole.Manager)
                await libraryBookService.CreateAsync(libraryId, bookId);
            
            return RedirectToAction(nameof(Details), new { id = libraryId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveBook(int libraryId, int libraryBookId) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var membership = await libraryMembershipService.GetMembershipAsync(libraryId, userId);

            if (membership == null)
                return NotFound();

            if (membership.Role == MembershipRole.Manager)
                await libraryBookService.DeleteAsync(libraryBookId);
            
            return RedirectToAction(nameof(Details), new { id = libraryId });
        }

        [HttpGet]
        public async Task<IActionResult> BookDetails(int libraryBookId) {
            var libraryBook = await libraryBookService.GetLibraryBookAsync(libraryBookId);
            if (libraryBook == null) return NotFound();

            BookDetailsViewModel model = new() {
                LibraryBook = libraryBook,
                Book = libraryBook.Book,
                CurrentLoan = await bookLoanService.GetCurrentBookLoanAsync(libraryBookId),
                PastLoans = bookLoanService.GetBookLoans(libraryBookId),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BorrowBook(int libraryBookId, DateTime dueDate) {
            var libraryBook = await libraryBookService.GetLibraryBookAsync(libraryBookId);

            if (libraryBook == null)
                throw new NotFoundException();
            
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var membership = await libraryMembershipService.GetMembershipAsync(libraryBook.LibraryId, userId);

            if (membership == null)
                return NotFound();

            await bookLoanService.CreateAsync(libraryBook.Id, membership.Id, dueDate);
            return RedirectToAction(nameof(BookDetails), new { libraryBookId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnBook(int bookLoanId) {
            await bookLoanService.ReturnBookAsync(bookLoanId);
            var bookLoan = await bookLoanService.GetBookLoanAsync(bookLoanId);

            if (bookLoan == null)
                return NotFound();

            return RedirectToAction(nameof(BookDetails), new { libraryBookId = bookLoan.LibraryBookId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id) {
            var library = await libraryService.GetLibraryAsync(id);
            if (library == null) return NotFound();

            LibraryViewModel viewModel = new() {
                Id = library.Id,
                Name = library.Name,
                AllowCopies = library.AllowCopies,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LibraryViewModel model) {
            if (!ModelState.IsValid)
                return View(model);

            Library? library = await libraryService.GetLibraryAsync(model.Id);
            if (library == null)
                return NotFound();

            library.Name = model.Name;
            library.AllowCopies = model.AllowCopies;

            await libraryService.UpdateAsync(library);
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id) {
            var library = await libraryService.GetLibraryAsync(id);
            if (library == null) 
                return NotFound();

            return View(library);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            await libraryService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}