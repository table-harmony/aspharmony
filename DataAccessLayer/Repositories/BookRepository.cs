using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Utils.Exceptions;

namespace DataAccessLayer.Repositories {
    public interface IBookRepository {
        Task<Book?> GetBookAsync(int id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task<IEnumerable<Book>> GetAllAsync(ServerType serverType);
        Task<Book> CreateAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);
        IDbContextTransaction BeginTransaction();
        Task CreateAudioBookAsync(AudioBook audioBook);
        Task<AudioBook?> GetAudioBookAsync(int id);
        Task DeleteAudioBookAsync(int id);
    }

    public class BookRepository(ApplicationContext context) : IBookRepository {
        public async Task<Book?> GetBookAsync(int id) {
            return await context.Books
                .Include(book => book.Author)
                .Include(book => book.AudioBooks)
                .AsNoTracking()
                .FirstOrDefaultAsync(book => book.Id == id);
        }

        public async Task<IEnumerable<Book>> GetUserBooksAsync(string userId) {
            return await context.Books
                .FromSqlRaw("SELECT * FROM dbo.GetBooksByUser({0})", userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAllAsync() {
            return await context.Books
                .Include(book => book.Author)
                .Include(book => book.AudioBooks)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAllAsync(ServerType serverType) {
            return await context.Books
                .Where(book => book.Server == serverType)
                .Include(book => book.Author)
                .Include(book => book.AudioBooks)
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task UpdateAsync(Book book) {
            context.Books.Update(book);
            await context.SaveChangesAsync();
        }

        public async Task<Book> CreateAsync(Book book) {
            await context.Books.AddAsync(book);
            await context.SaveChangesAsync();

            return book;
        }

        public async Task DeleteAsync(int id) {
            using var transaction = await context.Database.BeginTransactionAsync();

            try {
                var book = await context.Books
                    .Include(b => b.LibraryBooks)
                        .ThenInclude(lb => lb.Loans)
                    .Include(b => b.AudioBooks)
                    .FirstOrDefaultAsync(b => b.Id == id)
                    ?? throw new NotFoundException();

                var loans = book.LibraryBooks.SelectMany(lb => lb.Loans);
                context.BookLoans.RemoveRange(loans);

                context.LibraryBooks.RemoveRange(book.LibraryBooks);

                context.Books.Remove(book);

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            } catch {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public IDbContextTransaction BeginTransaction() {
            return context.Database.BeginTransaction();
        }

        public async Task CreateAudioBookAsync(AudioBook audioBook) {
            await context.AudioBooks.AddAsync(audioBook);
            await context.SaveChangesAsync();
        }

        public async Task<AudioBook?> GetAudioBookAsync(int id) {
            return await context.AudioBooks
                .Include(a => a.Book)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task DeleteAudioBookAsync(int id) {
            var audioBook = await context.AudioBooks.FindAsync(id);
            if (audioBook == null) return;

            context.AudioBooks.Remove(audioBook);
            await context.SaveChangesAsync();
        }
    }
}
