using BusinessLogicLayer.Events;
using DataAccessLayer.Repositories;
using DataAccessLayer.Entities;
using Utils.Exceptions;

namespace BusinessLogicLayer.Services {

    public interface ILibraryBookService {
        Task<LibraryBook?> GetLibraryBookAsync(int id);
        Task<LibraryBook?> GetLibraryBookAsync(int libraryId, int bookId);
        Task CreateAsync(int libraryId, int bookId);
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

        public async Task<LibraryBook?> GetLibraryBookAsync(int libraryId, int bookId) {
            var libraryBook = await libraryBookRepository.GetLibraryBookAsync(libraryId, bookId);
            if (libraryBook != null) {
                libraryBook.Book = await bookService.GetBookAsync(bookId);
            }
            return libraryBook;
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

            LibraryBook? existingBook = await libraryBookRepository.GetLibraryBookAsync(libraryId, bookId);

            if (existingBook != null && !library.AllowCopies)
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
