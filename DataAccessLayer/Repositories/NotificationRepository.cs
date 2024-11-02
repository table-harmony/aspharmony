using DataAccessLayer.Entities;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories {
    public interface INotificationRepository {
        Task<Notification?> GetAsync(int id);
        Task<IEnumerable<Notification>> GetByUserAsync(string userId);
        Task<IEnumerable<Notification>> GetUnreadByUserAsync(string userId);
        Task CreateAsync(Notification notification);
        Task UpdateAsync(Notification notification);
        Task DeleteAsync(int id);
        Task MarkAllAsReadAsync(string userId);
        Task<int> GetUnreadCountAsync(string userId);
        Task DeleteAllAsync(string userId);
    }

    public class NotificationRepository(ApplicationContext context) : INotificationRepository {
        public async Task<Notification?> GetAsync(int id)  {
            return await context.Notifications.FindAsync(id);
        }

        public async Task<IEnumerable<Notification>> GetByUserAsync(string userId) {
            return await context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetUnreadByUserAsync(string userId) {
            return await context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task CreateAsync(Notification notification) {
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Notification notification) {
            context.Notifications.Update(notification);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) {
            var notification = await context.Notifications.FindAsync(id);
            if (notification != null) {
                context.Notifications.Remove(notification);
                await context.SaveChangesAsync();
            }
        }

        public async Task MarkAllAsReadAsync(string userId) {
            await context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true));
        }

        public async Task<int> GetUnreadCountAsync(string userId) {
            return await context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task DeleteAllAsync(string userId) {
            var notifications = await context.Notifications
                .Where(n => n.UserId == userId)
                .ToListAsync();
            
            context.Notifications.RemoveRange(notifications);
            await context.SaveChangesAsync();
        }
    }
} 