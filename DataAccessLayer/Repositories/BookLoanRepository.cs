using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories {

    public interface IBookLoanRepository
    {
        Task<BookLoan> GetByIdAsync(int id);
        Task<IEnumerable<BookLoan>> GetByUserIdAsync(string userId);
        Task<IEnumerable<BookLoan>> GetByLibraryIdAsync(int libraryId);
        Task CreateAsync(BookLoan bookLoan);
        Task UpdateAsync(BookLoan bookLoan);
        Task<IEnumerable<BookLoan>> GetPastLoansByLibraryBookIdAsync(int libraryBookId);
        Task<BookLoan> GetCurrentLoanByLibraryBookIdAsync(int libraryBookId);
    }

    public class BookLoanRepository : IBookLoanRepository
    {
        private readonly ApplicationContext _context;

        public BookLoanRepository(ApplicationContext context) {
            _context = context;
        }

        public async Task<BookLoan> GetByIdAsync(int id) {
            return await _context.BookLoans
                .Include(bl => bl.User)
                .Include(bl => bl.LibraryBook)
                    .ThenInclude(lb => lb.Book)
                .Include(bl => bl.Library)
                .FirstOrDefaultAsync(bl => bl.Id == id);
        }

        public async Task<IEnumerable<BookLoan>> GetByUserIdAsync(string userId) {
            return await _context.BookLoans
                .Include(bl => bl.User)
                .Include(bl => bl.LibraryBook)
                    .ThenInclude(lb => lb.Book)
                .Include(bl => bl.Library)
                .Where(bl => bl.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<BookLoan>> GetByLibraryIdAsync(int libraryId) {
            return await _context.BookLoans
                .Include(bl => bl.User)
                .Include(bl => bl.LibraryBook)
                    .ThenInclude(lb => lb.Book)
                .Include(bl => bl.Library)
                .Where(bl => bl.LibraryId == libraryId)
                .ToListAsync();
        }

        public async Task CreateAsync(BookLoan bookLoan) {
            await _context.BookLoans.AddAsync(bookLoan);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BookLoan bookLoan) {
            _context.BookLoans.Update(bookLoan);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookLoan>> GetPastLoansByLibraryBookIdAsync(int libraryBookId) {
            return await _context.BookLoans
                .Include(bl => bl.User)
                .Where(bl => bl.LibraryBookId == libraryBookId && bl.ReturnDate.HasValue)
                .OrderByDescending(bl => bl.LoanDate)
                .ToListAsync();
        }

        public async Task<BookLoan> GetCurrentLoanByLibraryBookIdAsync(int libraryBookId) {
            return await _context.BookLoans
                .Include(bl => bl.User)
                .Where(bl => bl.LibraryBookId == libraryBookId && !bl.ReturnDate.HasValue)
                .FirstOrDefaultAsync();
        }
    }
}