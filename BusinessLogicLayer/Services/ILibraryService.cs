using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Services {
    public interface ILibraryService {
        Task CreateAsync(int userId, string name);
        Task UpdateAsync(string name);
        Task DeleteAsync(int id);
        IEnumerable<Book> GetBooks(int id);
        IEnumerable<LibraryMembership> GetMemberships(int id);
        Task<Library> GetByIdAsync(int id);
        Task<IEnumerable<Library>> GetAllAsync();
    }
}
