using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Utils.Exceptions;

namespace BusinessLogicLayer.Services {
    public interface ILibraryService {
        Task<IEnumerable<Library>> GetAllAsync();
        Task<Library?> GetLibraryAsync(int id);
        Task CreateAsync(Library library);
        Task UpdateAsync(Library library);
        Task DeleteAsync(int id);
    }

    public class LibraryService(ILibraryRepository libraryRepository,
                            ILibraryMembershipService membershipService,
                            IBookService bookService) : ILibraryService {
        public async Task<IEnumerable<Library>> GetAllAsync() {
            return await libraryRepository.GetAllAsync();
        }

        public async Task<Library?> GetLibraryAsync(int id) {
            Library? library = await libraryRepository.GetLibraryAsync(id);
            if (library == null)
                return null;
                 
            foreach (LibraryBook lbBook in library.Books)
                lbBook.Book = await bookService.GetBookAsync(lbBook.BookId);

            return library;
        }

        public async Task CreateAsync(Library library) {
            await libraryRepository.CreateAsync(library);
        }

        public async Task UpdateAsync(Library library) {
            await libraryRepository.UpdateAsync(library);
        }

        public async Task DeleteAsync(int id) {
            var library = await libraryRepository.GetLibraryAsync(id);
            if (library == null)
                return;

            await libraryRepository.DeleteAsync(id);
        }

    }
}
