using BusinessLogicLayer.Events;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using System;

namespace BusinessLogicLayer.Services {
    public interface ILibraryBookService {
        Task<LibraryBook> GetLibraryBookAsync(int id);
        Task<LibraryBook> GetLibraryBookAsync(int libraryId, int bookId);
        Task CreateAsync(int libraryId, int bookId);
        Task<IEnumerable<LibraryBook>> GetLibraryBooksAsync(int libraryId);
        Task DeleteAsync(int id);
    }

    public class LibraryBookService : ILibraryBookService {
        private readonly ILibraryBookRepository _libraryBookRepository;
        private readonly IBookService _bookService;
        private readonly ILibraryRepository _libraryRepository;

        public LibraryBookService(ILibraryBookRepository libraryBookRepository, 
                                  IBookService bookService,
                                  ILibraryRepository libraryRepository) {
            _libraryBookRepository = libraryBookRepository;
            _bookService = bookService;
            _libraryRepository = libraryRepository;
        }

        public async Task<LibraryBook> GetLibraryBookAsync(int id) {
            var libraryBook = await _libraryBookRepository.GetLibraryBookAsync(id);
            if (libraryBook != null) {
                libraryBook.Book = await _bookService.GetBookAsync(libraryBook.BookId);
            }
            return libraryBook;
        }

        public async Task<LibraryBook> GetLibraryBookAsync(int libraryId, int bookId) {
            var libraryBook = await _libraryBookRepository.GetLibraryBookAsync(libraryId, bookId);
            if (libraryBook != null) {
                libraryBook.Book = await _bookService.GetBookAsync(bookId);
            }
            return libraryBook;
        }
        
        public async Task<IEnumerable<LibraryBook>> GetLibraryBooksAsync(int libraryId) {
            var libraryBooks = await _libraryBookRepository.GetLibraryBooksAsync(libraryId);
            foreach (var libraryBook in libraryBooks) {
                libraryBook.Book = await _bookService.GetBookAsync(libraryBook.BookId);
            }
            return libraryBooks;
        }

        public async Task CreateAsync(int libraryId, int bookId) {
            LibraryBook libraryBook = new() {
                LibraryId = libraryId,
                BookId = bookId,
            };

            await _libraryBookRepository.CreateAsync(libraryBook);

            var book = await _bookService.GetBookAsync(bookId);
            var library = await _libraryRepository.GetLibraryAsync(libraryId);

            LibraryEvents.OnBookAddedToLibrary(book, library, book.Title);
        }

        public async Task DeleteAsync(int id) {
            var libraryBook = await GetLibraryBookAsync(id);
            if (libraryBook != null) {
                await _libraryBookRepository.DeleteAsync(id);
                var library = await _libraryRepository.GetLibraryAsync(libraryBook.LibraryId);
                var book = await _bookService.GetBookAsync(libraryBook.BookId);
                LibraryEvents.OnBookRemovedFromLibrary(book, library, book.Title);
            }
        }
    }

}
