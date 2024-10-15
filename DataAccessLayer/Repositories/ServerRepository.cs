using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories {
    public interface IServerRepository {
        Task<IEnumerable<Server>> GetAllAsync();
        Task<Server?> GetAsync(int id);
        Task<Server?> GetAsync(string name);
    }

    public class ServerRepository(ApplicationContext context) : IServerRepository {
        public async Task<IEnumerable<Server>> GetAllAsync() {
            return await context.Servers
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Server?> GetAsync(int id) {
            return await context.Servers
                .AsNoTracking()
                .FirstOrDefaultAsync(server => server.Id == id);
        }

        public async Task<Server?> GetAsync(string name) {
            return await context.Servers
                .AsNoTracking()
                .FirstOrDefaultAsync(server => server.Name == name);
        }
    }
}
