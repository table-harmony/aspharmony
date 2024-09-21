using DataAccessLayer.Entities;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Utils.Exceptions;

namespace DataAccessLayer.Repositories {
    public interface ILibraryMembershipRepository {
        Task<LibraryMembership> GetById(int id);
        Task<LibraryMembership> GetByUser(string userId);
        Task CreateAsync(LibraryMembership membership);
        Task UpdateAsync(LibraryMembership membership);
        Task DeleteAsync(int id);
    }

    public class LibraryMembershipRepository : ILibraryMembershipRepository {
        private readonly ApplicationContext _context;

        public LibraryMembershipRepository(ApplicationContext context) {
            _context = context;
        }

        public async Task CreateAsync(LibraryMembership membership) {
            await _context.LibraryMemberships.AddAsync(membership);
            await _context.SaveChangesAsync();
        }

        public async Task<LibraryMembership> GetByUser(string userId) {
            return await _context.LibraryMemberships
                         .FirstOrDefaultAsync(membership => membership.UserId == userId);
        }

        public async Task<LibraryMembership> GetById(int id) {
            return await _context.LibraryMemberships.FindAsync(id);    
        }

        public async Task UpdateAsync(LibraryMembership membership) {
            _context.LibraryMemberships.Update(membership);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) {
            LibraryMembership membership = await GetById(id);

            if (membership == null)
                throw new NotFoundException();

            _context.LibraryMemberships.Remove(membership);
            await _context.SaveChangesAsync();
        }
    }

}
