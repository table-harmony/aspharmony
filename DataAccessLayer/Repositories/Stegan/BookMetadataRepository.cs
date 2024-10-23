using DataAccessLayer.Data;
using DataAccessLayer.Entities.Stegan;
using Microsoft.Data.SqlClient;
using System.Data;
using Utils;

namespace DataAccessLayer.Repositories.Stegan {
    public interface IBookMetadataRepository {
        Task<IEnumerable<BookMetadata>> GetAllAsync();
        Task<BookMetadata?> GetAsync(int bookId);
        Task UpdateAsync(BookMetadata metadata);
        Task CreateAsync(BookMetadata metadata);
        Task DeleteAsync(int bookId);
    }

    public class BookMetadataRepository : IBookMetadataRepository {
        private readonly AdoContext _context = new(PathManager.GenerateConnectionString("Stegan.mdf"));

        public async Task<IEnumerable<BookMetadata>> GetAllAsync() {
            string query = "GetAllBooksMetadata";

            DataSet data = await _context.ExecuteQueryAsync(query, isStoredProcedure: true);

            return data.Tables[0].Rows.Cast<DataRow>()
                .Select(MapToMetadata)
                .ToList();
        }

        public async Task<BookMetadata?> GetAsync(int bookId) {
            string query = "GetBookMetadata";
            var parameters = new[] { new SqlParameter("@BookId", bookId) };

            DataSet data = await _context.ExecuteQueryAsync(query, parameters, true);

            return data.Tables[0].Rows.Cast<DataRow>()
                .Select(MapToMetadata)
                .FirstOrDefault();
        }

        public async Task CreateAsync(BookMetadata metadata) {
            string query = @"INSERT INTO BooksMetadata 
                                (BookId, Url) 
                                VALUES (@BookId, @Url)";

            var parameters = new[] {
                new SqlParameter("@BookId", metadata.BookId),
                new SqlParameter("@Url", metadata.Url),
            };

            await _context.ExecuteQueryAsync(query, parameters);
        }

        public async Task UpdateAsync(BookMetadata metadata) {
            string query = @"UPDATE BooksMetadata
                SET Url = @Url
                WHERE BookId = @BookId";

            var parameters = new[] {
                new SqlParameter("@BookId", metadata.BookId),
                new SqlParameter("@Url", metadata.Url),
            };

            await _context.ExecuteQueryAsync(query, parameters);
        }

        public async Task DeleteAsync(int bookId) {
            string query = "DeleteBookMetadata";
            var parameters = new[] {
                new SqlParameter("@BookId", bookId),
            };

            await _context.ExecuteQueryAsync(query, parameters, true);
        }

        private static BookMetadata MapToMetadata(DataRow row) {
            return new BookMetadata() {
                BookId = row.Field<int>("BookId"),
                Url = row.Field<string>("Url") ?? ""
            };
        }
    }
}
