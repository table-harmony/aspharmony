using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Utils.Exceptions;

namespace DataAccessLayer.Repositories {
    public interface IUserRepository {
        Task<User> GetByIdAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<IdentityResult> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(string id);
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

        public async Task<IEnumerable<User>> GetAllAsync() {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<IdentityResult> CreateAsync(User user) {
            return await _userManager.CreateAsync(user);
        }

        public async Task UpdateAsync(User user) {
            await _userManager.UpdateAsync(user);
        }

        public async Task DeleteAsync(string id) {
            User user = await GetByIdAsync(id);

            if (user == null)
                throw new NotFoundException();

            await _userManager.DeleteAsync(user);
        }
    }
}