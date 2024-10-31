using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Microsoft.Extensions.Configuration;
using Utils.Senders;

namespace BusinessLogicLayer.Services
{
    public interface INotificationService {
        Task<Notification?> GetAsync(int id);
        Task<IEnumerable<Notification>> GetByUserAsync(string userId);
        Task CreateAsync(string userId, string message);
        Task DeleteAsync(int id);
    }

    public class NotificationService(INotificationRepository notificationRepository, 
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
            User? user = await userService.GetByIdAsync(userId);

            if (user == null)
                return;

            Notification notification = new() {
                UserId = userId,
                Message = message,
                CreatedAt = DateTime.UtcNow
            };

            await notificationRepository.CreateAsync(notification);

            var senders = await userSenderService.GetByUserIdAsync(user.Id);

            foreach (UserSender dbSender in senders) {
                SenderType type = (SenderType)Enum.Parse(typeof(SenderType), dbSender.Sender.Name.ToLower());
                var sender = senderFactory.CreateSender(type);

                string to = user.PhoneNumber!;
                if (sender is EmailSender)
                    to = user.Email!;

                sender.Send(message, to);
            }
        }

        public async Task DeleteAsync(int id) {
            var notification = await notificationRepository.GetAsync(id);
            if (notification != null) {
                await notificationRepository.DeleteAsync(id);
            }
        }
    }
}