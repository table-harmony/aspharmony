using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services {
    public interface IBookService {
        Task<Book> GetByIdAsync(int id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task CreateAsync(string title, string description, string content, int authorId);
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);
    }

    public class BookService : IBookService {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository) {
            _bookRepository = bookRepository;
        }

        public async Task<Book> GetByIdAsync(int id) {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Book>> GetAllAsync() {
            return await _bookRepository.GetAllAsync();
        }

        public async Task CreateAsync(string title, string description, string content, int authorId) {
            Book book = new() {
                Title = title,
                Description = description,
                Content = content,
                AuthorId = authorId
            };

            await _bookRepository.CreateAsync(book);
        }

        public async Task UpdateAsync(Book book) {
            await _bookRepository.UpdateAsync(book);
        }

        public async Task DeleteAsync(int id) {
            await _bookRepository.DeleteAsync(id);
        }
    }
}
