using DataAccessLayer.Entities;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Utils.Exceptions;

namespace DataAccessLayer.Repositories {
    public interface IUserSenderRepository {
        Task<UserSender?> GetAsync(int id);
        Task<IEnumerable<UserSender>> GetAllAsync();
        Task CreateAsync(UserSender Sender);
        Task UpdateAsync(UserSender Sender);
        Task DeleteAsync(int id);
        Task<IEnumerable<UserSender>> GetByUserIdAsync(string userId);
        Task DeleteAsync(string userId, int senderId);
    }

    public class UserSenderRepository(ApplicationContext context) : IUserSenderRepository {
        public async Task<UserSender?> GetAsync(int id) {
            return await context.UserSenders
                .Include(userSender => userSender.User)
                .Include(userSender => userSender.Sender)
                .FirstOrDefaultAsync(us => us.Id == id);
        }

        public async Task<IEnumerable<UserSender>> GetAllAsync() {
            return await context.UserSenders.ToListAsync();
        }

        public async Task CreateAsync(UserSender userSender) {
            await context.UserSenders.AddAsync(userSender);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserSender userSender) {
            context.UserSenders.Update(userSender);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) {
            UserSender? userSender = await GetAsync(id);

            if (userSender == null)
                return;

            context.UserSenders.Remove(userSender);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserSender>> GetByUserIdAsync(string userId) {
            return await context.UserSenders
                .Where(us => us.UserId == userId)
                .Include(userSender => userSender.Sender)
                .ToListAsync();
        }

        public async Task DeleteAsync(string userId, int senderId) {
            var userSender = await context.UserSenders
                .FirstOrDefaultAsync(us => us.UserId == userId && us.SenderId == senderId);

            if (userSender != null) {
                context.UserSenders.Remove(userSender);
                await context.SaveChangesAsync();
            }
        }
    }
}
