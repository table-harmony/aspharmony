using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories {

    public interface IBookLoanRepository {
        Task<BookLoan> GetBookLoanAsync(int id);
        Task CreateAsync(BookLoan bookLoan);
        Task UpdateAsync(BookLoan bookLoan);
        IEnumerable<BookLoan> GetBookLoans(int bookId);
        Task<BookLoan> GetCurrentLoanAsync(int id);
        IEnumerable<BookLoan> GetMemberLoans(int membershipId);
    }

    public class BookLoanRepository : IBookLoanRepository {
        private readonly ApplicationContext _context;

        public BookLoanRepository(ApplicationContext context) {
            _context = context;
        }

        public async Task<BookLoan> GetBookLoanAsync(int id) {
            return await _context.BookLoans
                .Include(book => book.LibraryMembership)
                    .ThenInclude(membership => membership.User)
                .Include(book => book.LibraryBook)
                    .ThenInclude(book => book.Book)
                .FirstOrDefaultAsync(book => book.Id == id);
        }

        public async Task CreateAsync(BookLoan bookLoan) {
            await _context.BookLoans.AddAsync(bookLoan);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BookLoan bookLoan) {
            _context.BookLoans.Update(bookLoan);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<BookLoan> GetBookLoans(int bookId) {
            return _context.BookLoans
                .Include(libraryBook => libraryBook.LibraryMembership)
                    .ThenInclude(m => m.User)
                .Where(libraryBook => libraryBook.LibraryBookId == bookId)
                .OrderByDescending(libraryBook => libraryBook.LoanDate)
                .ToList();
        }

        public IEnumerable<BookLoan> GetMemberLoans(int membershipId) {
            return _context.BookLoans
                .Include(libraryBook => libraryBook.LibraryMembership)
                .Where(libraryBook => libraryBook.LibraryMembershipId == membershipId)
                .ToList();
        } 

        public async Task<BookLoan> GetCurrentLoanAsync(int libraryBookId) {
            return await _context.BookLoans
                .AsNoTracking()
                .Include(bl => bl.LibraryMembership)
                    .ThenInclude(lm => lm.User)
                .FirstOrDefaultAsync(bl => bl.LibraryBookId == libraryBookId && !bl.ReturnDate.HasValue);
        }
    }
}