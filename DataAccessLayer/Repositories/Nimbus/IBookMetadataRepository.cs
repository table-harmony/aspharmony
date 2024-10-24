using DataAccessLayer.Entities.Nimbus;

namespace DataAccessLayer.Repositories.Nimbus {
    public interface IBookMetadataRepository {
        Task<IEnumerable<BookMetadata>> GetAllAsync();
        Task<BookMetadata?> GetAsync(int bookId);
        Task UpdateAsync(BookMetadata metadata);
        Task CreateAsync(BookMetadata metadata);
        Task DeleteAsync(int bookId);
    }

}
