using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using DataAccessLayer.Entities.Nimbus;

namespace DataAccessLayer.Repositories.Nimbus.v2 {
    public class BookMetadataRepository : IBookMetadataRepository {
        private readonly IMongoCollection<BookMetadata> collection;

        public BookMetadataRepository(IConfiguration configuration) {
            var client = new MongoClient(configuration["Mongo:DatabaseUrl"]);
            var database = client.GetDatabase(configuration["Mongo:DatabaseName"]);

            collection = database.GetCollection<BookMetadata>("Metadata");
        }

        public async Task<IEnumerable<BookMetadata>> GetAllAsync() {
            return await collection.Find(_ => true).ToListAsync();
        }

        public async Task<BookMetadata?> GetAsync(int bookId) {
            return await collection.Find(m => m.BookId == bookId).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(BookMetadata metadata) {
            await collection.InsertOneAsync(metadata);
        }

        public async Task UpdateAsync(BookMetadata metadata) {
            await collection.ReplaceOneAsync(m => m.BookId == metadata.BookId, metadata);
        }

        public async Task DeleteAsync(int bookId) {
            await collection.DeleteOneAsync(m => m.BookId == bookId);
        }
    }

}