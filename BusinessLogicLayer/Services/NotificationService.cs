using BusinessLogicLayer.Servers.Books;
using BusinessLogicLayer.Services.Nimbus;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using ForeignBooksServiceReference;
using LocalBooksServiceReference;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Utils.Exceptions;
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
            senders.ToList()
                .ForEach(dbSender => {
                    var sender = senderFactory.CreateSender(dbSender.Sender.Name);

                    string to = user.PhoneNumber!;
                    if (sender is EmailSender)
                        to = user.Email!;

                    sender.Send(message, to);
                });
        }

        public async Task DeleteAsync(int id) {
            var notification = await notificationRepository.GetAsync(id);
            if (notification != null) {
                await notificationRepository.DeleteAsync(id);
            }
        }
    }

    public interface ISenderFactory {
        public ISender CreateSender(string senderName);
    }

    public class SenderFactory(IConfiguration configuration) : ISenderFactory {
        public ISender CreateSender(string senderName) {
            return senderName switch {
                "SMS" => new SMSSender(configuration),
                "WhatsApp" => new WhatsAppSender(configuration),
                "Email" => new EmailSender(configuration),
                _ => throw new InvalidOperationException($"Invalid sender: {senderName}"),
            };
        }
    }
}