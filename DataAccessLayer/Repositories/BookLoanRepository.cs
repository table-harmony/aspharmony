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
        IEnumerable<BookLoan> GetMemberLoans(int membershipId);
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
                .Include(libraryBook => libraryBook.LibraryMembership)
                    .ThenInclude(m => m.User)
                .Where(libraryBook => libraryBook.LibraryBookId == bookId)
                .OrderByDescending(libraryBook => libraryBook.LoanDate);
        }

        public IEnumerable<BookLoan> GetMemberLoans(int membershipId) {
            return context.BookLoans
                .Include(libraryBook => libraryBook.LibraryMembership)
                .Where(libraryBook => libraryBook.LibraryMembershipId == membershipId);
        } 

        public async Task<BookLoan?> GetCurrentLoanAsync(int libraryBookId) {
            return await context.BookLoans
                .AsNoTracking()
                .Include(bl => bl.LibraryMembership)
                    .ThenInclude(lm => lm.User)
                .FirstOrDefaultAsync(bl => bl.LibraryBookId == libraryBookId && !bl.ReturnDate.HasValue);
        }
    }
}