using BusinessLogicLayer.Events;
using DataAccessLayer.Repositories;
using DataAccessLayer.Entities;
using Utils.Exceptions;

namespace BusinessLogicLayer.Services {

    public interface ILibraryBookService {
        Task<LibraryBook?> GetLibraryBookAsync(int id);
        Task CreateAsync(int libraryId, int bookId);
        Task<IEnumerable<LibraryBook?>> GetLibraryBooksAsync(int libraryId, int bookId);
        Task<IEnumerable<LibraryBook>> GetLibraryBooksAsync(int libraryId);
        Task DeleteAsync(int id);
    }

    public class LibraryBookService(ILibraryBookRepository libraryBookRepository,
                              IBookService bookService,
                              ILibraryRepository libraryRepository,
                              IEventPublisher eventPublisher) : ILibraryBookService {
        public async Task<LibraryBook?> GetLibraryBookAsync(int id) {
            var libraryBook = await libraryBookRepository.GetLibraryBookAsync(id);
            if (libraryBook != null) {
                libraryBook.Book = await bookService.GetBookAsync(libraryBook.BookId);
            }
            return libraryBook;
        }

        public async Task<IEnumerable<LibraryBook?>> GetLibraryBooksAsync(int libraryId, int bookId) {
            return await libraryBookRepository.GetLibraryBooksAsync(libraryId, bookId);
        }

        public async Task<IEnumerable<LibraryBook>> GetLibraryBooksAsync(int libraryId) {
            var libraryBooks = await libraryBookRepository.GetLibraryBooksAsync(libraryId);
            foreach (var libraryBook in libraryBooks) {
                libraryBook.Book = await bookService.GetBookAsync(libraryBook.BookId);
            }
            return libraryBooks;
        }

        public async Task CreateAsync(int libraryId, int bookId) {
            Library? library = await libraryRepository.GetLibraryAsync(libraryId);

            if (library == null)
                return;

            var existingBooks = await libraryBookRepository.GetLibraryBooksAsync(libraryId, bookId);

            if (existingBooks.Any() && !library.AllowCopies)
                return;

            LibraryBook? newBook = new() {
                LibraryId = libraryId,
                BookId = bookId,
            };

            await libraryBookRepository.CreateAsync(newBook);
            newBook = await GetLibraryBookAsync(newBook.Id);

            if (newBook == null)
                throw new PublicException("Library book not created");

            eventPublisher.PublishBookAddedToLibrary(newBook);
        }

        public async Task DeleteAsync(int id) {
            var libraryBook = await GetLibraryBookAsync(id);
            if (libraryBook != null) {
                eventPublisher.PublishBookRemovedFromLibrary(libraryBook);
                await libraryBookRepository.DeleteAsync(id);
            }
        }
    }

}
