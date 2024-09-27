using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories {
    public interface ILibraryBookRepository {
        Task<LibraryBook> GetLibraryBookAsync(int id);
        Task<LibraryBook> GetLibraryBookAsync(int libraryId, int bookId);
        Task CreateAsync(LibraryBook book);
        Task<IEnumerable<LibraryBook>> GetLibraryBooksAsync(int libraryId);
        Task DeleteAsync(int id);
    }

    public class LibraryBookRepository : ILibraryBookRepository {
        private readonly ApplicationContext _context;

        public LibraryBookRepository(ApplicationContext context) {
            _context = context;
        }

        public async Task<LibraryBook> GetLibraryBookAsync(int id) {
            return await _context.LibraryBooks
                .AsNoTracking()
                .Include(lb => lb.Book)
                .FirstOrDefaultAsync(lb => lb.Id == id);
        }

        public async Task<LibraryBook> GetLibraryBookAsync(int libraryId, int bookId) {
            return await _context.LibraryBooks
                .AsNoTracking()
                .Include(lb => lb.Book)
                .FirstOrDefaultAsync(lb => lb.LibraryId == libraryId && lb.BookId == bookId);
        }

        public async Task<IEnumerable<LibraryBook>> GetLibraryBooksAsync(int libraryId) {
            return await _context.LibraryBooks
                .AsNoTracking()
                .Include(lb => lb.Book)
                .Where(lb => lb.LibraryId == libraryId)
                .ToListAsync();
        }

        public async Task DeleteAsync(int id) {
            var libraryBook = await _context.LibraryBooks.FindAsync(id);
            if (libraryBook != null) {
                _context.LibraryBooks.Remove(libraryBook);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateAsync(LibraryBook book) {
            await _context.LibraryBooks.AddAsync(book);
            await _context.SaveChangesAsync();
        }
    }
}
