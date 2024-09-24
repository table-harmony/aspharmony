using Microsoft.AspNetCore.Identity;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Utils.Exceptions;
using System.Security.Claims;

namespace BusinessLogicLayer.Services
{
    public interface IUserService {
        Task<User> GetByIdAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<IdentityResult> CreateAsync(string email, string password);
        Task UpdateAsync(User user);
        Task DeleteAsync(string id);
    }

    public class UserService : IUserService {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) {
            _userRepository = userRepository;
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
            return await _userRepository.CreateAsync(user);
        }

        public async Task UpdateAsync(User user) {
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(string id) {
            await _userRepository.DeleteAsync(id);
        }
    }
}
