using DataAccessLayer.Entities;
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
        Task<BookLoan?> GetActiveLoanOrRequestAsync(int libraryBookId, int membershipId);
        Task CreateRequestAsync(int libraryBookId, int membershipId);
        Task ProcessNextInQueueAsync(int libraryBookId);
        Task<int> GetQueuePositionAsync(int libraryBookId, int membershipId);
        Task<IEnumerable<BookLoan>> GetQueueAsync(int libraryBookId);
        Task<BookLoan?> GetActiveLoanAsync(int libraryBookId);
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
                RequestDate = DateTime.Now,
                LoanDate = DateTime.Now,
                Status = LoanStatus.Active
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
            loan.Status = LoanStatus.Completed;
            await UpdateAsync(loan);

            await ProcessNextInQueueAsync(loan.LibraryBookId);
        }

        public async Task CreateRequestAsync(int libraryBookId, int membershipId) {
            var loan = new BookLoan {
                LibraryBookId = libraryBookId,
                LibraryMembershipId = membershipId,
                RequestDate = DateTime.Now,
                Status = LoanStatus.Requested
            };

            var activeLoan = await loanRepository.GetActiveLoanAsync(libraryBookId);
            if (activeLoan == null) {
                loan.Status = LoanStatus.Active;
                loan.LoanDate = DateTime.Now;
            }

            await loanRepository.CreateAsync(loan);
        }

        public async Task ProcessNextInQueueAsync(int libraryBookId) {
            var nextInQueue = await loanRepository.GetNextInQueueAsync(libraryBookId);
            if (nextInQueue != null) {
                nextInQueue.Status = LoanStatus.Active;
                nextInQueue.LoanDate = DateTime.Now;
                await loanRepository.UpdateAsync(nextInQueue);
            }
        }

        public async Task<int> GetQueuePositionAsync(int libraryBookId, int membershipId) {
            return await loanRepository.GetQueuePositionAsync(libraryBookId, membershipId);
        }

        public async Task<BookLoan?> GetActiveLoanOrRequestAsync(int libraryBookId, int membershipId) {
            var loan = await loanRepository.GetActiveLoanOrRequestAsync(libraryBookId, membershipId);
            
            if (loan?.Status == LoanStatus.Completed)
                return null;
            
            return loan;
        }

        public async Task<IEnumerable<BookLoan>> GetQueueAsync(int libraryBookId) {
            return await loanRepository.GetQueueAsync(libraryBookId);
        }

        public async Task<BookLoan?> GetActiveLoanAsync(int libraryBookId) {
            return await loanRepository.GetActiveLoanAsync(libraryBookId);
        }
    }
}