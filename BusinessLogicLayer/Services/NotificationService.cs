using BusinessLogicLayer.Services.Nimbus;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Microsoft.Extensions.Logging;
using Utils.Exceptions;
using Utils.Senders;

namespace BusinessLogicLayer.Services {
    public interface INotificationService {
        Task<Notification?> GetAsync(int id);
        Task<IEnumerable<Notification>> GetByUserAsync(string userId);
        Task<IEnumerable<Notification>> GetUnreadByUserAsync(string userId);
        Task CreateAsync(string userId, string message);
        Task DeleteAsync(int id);
        Task DeleteAsync(string userId);
        Task MarkAsReadAsync(int id);
        Task MarkAllAsReadAsync(string userId);
        Task<int> GetUnreadCountAsync(string userId);
        Task NotifyUserAsync(string userId, string message);
        Task MarkAsUnReadAsync(int id);
    }

    public class NotificationService(
        INotificationRepository notificationRepository,
        IUserService userService,
        ISenderFactory senderFactory,
        IUserSenderService userSenderService) : INotificationService {
        public async Task<IEnumerable<Notification>> GetByUserAsync(string userId) {
            return await notificationRepository.GetByUserAsync(userId);
        }

        public async Task<Notification?> GetAsync(int id) {
            return await notificationRepository.GetAsync(id);
        }

        public async Task CreateAsync(string userId, string message) {
            var notification = new Notification {
                UserId = userId,
                Message = message,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            await notificationRepository.CreateAsync(notification);
            await NotifyUserAsync(userId, message);
        }

        public async Task NotifyUserAsync(string userId, string message) {
            User? user = await userService.GetByIdAsync(userId);

            if (user == null)
                return;

            var senders = await userSenderService.GetByUserIdAsync(userId);

            foreach (var dbSender in senders) {
                try {
                    var sender = senderFactory.CreateSender((SenderType)Enum.Parse(typeof(SenderType), dbSender.Sender.Name.ToLower()));
                    string to = sender is EmailSender ? user.Email! : user.PhoneNumber!;

                    if (string.IsNullOrEmpty(to)) {
                        continue;
                    }

                    sender.Send(message, to);
                } catch {
                }
            }
        }

        public async Task DeleteAsync(int id) {
            var notification = await notificationRepository.GetAsync(id);
            if (notification != null)
                await notificationRepository.DeleteAsync(id);
        }

        public async Task DeleteAsync(string userId) {
            await notificationRepository.DeleteAllAsync(userId);
        }


        public async Task<IEnumerable<Notification>> GetUnreadByUserAsync(string userId) {
            return await notificationRepository.GetUnreadByUserAsync(userId);
        }

        public async Task MarkAsReadAsync(int id) {
            var notification = await notificationRepository.GetAsync(id);
            if (notification != null && !notification.IsRead) {
                notification.IsRead = true;
                await notificationRepository.UpdateAsync(notification);
            }
        }

        public async Task MarkAsUnReadAsync(int id) {
            var notification = await notificationRepository.GetAsync(id);
            if (notification != null && notification.IsRead) {
                notification.IsRead = false;
                await notificationRepository.UpdateAsync(notification);
            }
        }

        public async Task MarkAllAsReadAsync(string userId) {
            await notificationRepository.MarkAllAsReadAsync(userId);
        }

        public async Task<int> GetUnreadCountAsync(string userId) {
            return await notificationRepository.GetUnreadCountAsync(userId);
        }
    }
}