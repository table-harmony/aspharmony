using DataAccessLayer.Repositories;
using DataAccessLayer.Entities;
using Utils.Exceptions;

namespace BusinessLogicLayer.Services {

    public interface IBookLoanService {
        Task<BookLoan> GetBookLoanAsync(int id);
        Task CreateAsync(int bookId, int libraryMembershipId, DateTime dueDate);
        Task ReturnBookAsync(int id);
        IEnumerable<BookLoan> GetBookLoans(int bookId);
        Task UpdateAsync(BookLoan loan);
        Task<BookLoan> GetCurrentBookLoanAsync(int bookId);
    }

    public class BookLoanService : IBookLoanService {
        private readonly IBookLoanRepository _loanRepository;
        private readonly ILibraryBookService _libraryBookService;

        public BookLoanService(IBookLoanRepository loanRepository, ILibraryBookService libraryBookService) {
            _loanRepository = loanRepository;
            _libraryBookService = libraryBookService;
        }

        public async Task<BookLoan> GetBookLoanAsync(int id) {
            return await _loanRepository.GetBookLoanAsync(id);
        }

        public IEnumerable<BookLoan> GetBookLoans(int bookId) {
            return _loanRepository.GetBookLoans(bookId);
        }

        public async Task CreateAsync(int bookId, int libraryMembershipId, DateTime dueDate) {
            LibraryBook libraryBook = _libraryBookService.GetLibraryBook(bookId);

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
            return await _loanRepository.GetCurrentLoanAsync(bookId);
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