using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Utils.Exceptions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services {
    public interface IBookService {
        Task<Book> GetByIdAsync(int id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task CreateAsync(string title, string description, string content, string authorId);
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);
    }

    public class BookService : IBookService {
        private readonly IBookRepository _bookRepository;
        private readonly INotificationService _notificationService;

        public event BookEventHandler? BookAdded;
        public event BookEventHandler? BookEdited;
        public event BookEventHandler? BookDeleted;

        public BookService(IBookRepository bookRepository, INotificationService notificationService) {
            _bookRepository = bookRepository;
            _notificationService = notificationService;
        }

        public async Task<Book> GetByIdAsync(int id) {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Book>> GetAllAsync() {
            return await _bookRepository.GetAllAsync();
        }

        public async Task CreateAsync(string title, string description, string content, string authorId) {
            var book = new Book
            {
                Title = title,
                Description = description,
                Content = content,
                AuthorId = authorId
            };
            await _bookRepository.CreateAsync(book);
            await _notificationService.CreateAsync(authorId, $"Your book '{title}' has been added.");
            BookAdded?.Invoke(this, new BookEventArgs { Book = book });
        }

        public async Task UpdateAsync(Book book) {
            var existingBook = await _bookRepository.GetByIdAsync(book.Id);
            if (existingBook == null) {
                throw new NotFoundException();
            }

            existingBook.Title = book.Title;
            existingBook.Description = book.Description;
            existingBook.Content = book.Content;

            await _bookRepository.UpdateAsync(existingBook);
            await _notificationService.CreateAsync(existingBook.AuthorId, $"Your book '{existingBook.Title}' has been edited.");
            BookEdited?.Invoke(this, new BookEventArgs { Book = existingBook });
        }

        public async Task DeleteAsync(int id) {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null) {
                throw new NotFoundException();
            }

            await _bookRepository.DeleteAsync(id);
            await _notificationService.CreateAsync(book.AuthorId, $"Your book '{book.Title}' has been deleted.");
            BookDeleted?.Invoke(this, new BookEventArgs { Book = book });
        }
    }

    public delegate void BookEventHandler(object sender, BookEventArgs e);

    public class BookEventArgs : EventArgs
    {
        public Book Book { get; set; }
    }
}
