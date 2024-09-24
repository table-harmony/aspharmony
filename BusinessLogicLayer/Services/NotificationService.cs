using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public interface INotificationService {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId);
        Task CreateNotificationAsync(string userId, string message);
        Task DeleteNotificationAsync(int id);
    }

    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository) {
            _notificationRepository = notificationRepository;
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId) {
            return await _notificationRepository.GetByUserIdAsync(userId);
        }

        public async Task CreateNotificationAsync(string userId, string message) {
            var notification = new Notification {
                UserId = userId,
                Message = message,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationRepository.CreateAsync(notification);
        }

        public async Task DeleteNotificationAsync(int id) {
            await _notificationRepository.DeleteAsync(id);
        }
    }
}