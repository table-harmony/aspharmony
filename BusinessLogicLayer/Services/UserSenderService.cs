using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services {
    public interface IUserSenderService {
        Task<IEnumerable<UserSender>> GetByUserIdAsync(string userId);
        Task CreateAsync(UserSender userSender);
        Task DeleteAsync(string userId, int senderId);
    }

    public class UserSenderService(IUserSenderRepository userSenderRepository) : IUserSenderService {
        public async Task<IEnumerable<UserSender>> GetByUserIdAsync(string userId) {
            return await userSenderRepository.GetByUserIdAsync(userId);
        }

        public async Task CreateAsync(UserSender userSender) {
            await userSenderRepository.CreateAsync(userSender);
        }

        public async Task DeleteAsync(string userId, int senderId) {
            await userSenderRepository.DeleteAsync(userId, senderId);
        }
    }
}
