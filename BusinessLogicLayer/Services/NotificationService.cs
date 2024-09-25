using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services
{
    public interface INotificationService {
        Task<Notification> GetAsync(int id);
        Task<IEnumerable<Notification>> GetByUserAsync(string userId);
        Task CreateAsync(string userId, string message);
        Task DeleteAsync(int id);
    }

    public class NotificationService : INotificationService {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository) {
            _notificationRepository = notificationRepository;
        }

        public async Task<IEnumerable<Notification>> GetByUserAsync(string userId) {
            return await _notificationRepository.GetByUserAsync(userId);
        }

        public async Task<Notification> GetAsync(int id) {
            return await _notificationRepository.GetAsync(id);
        }

        public async Task CreateAsync(string userId, string message) {
            Notification notification = new() {
                UserId = userId,
                Message = message,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationRepository.CreateAsync(notification);
        }

        public async Task DeleteAsync(int id) {
            await _notificationRepository.DeleteAsync(id);
        }
    }
}