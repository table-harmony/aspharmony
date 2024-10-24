using DataAccessLayer.Entities.Nimbus;

namespace DataAccessLayer.Repositories.Nimbus {
    public interface IBookChapterRepository {
        Task<IEnumerable<BookChapter>> GetChaptersAsync(int bookId);
        Task CreateAsync(BookChapter chapter);
        Task UpdateAsync(BookChapter chapter);
        Task DeleteAsync(int bookId);
    }
}
