using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services
{
    public interface INotificationService {
        Task<Notification?> GetAsync(int id);
        Task<IEnumerable<Notification>> GetByUserAsync(string userId);
        Task CreateAsync(string userId, string message);
        Task DeleteAsync(int id);
    }

    public class NotificationService(INotificationRepository notificationRepository) : INotificationService {
        public async Task<IEnumerable<Notification>> GetByUserAsync(string userId) {
            return await notificationRepository.GetByUserAsync(userId);
        }

        public async Task<Notification?> GetAsync(int id) {
            return await notificationRepository.GetAsync(id);
        }
        
        public async Task CreateAsync(string userId, string message) {
            Notification notification = new() {
                UserId = userId,
                Message = message,
                CreatedAt = DateTime.UtcNow
            };

            await notificationRepository.CreateAsync(notification);
        }

        public async Task DeleteAsync(int id) {
            var notification = await notificationRepository.GetAsync(id);
            if (notification != null) {
                await notificationRepository.DeleteAsync(id);
            }
        }
    }
}