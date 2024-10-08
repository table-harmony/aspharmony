using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Utils.Exceptions;

namespace DataAccessLayer.Repositories {
    public interface INotificationRepository {
        Task<Notification?> GetAsync(int id);
        Task<IEnumerable<Notification>> GetByUserAsync(string userId);
        Task CreateAsync(Notification notification);
        Task DeleteAsync(int id);
    }

    public class NotificationRepository(ApplicationContext context) : INotificationRepository {
        public async Task<Notification?> GetAsync(int id) {
            return await context.Notifications.FindAsync(id);
        }

        public async Task<IEnumerable<Notification>> GetByUserAsync(string userId) {
            return await context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task CreateAsync(Notification notification) {
            await context.Notifications.AddAsync(notification);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) {
            Notification? notification = await GetAsync(id);

            if (notification == null)
                throw new NotFoundException();

            context.Notifications.Remove(notification);
            await context.SaveChangesAsync();
        }
    }
}