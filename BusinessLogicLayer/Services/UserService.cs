using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services {
    public interface IUserService {
        Task<User?> GetByIdAsync(string id);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task CreateAsync(string email, string password);
        Task UpdateAsync(User user);
        Task DeleteAsync(string id);
    }

    public class UserService(IUserRepository userRepository) : IUserService {
        public async Task<User?> GetByIdAsync(string id) {
            return await userRepository.GetByIdAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email) {
            return await userRepository.GetByEmailAsync(email);
        }

        public async Task<IEnumerable<User>> GetAllAsync() {
            return await userRepository.GetAllAsync();
        }

        public async Task CreateAsync(string email, string password) {
            var user = new User { UserName = email, Email = email };
            await userRepository.CreateAsync(user);
        }

        public async Task UpdateAsync(User user) {
            await userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(string id) {
            await userRepository.DeleteAsync(id);
        }
    }
}