using DataAccessLayer.Repositories;
using DataAccessLayer.Entities;
using Utils.Exceptions;

namespace BusinessLogicLayer.Services {

    public interface IBookLoanService {
        Task<BookLoan> GetBookLoanAsync(int id);
        Task CreateAsync(int bookId, int libraryMembershipId, DateTime dueDate);
        Task ReturnBookAsync(int id);
        Task<IEnumerable<BookLoan>> GetBookLoansAsync(int bookId);
        Task UpdateAsync(BookLoan loan);
        Task<BookLoan> GetCurrentBookLoanAsync(int id);
    }

    public class BookLoanService : IBookLoanService {
        private readonly IBookLoanRepository _loanRepository;
        private readonly ILibraryBookService _libraryBookService;
        private readonly IBookService _bookService;

        public BookLoanService(IBookLoanRepository loanRepository, 
                               ILibraryBookService libraryBookService,
                               IBookService bookService) {
            _loanRepository = loanRepository;
            _libraryBookService = libraryBookService;
            _bookService = bookService;
        }

        public async Task<BookLoan> GetBookLoanAsync(int id) {
            var bookLoan = await _loanRepository.GetBookLoanAsync(id);
            if (bookLoan != null) {
                bookLoan.LibraryBook.Book = await _bookService.GetBookAsync(bookLoan.LibraryBook.BookId);
            }
            return bookLoan;
        }

        public async Task<IEnumerable<BookLoan>> GetBookLoansAsync(int bookId) {
            var bookLoans = _loanRepository.GetBookLoans(bookId);
            foreach (var loan in bookLoans) {
               loan.LibraryBook.Book = await _bookService.GetBookAsync(loan.LibraryBook.BookId);
            }
            return bookLoans;
        }

        public async Task CreateAsync(int bookId, int libraryMembershipId, DateTime dueDate) {
            LibraryBook libraryBook = await _libraryBookService.GetLibraryBookAsync(bookId);

            if (libraryBook == null)
                throw new NotFoundException();

            BookLoan currentLoan = await GetCurrentBookLoanAsync(bookId);
            if (currentLoan != null)
                throw new AuthorizationException();

            BookLoan loan = new() {
                LibraryBookId = libraryBook.Id,
                LibraryMembershipId = libraryMembershipId,
                LoanDate = DateTime.Now,
                DueDate = dueDate
            };

            await _loanRepository.CreateAsync(loan);
        }

        public async Task UpdateAsync(BookLoan loan) {
            await _loanRepository.UpdateAsync(loan);
        }

        public async Task<BookLoan> GetCurrentBookLoanAsync(int bookId) {
            var currentLoan = await _loanRepository.GetCurrentLoanAsync(bookId);
            if (currentLoan != null) {
                currentLoan.LibraryBook.Book = await _bookService.GetBookAsync(currentLoan.LibraryBook.BookId);
            }
            return currentLoan;
        }

        public async Task ReturnBookAsync(int id) {
            BookLoan loan = await GetBookLoanAsync(id);
            if (loan == null)
                throw new NotFoundException();

            loan.ReturnDate = DateTime.Now;
            await UpdateAsync(loan);
        }
    }
}