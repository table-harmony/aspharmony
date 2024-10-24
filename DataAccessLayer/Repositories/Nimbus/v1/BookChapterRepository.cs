using DataAccessLayer.Data;
using DataAccessLayer.Entities.Nimbus;
using Microsoft.Data.SqlClient;
using MongoDB.Bson;
using System.Data;
using Utils;

namespace DataAccessLayer.Repositories.Nimbus.v1 {
    public class BookChapterRepository : IBookChapterRepository {
        private readonly AdoContext _context = new(PathManager.GenerateConnectionString("Nimbus.mdf"));

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
                             SET Title = @Title, Content = @Content
                             WHERE BookId = @Id AND [Index] = @Index";

            var parameters = new[] {
                new SqlParameter("@Index", chapter.Index),
                new SqlParameter("@Title", chapter.Title),
                new SqlParameter("@Content", chapter.Content)
            };

            await _context.ExecuteQueryAsync(query, parameters);
        }

        public async Task DeleteAsync(int bookId) {
            string query = "DeleteChapter";
            var parameters = new[] {
                new SqlParameter("@BookId", bookId)
            };

            await _context.ExecuteQueryAsync(query, parameters, true);
        }

        private static BookChapter MapToChapter(DataRow row) {
            return new BookChapter {
                BookId = row.Field<int>("BookId"),
                Index = row.Field<int>("Index"),
                Title = row.Field<string>("Title") ?? "",
                Content = row.Field<string>("Content") ?? ""
            };
        }
    }
}
