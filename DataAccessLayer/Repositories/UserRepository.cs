using DataAccessLayer.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Utils.Exceptions;

namespace DataAccessLayer.Repositories
{
    public interface IUserRepository {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }

    public class UserRepository : IUserRepository {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context) {
            _context = context;
        }

        public async Task<User> GetByIdAsync(int id) {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetByEmailAsync(string email) {
            return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllAsync() {
            return await _context.Users.ToListAsync();
        }

        public async Task CreateAsync(User user) {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user) {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) {
            User user = await GetByIdAsync(id);

            if (user == null)
                throw new NotFoundException();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
