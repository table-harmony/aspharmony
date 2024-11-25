using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Utils.Exceptions;

namespace DataAccessLayer.Repositories {
    public interface IUserRepository {
        Task<User?> GetByIdAsync(string id);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(string id);
    }

    public class UserRepository(ApplicationContext context) : IUserRepository {
        public async Task<User?> GetByIdAsync(string id) {
            return await context.Users.FindAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email) {
            return await context.Users.Where(user => user.Email == email).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync() {
            return await context.Users.ToListAsync();
        }

        public async Task CreateAsync(User user) {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user) {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id) {
            using var transaction = await context.Database.BeginTransactionAsync();
            try {
                var user = await context.Users
                    .Include(u => u.Books)
                        .ThenInclude(b => b.LibraryBooks)
                            .ThenInclude(lb => lb.Loans)
                    .Include(u => u.Memberships)
                        .ThenInclude(m => m.BookLoans)
                    .FirstOrDefaultAsync(u => u.Id == id)
                    ?? throw new NotFoundException();

                var bookLoans = user.Books
                    .SelectMany(b => b.LibraryBooks)
                    .SelectMany(lb => lb.Loans)
                    .Concat(user.Memberships.SelectMany(m => m.BookLoans));
                context.BookLoans.RemoveRange(bookLoans);

                var libraryBooks = user.Books.SelectMany(b => b.LibraryBooks);
                context.LibraryBooks.RemoveRange(libraryBooks);

                context.Books.RemoveRange(user.Books);

                context.Users.Remove(user);

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            } catch {
                await transaction.RollbackAsync();
                throw;
            }

        }
    }
}