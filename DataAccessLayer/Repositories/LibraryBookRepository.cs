using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories {
    public interface ILibraryBookRepository {
        Task<LibraryBook?> GetLibraryBookAsync(int id);
        Task CreateAsync(LibraryBook book);
        Task<IEnumerable<LibraryBook?>> GetLibraryBooksAsync(int libraryId, int bookId);
        Task<IEnumerable<LibraryBook>> GetLibraryBooksAsync(int libraryId);
        Task DeleteAsync(int id);
    }

    public class LibraryBookRepository(ApplicationContext context) : ILibraryBookRepository {
        public async Task<LibraryBook?> GetLibraryBookAsync(int id) {
            return await context.LibraryBooks
                .AsNoTracking()
                .Include(lb => lb.Library)
                .Include(lb => lb.Book)
                .FirstOrDefaultAsync(lb => lb.Id == id);
        }

        public async Task<IEnumerable<LibraryBook?>> GetLibraryBooksAsync(int libraryId, int bookId) {
            return await context.LibraryBooks
                .AsNoTracking()
                .Include(lb => lb.Library)
                .Include(lb => lb.Book)
                .Where(lb => lb.LibraryId == libraryId && lb.BookId == bookId)
                .ToListAsync();
        }

        public async Task<IEnumerable<LibraryBook>> GetLibraryBooksAsync(int libraryId) {
            return await context.LibraryBooks
                .AsNoTracking()
                .Include(lb => lb.Book)
                .Where(lb => lb.LibraryId == libraryId)
                .ToListAsync();
        }

        public async Task DeleteAsync(int id) {
            var libraryBook = await context.LibraryBooks.FindAsync(id);
            if (libraryBook != null) {
                context.LibraryBooks.Remove(libraryBook);
                await context.SaveChangesAsync();
            }
        }

        public async Task CreateAsync(LibraryBook book) {
            await context.LibraryBooks.AddAsync(book);
            await context.SaveChangesAsync();
        }
    }
}
