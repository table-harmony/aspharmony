using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories {
    public interface IBookMetadataRepository {
        Task<IEnumerable<BookMetadata>> GetAllAsync();
        Task<BookMetadata?> GetAsync(int bookId);
        Task UpdateAsync(BookMetadata metadata);
        Task CreateAsync(BookMetadata metadata);
        Task DeleteAsync(int bookId);
    }

    public class BookMetadataRepository(ApplicationContext context) : IBookMetadataRepository {

        public async Task<IEnumerable<BookMetadata>> GetAllAsync() {
            return await context.BookMetadatas.ToListAsync();
        }

        public async Task<BookMetadata?> GetAsync(int bookId) {
            return await context.BookMetadatas
                .FirstOrDefaultAsync(m => m.BookId == bookId);
        }

        public async Task CreateAsync(BookMetadata metadata) {
            await context.BookMetadatas.AddAsync(metadata);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BookMetadata metadata) {
            context.BookMetadatas.Update(metadata);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int bookId) {
            BookMetadata? metadata = await GetAsync(bookId);

            if (metadata != null) {
                context.BookMetadatas.Remove(metadata);
                await context.SaveChangesAsync();
            }
        }
    }
}
