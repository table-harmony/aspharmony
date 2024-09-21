using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services {
    public interface IBookService {
        Task<Book> GetByIdAsync(int id);
        Task CreateAsync();
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);
    }

    public class BookService : IBookService {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository) {
            _bookRepository = bookRepository;
        }

        public Task CreateAsync() {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id) {
            await _bookRepository.DeleteAsync(id);
        }

        public async Task<Book> GetByIdAsync(int id) {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Book book) {
            throw new NotImplementedException();

        }
    }
}
