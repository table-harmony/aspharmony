using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Utils.Exceptions;

namespace DataAccessLayer.Repositories {
    public interface IBookRepository {
        Task<Book> GetBookAsync(int id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task<int> CreateAsync(Book book);
        Task DeleteAsync(int id);
    }

    public class BookRepository : IBookRepository {
        private readonly ApplicationContext _context;
        
        public BookRepository(ApplicationContext context) {
            _context = context;
        }

        public async Task<Book> GetBookAsync(int id) {
            return await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> GetAllAsync() {
            return await _context.Books
                .Include(b => b.Author)
                .ToListAsync();
        }

        public async Task<int> CreateAsync(Book book) {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return book.Id;
        }

        public async Task DeleteAsync(int id) {
            Book book = await GetBookAsync(id);

            if (book == null)
                throw new NotFoundException();

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }

}
