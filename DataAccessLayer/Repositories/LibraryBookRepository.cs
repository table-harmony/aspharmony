using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories {
    public interface ILibraryBookRepository {
        LibraryBook GetLibraryBook(int id);
        LibraryBook GetLibraryBook(int libraryId, int bookId);
        Task CreateAsync(LibraryBook book);
        IEnumerable<LibraryBook> GetLibraryBooks(int libraryId);
        Task DeleteAsync(int id);
    }

    public class LibraryBookRepository : ILibraryBookRepository {
        private readonly ApplicationContext _context;

        public LibraryBookRepository(ApplicationContext context) {
            _context = context;
        }

        public LibraryBook GetLibraryBook(int id) {
            return _context.LibraryBooks
                .Include(libraryBook => libraryBook.Library)
                .Include(libraryBook => libraryBook.Book)
                .First(libraryBook => libraryBook.Id == id);
        }

        public LibraryBook GetLibraryBook(int libraryId, int bookId) {
            return _context.LibraryBooks
                .Include(libraryBook => libraryBook.Library)
                .Include(libraryBook => libraryBook.Book)
                .First(libraryBook => libraryBook.Book.Id == bookId && 
                                                    libraryBook.Library.Id == libraryId);
        }

        public IEnumerable<LibraryBook> GetLibraryBooks(int libraryId) {
            return _context.LibraryBooks
                    .Include(lb => lb.Book)
                    .Where(lb => lb.LibraryId == libraryId)
                    .ToList();
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
