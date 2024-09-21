using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public interface IUserRepository {
        Task<User> GetByIdAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
    }

    public class UserRepository : IUserRepository {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager) {
            _userManager = userManager;
        }

        public async Task<User> GetByIdAsync(string id) {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<User> GetByEmailAsync(string email) {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userManager.Users.ToListAsync();
        }
    }
}
