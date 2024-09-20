using DataAccessLayer.Entities;
using DataAccessLayer.Migrations;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Utils.Exceptions;

namespace DataAccessLayer.Repositories {
    public class LibraryMembershipRepository : ILibraryMembershipRepository {
        private readonly ApplicationContext _context;

        public LibraryMembershipRepository(ApplicationContext context) {
            _context = context;
        }

        public async Task CreateAsync(Entities.LibraryMembership membership) {
            await _context.LibraryMemberships.AddAsync(membership);
            await _context.SaveChangesAsync();
        }

        public async Task<Entities.LibraryMembership> GetByUser(int userId) {
            return await _context.LibraryMemberships
                         .FirstOrDefaultAsync(membership => membership.UserId == userId);
        }

        public async Task<Entities.LibraryMembership> GetById(int id) {
            return await _context.LibraryMemberships.FindAsync(id);    
        }

        public async Task UpdateAsync(Entities.LibraryMembership membership) {
            _context.LibraryMemberships.Update(membership);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) {
            Entities.LibraryMembership membership = await GetById(id);

            if (membership == null)
                throw new NotFoundException();

            _context.LibraryMemberships.Remove(membership);
            await _context.SaveChangesAsync();
        }
    }

}
