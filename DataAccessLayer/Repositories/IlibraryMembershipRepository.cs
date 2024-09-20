using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories {
    public interface ILibraryMembershipRepository {
        Task<LibraryMembership> GetById(int id);
        Task<LibraryMembership> GetByUser(int userId);
        Task CreateAsync(LibraryMembership membership);
        Task UpdateAsync(LibraryMembership membership);
        Task DeleteAsync(int id);
    }
}
