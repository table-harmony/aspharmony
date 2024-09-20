using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories {
    public interface ILibraryRepository {
        IEnumerable<Book> GetBooks(int id);
        IEnumerable<LibraryMembership> GetMemberships(int id);
        Task<Library> GetByIdAsync(int id);
        Task<IEnumerable<Library>> GetAllAsync();
        Task CreateAsync(Library library);
        Task UpdateAsync(Library library);
        Task DeleteAsync(int id);
    }
}
