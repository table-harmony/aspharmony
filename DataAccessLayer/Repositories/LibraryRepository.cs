using DataAccessLayer.Entities;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Utils.Exceptions;

namespace DataAccessLayer.Repositories {
    public interface ILibraryRepository {
        Task<Library> GetLibraryAsync(int id);
        Task<IEnumerable<Library>> GetAllAsync();
        Task CreateAsync(Library library);
        Task UpdateAsync(Library library);
        Task DeleteAsync(int id);
    }

    public class LibraryRepository : ILibraryRepository {
        private readonly ApplicationContext _context;

        public LibraryRepository(ApplicationContext context) {
            _context = context;
        }

        public async Task<Library> GetLibraryAsync(int id) {
            return await _context.Libraries
                .AsNoTracking()
                .Include(l => l.Memberships)
                .Include(l => l.Books)
                    .ThenInclude(lb => lb.Book)
                    .ThenInclude(b => b.Author)
                .FirstOrDefaultAsync(l => l.Id == id);
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
            Library library = await GetLibraryAsync(id);

            if (library == null)
                throw new NotFoundException();

            _context.Libraries.Remove(library);
            await _context.SaveChangesAsync();
        }

        public async Task<Library> GetLibraryAsNoTrackingAsync(int id) {
            return await _context.Libraries
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);
        }
    }
}
