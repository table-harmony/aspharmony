using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services {
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetByUserIdAsync(string userId);
        Task<Notification> GetByIdAsync(int id);
        Task CreateAsync(string userId, string message);
        Task DeleteAsync(int id);
    }

    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(string userId)
        {
            return await _notificationRepository.GetByUserIdAsync(userId);
        }

        public async Task<Notification> GetByIdAsync(int id)
        {
            return await _notificationRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(string userId, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                CreatedAt = DateTime.UtcNow
            };
            await _notificationRepository.CreateAsync(notification);
        }

        public async Task DeleteAsync(int id)
        {
            await _notificationRepository.DeleteAsync(id);
        }
    }
}