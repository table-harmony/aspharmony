using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Utils.Exceptions;

namespace DataAccessLayer.Repositories {
    public interface IUserRepository {
        Task<User?> GetByIdAsync(string id);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(string id);
    }

    public class UserRepository(UserManager<User> userManager, ApplicationContext context) : IUserRepository {
        public async Task<User?> GetByIdAsync(string id) {
            return await userManager.FindByIdAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email) {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<IEnumerable<User>> GetAllAsync() {
            return await context.Users.ToListAsync();
        }

        public async Task CreateAsync(User user) {
            await userManager.UpdateAsync(user);
        }

        public async Task UpdateAsync(User user) {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id) {
            User? user = await GetByIdAsync(id);

            if (user == null)
                throw new NotFoundException();

            await userManager.DeleteAsync(user);
        }
    }
}