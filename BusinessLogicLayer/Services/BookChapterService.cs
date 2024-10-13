using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services {
    public interface IBookChapterService {
        IEnumerable<BookChapter> GetChapters(int bookId);
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

        public IEnumerable<BookChapter> GetChapters(int bookId) {
            return repository.GetChapters(bookId);
        }
    }
}
