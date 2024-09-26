﻿using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils.Exceptions;

namespace DataAccessLayer.Repositories {
    public interface INotificationRepository {
        Task<Notification> GetAsync(int id);
        Task<IEnumerable<Notification>> GetByUserAsync(string userId);
        Task CreateAsync(Notification notification);
        Task DeleteAsync(int id);
    }

    public class NotificationRepository : INotificationRepository {
        private readonly ApplicationContext _context;

        public NotificationRepository(ApplicationContext context) {
            _context = context;
        }

        public async Task<Notification> GetAsync(int id) {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task<IEnumerable<Notification>> GetByUserAsync(string userId) {
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
            Notification notification = await GetAsync(id);

            if (notification == null)
                throw new NotFoundException();

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }
    }
}