using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.Data.SqlClient;
using System.Data;
using Utils;

namespace DataAccessLayer.Repositories {
    public interface IBookChapterRepository {
        Task<IEnumerable<BookChapter>> GetChaptersAsync(int bookId);
        Task CreateAsync(BookChapter chapter);
        Task UpdateAsync(BookChapter chapter);
        Task DeleteAsync(int chapterId);
    }

    public class BookChapterRepository : IBookChapterRepository {
        private readonly AdoContext _context;

        public BookChapterRepository() {
            string connectionString = ConnectionStringBuilder.GenerateConnectionString("Nimbus.mdf");
            _context = new AdoContext(connectionString);
        }

        public async Task<IEnumerable<BookChapter>> GetChaptersAsync(int bookId) {
            string query = "GetBookChapters";
            var parameters = new[] { new SqlParameter("@BookId", bookId) };

            DataSet data = await _context.ExecuteQueryAsync(query, parameters, true);

            return data.Tables[0].Rows.Cast<DataRow>()
                .Select(MapToChapter)
                .ToList();
        }

        public async Task CreateAsync(BookChapter chapter) {
            string query = @"INSERT INTO BookChapters 
                             (BookId, [Index], Title, Content) 
                             VALUES (@BookId, @Index, @Title, @Content)";

            var parameters = new[] {
                new SqlParameter("@BookId", chapter.BookId),
                new SqlParameter("@Index", chapter.Index),
                new SqlParameter("@Title", chapter.Title),
                new SqlParameter("@Content", chapter.Content)
            };

            await _context.ExecuteQueryAsync(query, parameters);
        }

        public async Task UpdateAsync(BookChapter chapter) {
            string query = @"UPDATE BookChapters
                             SET [Index] = @Index, Title = @Title, Content = @Content
                             WHERE Id = @Id";

            var parameters = new[] {
                new SqlParameter("@Id", chapter.Id),
                new SqlParameter("@Index", chapter.Index),
                new SqlParameter("@Title", chapter.Title),
                new SqlParameter("@Content", chapter.Content)
            };

            await _context.ExecuteQueryAsync(query, parameters);
        }
        
        public async Task DeleteAsync(int chapterId) {
            string query = "DeleteChapter";
            var parameters = new[] {
                new SqlParameter("@BookId", chapterId)
            };

            await _context.ExecuteQueryAsync(query, parameters, true);
        }

        private static BookChapter MapToChapter(DataRow row) {
            return new BookChapter {
                Id = row.Field<int>("Id"),
                BookId = row.Field<int>("BookId"),
                Index = row.Field<int>("Index"),
                Title = row.Field<string>("Title") ?? "",
                Content = row.Field<string>("Content") ?? ""
            };
        }
    }
}
