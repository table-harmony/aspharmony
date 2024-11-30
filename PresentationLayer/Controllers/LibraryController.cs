using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using System.Security.Claims;
using PresentationLayer.Models;
using Utils.Exceptions;
using BusinessLogicLayer.Events;
using DocumentFormat.OpenXml.Office2010.Excel;
using NuGet.Packaging.Signing;

namespace PresentationLayer.Controllers {
    [Authorize]
    public class LibraryController(
        ILibraryService libraryService,
        ILibraryMembershipService libraryMembershipService,
        ILibraryBookService libraryBookService,
        IBookService bookService,
        IBookLoanService bookLoanService,
        IEventPublisher eventPublisher) : Controller {

        public async Task<IActionResult> Index(string searchString, int pageSize = 10, int pageIndex = 1) {
            var libraries = await libraryService.GetAllAsync();
            if (!string.IsNullOrEmpty(searchString)) {
                libraries = libraries.Where(library => 
                                    library.Name.ToLower().Contains(searchString.ToLower()));
            }

            ViewBag.PageSize = pageSize;
            return View(PaginatedList<Library>.Create(libraries.AsQueryable(), pageIndex, pageSize));
        }

        public async Task<IActionResult> Details(int id, string searchString = "", int pageSize = 10, int pageIndex = 1) {
            var library = await libraryService.GetLibraryAsync(id);
            if (library == null)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var membership = await libraryMembershipService.GetMembershipAsync(id, userId);
            if (membership == null)
                return NotFound();

            ViewBag.Membership = membership;
            ViewBag.PageSize = pageSize;

            var libraryBooks = await libraryBookService.GetLibraryBooksAsync(id);

            var uniqueBooks = libraryBooks
                .GroupBy(lb => lb.Book.Id)
                .Select(g => g.First())
                .ToList();

            if (!string.IsNullOrEmpty(searchString)) {
                uniqueBooks = uniqueBooks.Where(lb => {
                    var book = lb.Book as BusinessLogicLayer.Services.Book;
                    return (book?.Metadata?.Title?.Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? false) ||
                           (book?.Author?.UserName?.Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? false);
                }).ToList();
            }

            var paginatedBooks = PaginatedList<LibraryBook>.Create(
                uniqueBooks.AsQueryable(),
                pageIndex, 
                pageSize
            );

            return View(new LibraryDetailsViewModel { 
                Library = library,
                Books = paginatedBooks
            });
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
        public async Task<IActionResult> Join(int libraryId) {
            var library = await libraryService.GetLibraryAsync(libraryId);

            if (library == null)
                return RedirectToAction(nameof(Index));

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            LibraryMembership membership = new() {
                UserId = userId,
                LibraryId = libraryId,
                Role = MembershipRole.Member,
            };

            await libraryMembershipService.CreateAsync(membership);

            return RedirectToAction(nameof(Details), new { id = libraryId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMember(int libraryId, string userId)  {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var membership = await libraryMembershipService.GetMembershipAsync(libraryId, userId);

            if (membership == null)
                return RedirectToAction(nameof(Index));

            if (userId == currentUserId || 
                (await IsLibraryManager(libraryId) && membership.Role != MembershipRole.Manager)) {
                await libraryMembershipService.DeleteAsync(libraryId, userId);
                
                if (userId == currentUserId)
                    return RedirectToAction(nameof(Index));
            }
            
            return RedirectToAction(nameof(Manage), new { id = libraryId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBook(int libraryId, int bookId, int copies = 1) {
            bool isManager = await IsLibraryManager(libraryId);

            if (!isManager)
                return RedirectToAction(nameof(Details), new { id = libraryId });

            for (int i = 0; i < copies; i++) {
                await libraryBookService.CreateAsync(libraryId, bookId);
            }

            return RedirectToAction(nameof(Details), new { id = libraryId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveBook(int libraryId, int libraryBookId) {
            bool isManager = await IsLibraryManager(libraryId);

            if (isManager)
                await libraryBookService.DeleteAsync(libraryBookId);
            
            return RedirectToAction(nameof(Details), new { id = libraryId });
        }

        [HttpGet]
        public async Task<IActionResult> BookDetails(int libraryBookId) {
            var libraryBook = await libraryBookService.GetLibraryBookAsync(libraryBookId);
            if (libraryBook == null) return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var membership = await libraryMembershipService.GetMembershipAsync(libraryBook.LibraryId, userId);
            if (membership == null) return NotFound();

            ViewBag.Membership = membership;

            var currentLoan = await bookLoanService.GetCurrentBookLoanAsync(libraryBookId);
            var pastLoans = bookLoanService.GetBookLoans(libraryBookId);
            var otherCopies = await libraryBookService.GetLibraryBooksAsync(libraryBook.LibraryId, libraryBook.BookId);
            
            var activeLoan = await bookLoanService.GetActiveLoanAsync(libraryBookId);
            var queuePosition = await bookLoanService.GetQueuePositionAsync(libraryBookId, membership.Id);
            var queue = await bookLoanService.GetQueueAsync(libraryBookId);

            var model = new BookDetailsViewModel {
                LibraryBook = libraryBook,
                Book = libraryBook.Book,
                CurrentLoan = currentLoan,
                PastLoans = pastLoans,
                OtherCopies = otherCopies.Where(lb => lb.Id != libraryBookId),
                ActiveLoan = activeLoan,
                QueuePosition = queuePosition,
                Queue = queue
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestBook(int libraryBookId) {
            var libraryBook = await libraryBookService.GetLibraryBookAsync(libraryBookId);
            if (libraryBook == null)
                return NotFound();
            
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var membership = await libraryMembershipService.GetMembershipAsync(libraryBook.LibraryId, userId);
            if (membership == null)
                return NotFound();

            var existingRequest = await bookLoanService.GetActiveLoanOrRequestAsync(libraryBookId, membership.Id);
            if (existingRequest != null)
                return BadRequest(new { error = "You already have an active request or loan for this book" });

            await bookLoanService.CreateRequestAsync(libraryBook.Id, membership.Id);
            return RedirectToAction(nameof(BookDetails), new { libraryBookId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnBook(int bookLoanId) {
            await bookLoanService.ReturnBookAsync(bookLoanId);
            var bookLoan = await bookLoanService.GetBookLoanAsync(bookLoanId);
            if (bookLoan == null)
                return NotFound();

            await bookLoanService.ProcessNextInQueueAsync(bookLoan.LibraryBookId);
            
            return RedirectToAction(nameof(BookDetails), new { libraryBookId = bookLoan.LibraryBookId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id) {
            var library = await libraryService.GetLibraryAsync(id);
            if (library == null) return NotFound();

            bool isManager = await IsLibraryManager(id);
            if (!isManager) 
                return RedirectToAction(nameof(Details), new { id });

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

            bool isManager = await IsLibraryManager(library.Id);
            if (!isManager) return RedirectToAction(nameof(Details), new { id = library.Id });

            library.Name = model.Name;
            library.AllowCopies = model.AllowCopies;

            await libraryService.UpdateAsync(library);
            return RedirectToAction(nameof(Manage), new { id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id) {
            var library = await libraryService.GetLibraryAsync(id);
            if (library == null) 
                return NotFound();

            bool isManager = await IsLibraryManager(library.Id);
            if (!isManager) 
                return RedirectToAction(nameof(Details), new { id });

            return View(library);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            bool isManager = await IsLibraryManager(id);
            if (!isManager) 
                return RedirectToAction(nameof(Details), new { id });

            await libraryService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));                        
        }

        [HttpGet]
        public async Task<IActionResult> Manage(int id, string searchString, int pageSize = 10, int pageIndex = 1) {
            if (!await IsLibraryManager(id))
                return RedirectToAction(nameof(Details), new { id });

            var library = await libraryService.GetLibraryAsync(id);
            var members = libraryMembershipService.GetLibraryMembers(id);
            
            if (!string.IsNullOrEmpty(searchString)) {
                members = members.Where(m => 
                    m.User.UserName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    m.User.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }
            
            ViewBag.PageSize = pageSize;
            var paginatedMembers = PaginatedList<LibraryMembership>.Create(
                members.AsQueryable(), pageIndex, pageSize);

            return View(new ManageMembersViewModel  { 
                Library = library,
                Members = paginatedMembers,
                SearchString = searchString
            });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PromoteMember(int libraryId, string userId) {
            if (!await IsLibraryManager(libraryId))
                return RedirectToAction(nameof(Details), new { id = libraryId });

            var membership = await libraryMembershipService.GetMembershipAsync(libraryId, userId);
            if (membership != null) {
                membership.Role = MembershipRole.Manager;
                await libraryMembershipService.UpdateAsync(membership);
            }

            return RedirectToAction(nameof(Manage), new { id = libraryId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DemoteMember(int libraryId, string userId) {
            if (!await IsLibraryManager(libraryId))
                return RedirectToAction(nameof(Details), new { id = libraryId });

            var membership = await libraryMembershipService.GetMembershipAsync(libraryId, userId);
            if (membership != null) {
                membership.Role = MembershipRole.Member;
                await libraryMembershipService.UpdateAsync(membership);
            }

            return RedirectToAction(nameof(Manage), new { id = libraryId });
        }

        [HttpGet]
        public async Task<IActionResult> AddBook(int id, string searchString, int pageSize = 10, int pageIndex = 1) {
            var library = await libraryService.GetLibraryAsync(id);
            if (library == null) return NotFound();

            bool isManager = await IsLibraryManager(id);
            if (!isManager) return Forbid();

            var allBooks = await bookService.GetAllAsync();
            var libraryBooks = await libraryBookService.GetLibraryBooksAsync(id);
            var availableBooks = allBooks;

            if (!library.AllowCopies)
                availableBooks = availableBooks.Where(b => !libraryBooks.Any(lb => lb.BookId == b.Id));

            if (!string.IsNullOrEmpty(searchString)) {
                availableBooks = availableBooks.Where(b => 
                    b.Metadata.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    (b.Author?.UserName?.Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? false)
                );
            }

            var paginatedBooks = PaginatedList<BusinessLogicLayer.Services.Book>.Create(availableBooks.AsQueryable(), pageIndex, pageSize);

            ViewBag.SearchString = searchString;
            ViewBag.PageSize = pageSize;
            ViewBag.PageIndex = pageIndex;
            ViewBag.TotalPages = paginatedBooks.TotalPages;

            var model = new AddBookViewModel {
                Library = library,
                AvailableBooks = [.. paginatedBooks.Items]
            };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Profile(int id) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var membership = await libraryMembershipService.GetMembershipAsync(id, userId);

            if (membership == null)
                return NotFound();

            var allLoans = await bookLoanService.GetMemberLoansAsync(membership.Id);

            var viewModel = new LibraryProfileViewModel {
                Membership = membership,
                RequestedLoans = allLoans.Where(l => l.Status == LoanStatus.Requested),
                ActiveLoans = allLoans.Where(l => l.Status == LoanStatus.Active),
                PastLoans = allLoans.Where(l => l.Status == LoanStatus.Completed)
                    .OrderByDescending(l => l.ReturnDate)
            };

            return View(viewModel);
        }

        private async Task<bool> IsLibraryManager(int libraryId) {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var membership = await libraryMembershipService.GetMembershipAsync(libraryId, userId);
            return membership?.Role == MembershipRole.Manager;
        }
    }
}