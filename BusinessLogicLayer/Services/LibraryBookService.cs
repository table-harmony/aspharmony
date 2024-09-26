using BusinessLogicLayer.Events;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using System;

namespace BusinessLogicLayer.Services {
    public interface ILibraryBookService {
        LibraryBook GetLibraryBook(int id);
        LibraryBook GetLibraryBook(int libraryId, int bookId);
        Task CreateAsync(int libraryId, int bookId);
        IEnumerable<LibraryBook> GetLibraryBooks(int libraryId);
        Task DeleteAsync(int id);
    }

    public class LibraryBookService : ILibraryBookService {
        private readonly ILibraryBookRepository _libraryBookRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ILibraryRepository _libraryRepository;
        public LibraryBookService(ILibraryBookRepository libraryBookRepository, 
                                  IBookRepository bookRepository,
                                  ILibraryRepository libraryRepository) {
            _libraryBookRepository = libraryBookRepository;
            _bookRepository = bookRepository;
            _libraryRepository = libraryRepository;
        }

        public LibraryBook GetLibraryBook(int id) {
            return _libraryBookRepository.GetLibraryBook(id);
        }

        public LibraryBook GetLibraryBook(int libraryId, int bookId) {
            return _libraryBookRepository.GetLibraryBook(bookId, libraryId);
        }
        
        public IEnumerable<LibraryBook> GetLibraryBooks(int libraryId) {
            return _libraryBookRepository.GetLibraryBooks(libraryId);
        }

        public async Task CreateAsync(int libraryId, int bookId) {
            LibraryBook libraryBook = new() {
                LibraryId = libraryId,
                BookId = bookId,
            };

            await _libraryBookRepository.CreateAsync(libraryBook);

            UserEvents.OnBookAddedToLibrary(bookId, libraryId);
        }

        public async Task DeleteAsync(int id) {
            await _libraryBookRepository.DeleteAsync(id);
        }
    }

}
