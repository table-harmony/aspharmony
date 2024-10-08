using DataAccessLayer.Repositories;
using Utils.Exceptions;

namespace BusinessLogicLayer.Services {

    public interface IBookLoanService {
        Task<BookLoan?> GetBookLoanAsync(int id);
        Task CreateAsync(int libraryBookId, int libraryMembershipId, DateTime dueDate);
        Task ReturnBookAsync(int id);
        IEnumerable<BookLoan> GetBookLoans(int bookId);
        Task UpdateAsync(BookLoan loan);
        Task<BookLoan?> GetCurrentBookLoanAsync(int id);
    }

    public class BookLoanService(IBookLoanRepository loanRepository,
                           ILibraryBookRepository libraryBookRepository,
                           IBookRepository bookRepository) : IBookLoanService {
        public async Task<BookLoan?> GetBookLoanAsync(int id) {
            return await loanRepository.GetBookLoanAsync(id);
        }

        public IEnumerable<BookLoan> GetBookLoans(int bookId) {
            return loanRepository.GetBookLoans(bookId);
        }

        public async Task CreateAsync(int libraryBookId, int libraryMembershipId, DateTime dueDate) {
            var libraryBook = await libraryBookRepository.GetLibraryBookAsync(libraryBookId);
            if (libraryBook == null)
                throw new NotFoundException();

            var currentLoan = await GetCurrentBookLoanAsync(libraryBookId);
            if (currentLoan != null)
                throw new AuthorizationException();

            var loan = new BookLoan {
                LibraryBookId = libraryBookId,
                LibraryMembershipId = libraryMembershipId,
                LoanDate = DateTime.Now,
                DueDate = dueDate
            };

            await loanRepository.CreateAsync(loan);
        }

        public async Task UpdateAsync(BookLoan loan) {
            await loanRepository.UpdateAsync(loan);
        }

        public async Task<BookLoan?> GetCurrentBookLoanAsync(int libraryBookId) {
            return await loanRepository.GetCurrentLoanAsync(libraryBookId);
        }

        public async Task ReturnBookAsync(int id) {
            var loan = await GetBookLoanAsync(id);
            if (loan == null)
                throw new NotFoundException();

            loan.ReturnDate = DateTime.Now;
            await UpdateAsync(loan);
        }
    }
}