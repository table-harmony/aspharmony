using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces {
    public interface IUserRepository {
        Task<User> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}
