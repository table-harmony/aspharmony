using MongoDB.Driver;
using DataAccessLayer.Entities.Nimbus;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer.Repositories.Nimbus.v2 {

    public class BookChapterRepository : IBookChapterRepository {
        private readonly IMongoCollection<BookChapter> collection;

        public BookChapterRepository(IConfiguration configuration) {
            var client = new MongoClient(configuration["Mongo:DatabaseUrl"]);
            var database = client.GetDatabase(configuration["Mongo:DatabaseName"]);

            collection = database.GetCollection<BookChapter>("Chapters");
        }

        public async Task<IEnumerable<BookChapter>> GetChaptersAsync(int bookId) {
            return await collection
                .Find(c => c.BookId == bookId)
                .ToListAsync();
        }

        public async Task<BookChapter> GetChapterAsync(int bookId, int chapterIndex) {
            return await collection
                .Find(c => c.BookId == bookId && c.Index == chapterIndex)
                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(BookChapter chapter) {
            await collection.InsertOneAsync(chapter);
        }

        public async Task UpdateAsync(BookChapter chapter) {
            await collection.ReplaceOneAsync(c => c.Id == chapter.Id, chapter);
        }

        public async Task DeleteAsync(int bookId) {
            await collection.DeleteOneAsync(c => c.BookId == bookId);
        }

    }
}