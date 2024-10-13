using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services {
    public interface IBookMetadataService {
        Task<IEnumerable<BookMetadata>> GetAllAsync();
        Task<BookMetadata?> GetAsync(int bookId);
        Task UpdateAsync(BookMetadata metadata);
        Task CreateAsync(BookMetadata metadata);
        Task DeleteAsync(int bookId);
    }

    public class BookMetadataService(IBookMetadataRepository repository) : IBookMetadataService {
        public async Task<IEnumerable<BookMetadata>> GetAllAsync() { 
            return await repository.GetAllAsync();
        }

        public async Task UpdateAsync(BookMetadata metadata) {
            await repository.UpdateAsync(metadata);
        }
        
        public async Task CreateAsync(BookMetadata metadata) {
            await repository.CreateAsync(metadata);
        }

        public async Task DeleteAsync(int bookId) {
            await repository.DeleteAsync(bookId);
        }

        public async Task<BookMetadata?> GetAsync(int bookId) {
            return await repository.GetAsync(bookId);
        }
    }
}
