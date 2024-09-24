using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Utils.Exceptions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services {
    public interface IBookService {
        Task<Book> GetBookAsync(int id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task CreateAsync(string title, string description, string content, string authorId);
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);
    }

    public class BookService : IBookService {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository) {
            _bookRepository = bookRepository;
        }

        public async Task<Book> GetBookAsync(int id) {
            return await _bookRepository.GetBookAsync(id);
        }

        public async Task<IEnumerable<Book>> GetAllAsync() {
            return await _bookRepository.GetAllAsync();
        }

        public async Task CreateAsync(string title, string description, string content, string authorId) {
            Book book = new Book {
                Title = title,
                Description = description,
                Content = content,
                AuthorId = authorId
            };

            await _bookRepository.CreateAsync(book);
        }

        public async Task UpdateAsync(Book book) {
            var existingBook = await _bookRepository.GetBookAsync(book.Id);
            if (existingBook == null)
                throw new NotFoundException();
           
            existingBook.Title = book.Title;
            existingBook.Description = book.Description;
            existingBook.Content = book.Content;

            await _bookRepository.UpdateAsync(existingBook);
        }

        public async Task DeleteAsync(int id) { 
            await _bookRepository.DeleteAsync(id);
        }
    }
}
