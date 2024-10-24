using DataAccessLayer.Entities.Nimbus;
using DataAccessLayer.Repositories.Nimbus;

namespace BusinessLogicLayer.Services.Nimbus {
    public interface IBookChapterService {
        Task<IEnumerable<BookChapter>> GetChaptersAsync(int bookId);
        Task CreateAsync(BookChapter chapter);
        Task UpdateAsync(BookChapter chapter);
        Task DeleteAsync(int bookId);
    }

    public class BookChapterService(IBookChapterRepository repository) : IBookChapterService {
        public async Task CreateAsync(BookChapter chapter) {
            await repository.CreateAsync(chapter);
        }

        public async Task UpdateAsync(BookChapter chapter) {
            await repository.UpdateAsync(chapter);
        }

        public async Task DeleteAsync(int bookId) {
            await repository.DeleteAsync(bookId);
        }

        public async Task<IEnumerable<BookChapter>> GetChaptersAsync(int bookId) {
            return await repository.GetChaptersAsync(bookId);
        }
    }
}
