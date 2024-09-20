using DataAccessLayer.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Exceptions;

namespace DataAccessLayer.Repositories {
    public class LibraryRepository : ILibraryRepository {
        private readonly ApplicationContext _context;

        public LibraryRepository(ApplicationContext context) {
            _context = context;
        }

        public async Task<Library> GetByIdAsync(int id) {
            return await _context.Libraries.FindAsync(id);
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
    }
}
