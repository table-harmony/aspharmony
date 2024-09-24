using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories {
    public interface INotificationRepository {
        Task<Notification> GetByIdAsync(int id);
        Task<IEnumerable<Notification>> GetByUserIdAsync(string userId);
        Task CreateAsync(Notification notification);
        Task DeleteAsync(int id);
    }

    public class NotificationRepository : INotificationRepository {
        private readonly ApplicationContext _context;

        public NotificationRepository(ApplicationContext context) {
            _context = context;
        }

        public async Task<Notification> GetByIdAsync(int id) {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(string userId) {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task CreateAsync(Notification notification) {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null) {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }
    }
}