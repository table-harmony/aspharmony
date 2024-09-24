using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories {

    public interface IBookLoanRepository {
        Task<BookLoan> GetBookLoanAsync(int id);
        Task CreateAsync(BookLoan bookLoan);
        Task UpdateAsync(BookLoan bookLoan);
        IEnumerable<BookLoan> GetBookLoans(int bookId);
        Task<BookLoan> GetCurrentLoanAsync(int id);
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

        public async Task<BookLoan> GetCurrentLoanAsync(int id) {
            return await _context.BookLoans
                .Include(libraryBook => libraryBook.LibraryMembership)
                    .ThenInclude(membership => membership.User)
                .Where(libraryBook => libraryBook.LibraryBookId == id && !libraryBook.ReturnDate.HasValue)
                .FirstOrDefaultAsync();
        }
    }
}