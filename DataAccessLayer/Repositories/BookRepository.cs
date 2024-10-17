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
    }

    public class BookRepository(ApplicationContext context) : IBookRepository {
        public async Task<Book?> GetBookAsync(int id) {
            return await context.Books
                .Include(book => book.Author)
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
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAllAsync(ServerType serverType) {
            return await context.Books
                .Where(book => book.Server == serverType)
                .Include(book => book.Author)
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
            Book book = await GetBookAsync(id) ?? throw new NotFoundException();

            context.Books.Remove(book);
            await context.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction() {
            return context.Database.BeginTransaction();
        }
    }

}
