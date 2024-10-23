using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services
{
    public interface ISenderService {
        Task<IEnumerable<Sender>> GetAllAsync();
    }

    public class SenderService(ISenderRepository senderRepository) : ISenderService {
        public async Task<IEnumerable<Sender>> GetAllAsync() {
            return await senderRepository.GetAllAsync();
        }
    }
}
