using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories {

    public interface IBookLoanRepository {
        Task<BookLoan?> GetBookLoanAsync(int id);
        Task CreateAsync(BookLoan bookLoan);
        Task UpdateAsync(BookLoan bookLoan);
        IEnumerable<BookLoan> GetBookLoans(int bookId);
        Task<BookLoan?> GetCurrentLoanAsync(int id);
        Task<IEnumerable<BookLoan>> GetMemberLoansAsync(int membershipId);
        IEnumerable<BookLoan> GetMemberLoans(int membershipId);
        Task<BookLoan?> GetActiveLoanAsync(int libraryBookId);
        Task<BookLoan?> GetNextInQueueAsync(int libraryBookId);
        Task<int> GetQueuePositionAsync(int libraryBookId, int membershipId);
        Task<BookLoan?> GetActiveLoanOrRequestAsync(int libraryBookId, int membershipId);
        Task<IEnumerable<BookLoan>> GetQueueAsync(int libraryBookId);
    }

    public class BookLoanRepository(ApplicationContext context) : IBookLoanRepository {
        public async Task<BookLoan?> GetBookLoanAsync(int id) {
            return await context.BookLoans
                .Include(book => book.LibraryMembership)
                    .ThenInclude(membership => membership.User)
                .Include(book => book.LibraryBook)
                    .ThenInclude(book => book.Book)
                .FirstOrDefaultAsync(book => book.Id == id);
        }

        public async Task CreateAsync(BookLoan bookLoan) {
            await context.BookLoans.AddAsync(bookLoan);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BookLoan bookLoan) {
            context.BookLoans.Update(bookLoan);
            await context.SaveChangesAsync();
        }

        public IEnumerable<BookLoan> GetBookLoans(int bookId) {
            return context.BookLoans
                .Include(bookLoan => bookLoan.LibraryMembership)
                    .ThenInclude(membership => membership.User)
                .Where(bookLoan => bookLoan.LibraryBookId == bookId)
                .OrderByDescending(bookLoan => bookLoan.LoanDate);
        }

        public IEnumerable<BookLoan> GetMemberLoans(int membershipId) {
            return context.BookLoans
                .Include(bookLoan => bookLoan.LibraryMembership)
                .Where(bookLoan => bookLoan.LibraryMembershipId == membershipId);
        }

        public async Task<IEnumerable<BookLoan>> GetMemberLoansAsync(int membershipId) {
            return await context.BookLoans
                .AsNoTracking()
                .Include(bookLoan => bookLoan.LibraryMembership)
                .Include(bookLoan => bookLoan.LibraryBook)
                .Where(bookLoan => bookLoan.LibraryMembershipId == membershipId).ToListAsync();
        }

        public async Task<BookLoan?> GetCurrentLoanAsync(int libraryBookId) {
            return await context.BookLoans
                .AsNoTracking()
                .Include(bl => bl.LibraryMembership)
                    .ThenInclude(lm => lm.User)
                .FirstOrDefaultAsync(bl => bl.LibraryBookId == libraryBookId && !bl.ReturnDate.HasValue);
        }

        public async Task<BookLoan?> GetActiveLoanAsync(int libraryBookId) {
            return await context.BookLoans
                .FirstOrDefaultAsync(bl => 
                    bl.LibraryBookId == libraryBookId && 
                    bl.Status == LoanStatus.Active);
        }

        public async Task<BookLoan?> GetNextInQueueAsync(int libraryBookId) {
            return await context.BookLoans
                .Where(bl => 
                    bl.LibraryBookId == libraryBookId && 
                    bl.Status == LoanStatus.Requested)
                .Include(bl => bl.LibraryMembership)
                .OrderBy(bl => bl.RequestDate)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetQueuePositionAsync(int libraryBookId, int membershipId) {
            var loans = await context.BookLoans
                .Where(bl => 
                    bl.LibraryBookId == libraryBookId && 
                    bl.Status == LoanStatus.Requested)
                .OrderBy(bl => bl.RequestDate)
                .ToListAsync();

            return loans.FindIndex(bl => bl.LibraryMembershipId == membershipId) + 1;
        }

        public async Task<BookLoan?> GetActiveLoanOrRequestAsync(int libraryBookId, int membershipId) {
            return await context.BookLoans
                .FirstOrDefaultAsync(bl => 
                    bl.LibraryBookId == libraryBookId && 
                    bl.LibraryMembershipId == membershipId && 
                    (bl.Status == LoanStatus.Active || bl.Status == LoanStatus.Requested));
        }

        public async Task<IEnumerable<BookLoan>> GetQueueAsync(int libraryBookId) {
            return await context.BookLoans
                .Include(bl => bl.LibraryMembership)
                    .ThenInclude(lm => lm.User)
                .Where(bl => 
                    bl.LibraryBookId == libraryBookId && 
                    bl.Status == LoanStatus.Requested)
                .OrderBy(bl => bl.RequestDate)
                .ToListAsync();
        }
    }
}