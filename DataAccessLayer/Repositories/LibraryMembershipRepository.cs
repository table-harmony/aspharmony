using DataAccessLayer.Entities;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Utils.Exceptions;

namespace DataAccessLayer.Repositories {
    public interface ILibraryMembershipRepository {
        Task CreateAsync(LibraryMembership membership);
        IEnumerable<LibraryMembership> GetUserMemberships(string userId);
        Task<LibraryMembership?> GetMembershipAsync(int libraryId, string userId);
        Task<LibraryMembership?> GetMembershipAsync(int id);
        Task UpdateAsync(LibraryMembership membership);
        Task DeleteAsync(int id);
        IEnumerable<LibraryMembership> GetLibraryMembers(int libraryId);
        Task DeleteAsync(int libraryId, string userId);
    }

    public class LibraryMembershipRepository(ApplicationContext context,
                                        IBookLoanRepository bookLoanRepository) : ILibraryMembershipRepository {
        public async Task CreateAsync(LibraryMembership membership) {
            await context.LibraryMemberships.AddAsync(membership);
            await context.SaveChangesAsync();
        }

        public IEnumerable<LibraryMembership> GetUserMemberships(string userId) {
            return context.LibraryMemberships
                .Include(membership => membership.Library)
                .Include(membership => membership.User)
                .Where(membership => membership.UserId == userId);
        }

        public async Task<LibraryMembership?> GetMembershipAsync(int libraryId, string userId) {
            return await context.LibraryMemberships
                .Include(membership => membership.Library)
                .Include(membership => membership.User)
                .FirstOrDefaultAsync(m => m.LibraryId == libraryId && m.UserId == userId);
        }

        public async Task<LibraryMembership?> GetMembershipAsync(int id) {
            return await context.LibraryMemberships
                .Include(membership => membership.Library)
                .Include(membership => membership.User)
                .FirstOrDefaultAsync(membership => membership.Id == id);
        }

        public async Task UpdateAsync(LibraryMembership membership) {
            context.LibraryMemberships.Update(membership);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) {
            var transaction = context.Database.BeginTransaction();

            LibraryMembership? membership = await GetMembershipAsync(id);

            if (membership == null)
                throw new NotFoundException();

            IEnumerable<BookLoan> loans = bookLoanRepository.GetMemberLoans(id);

            if (loans.Any())
                context.BookLoans.RemoveRange(membership.BookLoans);

            context.LibraryMemberships.Remove(membership);
            await context.SaveChangesAsync();

            transaction.Commit();
        }


        public IEnumerable<LibraryMembership> GetLibraryMembers(int libraryId) {
            return context.LibraryMemberships
                .Include(membership => membership.User)
                .Where(membership => membership.LibraryId == libraryId);
        }

        public async Task DeleteAsync(int libraryId, string userId) {
            var transaction = context.Database.BeginTransaction();

            LibraryMembership? membership = await GetMembershipAsync(libraryId, userId);

            if (membership == null)
                throw new NotFoundException();

            IEnumerable<BookLoan> loans = bookLoanRepository.GetMemberLoans(membership.Id);
            
            if (loans.Any())
                context.BookLoans.RemoveRange(membership.BookLoans);
            
            context.LibraryMemberships.Remove(membership);
            await context.SaveChangesAsync();

            transaction.Commit();
        }
    }

}
