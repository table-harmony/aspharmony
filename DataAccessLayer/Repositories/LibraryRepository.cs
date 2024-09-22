using DataAccessLayer.Entities;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Utils.Exceptions;

namespace DataAccessLayer.Repositories {
    public interface ILibraryRepository {
        IEnumerable<LibraryBook> GetBooks(int id);
        IEnumerable<LibraryMembership> GetMemberships(int id);
        Task<Library> GetByIdAsync(int id);
        Task<IEnumerable<Library>> GetAllAsync();
        Task CreateAsync(Library library);
        Task UpdateAsync(Library library);
        Task DeleteAsync(int id);
        Task AddBookAsync(LibraryBook libraryBook);
        Task<LibraryBook> GetLibraryBookByIdAsync(int libraryBookId);
    }

    public class LibraryRepository : ILibraryRepository {
        private readonly ApplicationContext _context;

        public LibraryRepository(ApplicationContext context) {
            _context = context;
        }

        public async Task<Library> GetByIdAsync(int id) {
            return await _context.Libraries
                .Include(l => l.Memberships)
                .Include(l => l.Books)
                    .ThenInclude(lb => lb.Book)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public IEnumerable<LibraryMembership> GetMemberships(int id) {
            var memberships = _context.LibraryMemberships
                                .Include(lb => lb.User)
                                .Where(lb => lb.LibraryId == id)
                                .ToList();

            return memberships;
        }

        public IEnumerable<LibraryBook> GetBooks(int id) {
            var books = _context.LibraryBooks
                            .Include(lb => lb.Book)
                            .Where(lb => lb.LibraryId == id)
                            .ToList();

            return books;
        }

        public async Task<IEnumerable<Library>> GetAllAsync() {
            return await _context.Libraries.ToListAsync();
        }

        public async Task CreateAsync(Library library) {
            await _context.Libraries.AddAsync(library);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Library library) {
            _context.Libraries.Update(library);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) {
            Library library = await GetByIdAsync(id);

            if (library == null)
                throw new NotFoundException();

            _context.Libraries.Remove(library);
            await _context.SaveChangesAsync();
        }

        public async Task AddBookAsync(LibraryBook libraryBook) {
            await _context.LibraryBooks.AddAsync(libraryBook);
            await _context.SaveChangesAsync();
        }

        public async Task<LibraryBook> GetLibraryBookByIdAsync(int libraryBookId) {
            return await _context.LibraryBooks
                .Include(lb => lb.Book)
                .Include(lb => lb.Library)
                .FirstOrDefaultAsync(lb => lb.Id == libraryBookId);
        }
    }
}
