using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services {
    public interface ISenderService {
        Task<IEnumerable<Sender>> GetAllAsync();
        Task<Sender?> GetAsync(int id);
    }

    public class SenderService(ISenderRepository senderRepository) : ISenderService {
        public async Task<IEnumerable<Sender>> GetAllAsync() {
            return await senderRepository.GetAllAsync();
        }

        public async Task<Sender?> GetAsync(int id) {
            return await senderRepository.GetAsync(id);
        }
    }
}
