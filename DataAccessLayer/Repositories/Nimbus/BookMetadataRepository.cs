using DataAccessLayer.Data;
using DataAccessLayer.Entities.Nimbus;
using Microsoft.Data.SqlClient;
using System.Data;
using Utils;

namespace DataAccessLayer.Repositories.Nimbus
{
    public interface IBookMetadataRepository {
        Task<IEnumerable<BookMetadata>> GetAllAsync();
        Task<BookMetadata?> GetAsync(int bookId);
        Task UpdateAsync(BookMetadata metadata);
        Task CreateAsync(BookMetadata metadata);
        Task DeleteAsync(int bookId);
    }

    public class BookMetadataRepository : IBookMetadataRepository {
        private readonly AdoContext _context;

        public BookMetadataRepository() {
            string connectionString = PathManager.GenerateConnectionString("Nimbus.mdf");
            _context = new AdoContext(connectionString);
        }

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
                                (BookId, Title, Description, ImageUrl) 
                                VALUES (@BookId, @Title, @Description, @ImageUrl)";

            var parameters = new[] {
                new SqlParameter("@BookId", metadata.BookId),
                new SqlParameter("@Title", metadata.Title),
                new SqlParameter("@Description", metadata.Description),
                new SqlParameter("@ImageUrl", metadata.ImageUrl),
            };

            await _context.ExecuteQueryAsync(query, parameters);
        }

        public async Task UpdateAsync(BookMetadata metadata) {
            string query = @"UPDATE BooksMetadata
                SET BookId = @BookId, Title = @Title, Description = @Description, @ImageUrl = ImageUrl
                WHERE Id = @Id";

            var parameters = new[] {
                new SqlParameter("@Id", metadata.Id),
                new SqlParameter("@BookId", metadata.BookId),
                new SqlParameter("@Title", metadata.Title),
                new SqlParameter("@Description", metadata.Description),
                new SqlParameter("ImageUrl", metadata.ImageUrl),
            };

            await _context.ExecuteQueryAsync(query, parameters);
        }

        public async Task DeleteAsync(int bookId) {
            string query = "DELETE FROM BooksMetadata WHERE BookId = @BookId";
            var parameters = new[] {
                new SqlParameter("@BookId", bookId),
            };

            await _context.ExecuteQueryAsync(query, parameters);
        }

        private static BookMetadata MapToMetadata(DataRow row) {
            return new BookMetadata() {
                Id = row.Field<int>("Id"),
                BookId = row.Field<int>("BookId"),
                Title = row.Field<string>("Title") ?? "",
                Description = row.Field<string>("Description") ?? "",
                ImageUrl = row.Field<string>("ImageUrl") ?? ""
            };
        }
    }
}
