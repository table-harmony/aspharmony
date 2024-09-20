using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services {
    public class UserService : IUserService {
        private readonly IUserRepository _userRepository;
        
        public UserService(IUserRepository userRepository) {
            _userRepository = userRepository;
        }

        public async Task<User> GetByIdAsync(int id) {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllAsync() {
            return await _userRepository.GetAllAsync();
        }

        public async Task CreateAsync(User user) {
            await _userRepository.CreateAsync(user);
        }

        public async Task UpdateAsync(User user) {
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(int id) {
            await _userRepository.DeleteAsync(id);
        }

    }
}
