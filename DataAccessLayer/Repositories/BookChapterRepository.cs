using DataAccessLayer.Data;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories {
    public interface IBookChapterRepository {
        IEnumerable<BookChapter> GetChapters(int bookId);
        Task CreateAsync(BookChapter chapter);
        Task UpdateAsync(BookChapter chapter);
        Task DeleteAsync(int bookId);
    }

    public class BookChapterRepository(ApplicationContext context) : IBookChapterRepository {

        public IEnumerable<BookChapter> GetChapters(int bookId) {
            return context.BookChapters.Where(c => c.BookId == bookId);
        }

        public async Task CreateAsync(BookChapter chapter) {
            await context.BookChapters.AddAsync(chapter);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BookChapter chapter) {
            context.BookChapters.Update(chapter);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int bookId) {
            var chapters = GetChapters(bookId);

            if (chapters.Any()) {
                context.BookChapters.RemoveRange(chapters);
                await context.SaveChangesAsync();
            }
        }
    }
}
