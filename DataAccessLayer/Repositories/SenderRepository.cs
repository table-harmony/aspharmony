using DataAccessLayer.Entities;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Utils.Exceptions;

namespace DataAccessLayer.Repositories {
    public interface ISenderRepository {
        Task<Sender?> GetAsync(int id);
        Task<IEnumerable<Sender>> GetAllAsync();
        Task CreateAsync(Sender Sender);
        Task UpdateAsync(Sender Sender);
        Task DeleteAsync(int id);
    }

    public class SenderRepository(ApplicationContext context) : ISenderRepository {
        public async Task<Sender?> GetAsync(int id) {
            return await context.Senders
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Sender>> GetAllAsync() {
            return await context.Senders.ToListAsync();
        }

        public async Task CreateAsync(Sender sender) {
            await context.Senders.AddAsync(sender);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Sender sender) {
            context.Senders.Update(sender);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) {
            Sender? sender = await GetAsync(id);

            if (sender == null)
                return;

            context.Senders.Remove(sender);
            await context.SaveChangesAsync();
        }
    }
}
