using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services {
    public interface IServerService {
        Task<IEnumerable<Server>> GetAllAsync();
        Task<Server?> GetAsync(int id);
        Task<Server?> GetAsync(string name);
    }

    public class ServerService(IServerRepository repository) : IServerService {
        public async Task<IEnumerable<Server>> GetAllAsync() {
            return await repository.GetAllAsync();
        }

        public async Task<Server?> GetAsync(int id) {
            return await repository.GetAsync(id);
        }

        public async Task<Server?> GetAsync(string name) {
            return await repository.GetAsync(name);
        }
    }
}
