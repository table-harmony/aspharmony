using Microsoft.AspNetCore.Identity;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Utils.Exceptions;

namespace BusinessLogicLayer.Services
{
    public interface IUserService
    {
        Task<User> GetByIdAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<IdentityResult> CreateAsync(string email, string password);
        Task<IdentityResult> UpdateAsync(User user);
        Task<IdentityResult> DeleteAsync(string id);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;

        public UserService(IUserRepository userRepository, UserManager<User> userManager) {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<User> GetByIdAsync(string id) {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> GetByEmailAsync(string email) {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<IEnumerable<User>> GetAllAsync() {
            return await _userRepository.GetAllAsync();
        }

        public async Task<IdentityResult> CreateAsync(string email, string password) {
            var user = new User { UserName = email, Email = email };
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> UpdateAsync(User user) {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteAsync(string id) {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                throw new NotFoundException();

            return await _userManager.DeleteAsync(user);
        }
    }
}
