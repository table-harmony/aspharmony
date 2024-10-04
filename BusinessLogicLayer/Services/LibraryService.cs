using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Utils.Exceptions;

namespace BusinessLogicLayer.Services {
    public interface ILibraryService {
        Task<IEnumerable<Library>> GetAllAsync();
        Task<Library> GetLibraryAsync(int id);
        Task CreateAsync(Library library);
        Task UpdateAsync(int id, string name);
        Task DeleteAsync(int id);
    }

    public class LibraryService : ILibraryService {
        private readonly ILibraryRepository _libraryRepository;
        private readonly ILibraryMembershipService _membershipService;
        private readonly IBookService _bookService;

        public LibraryService(ILibraryRepository libraryRepository, 
                                ILibraryMembershipService membershipService,
                                IBookService bookService) {
            _libraryRepository = libraryRepository;
            _bookService = bookService;
            _membershipService = membershipService;
        }

        public async Task<IEnumerable<Library>> GetAllAsync() {
            return await _libraryRepository.GetAllAsync();
        }

        public async Task<Library> GetLibraryAsync(int id) {
            Library library = await _libraryRepository.GetLibraryAsync(id);
            if (library == null)
                throw new NotFoundException();
                 
            foreach (LibraryBook lbBook in library.Books)
                lbBook.Book = await _bookService.GetBookAsync(lbBook.BookId);

            return library;
        }

        public async Task CreateAsync(Library library) {
            await _libraryRepository.CreateAsync(library);
        }

        public async Task UpdateAsync(int id, string name) {
            var library = await _libraryRepository.GetLibraryAsync(id);
            if (library == null)
                throw new NotFoundException();
         
            library.Name = name;
            await _libraryRepository.UpdateAsync(library);
        }

        public async Task DeleteAsync(int id) {
            var library = await _libraryRepository.GetLibraryAsync(id);
            if (library == null)
                throw new NotFoundException();

            await _libraryRepository.DeleteAsync(library.Id);
        }

    }
}
