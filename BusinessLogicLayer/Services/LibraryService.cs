using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Utils.Exceptions;

namespace BusinessLogicLayer.Services {
    public interface ILibraryService {
        Task CreateAsync(string name, string userId);
        Task UpdateAsync(int id, string name);
        Task DeleteAsync(int id);
        IEnumerable<LibraryBook> GetBooks(int id);
        IEnumerable<LibraryMembership> GetMemberships(int id);
        Task<Library> GetByIdAsync(int id);
        Task<IEnumerable<Library>> GetAllAsync();
        Task AddBookToLibraryAsync(int libraryId, int bookId);
        Task<LibraryBook> GetLibraryBookByIdAsync(int libraryBookId);
    }

    public class LibraryService : ILibraryService {
        private readonly ILibraryRepository _libraryRepository;
        private readonly ILibraryMembershipService _membershipService;
        private readonly IBookService _bookService;
        private readonly IUserService _userService;

        public LibraryService(ILibraryRepository libraryRepository, 
                                ILibraryMembershipService membershipsService,
                                IUserService userService,
                                IBookService bookService) {
            _libraryRepository = libraryRepository;
            _userService = userService;
            _membershipService = membershipsService;
            _bookService = bookService;
        }

        public async Task CreateAsync(string name, string userId) {
            User user = await _userService.GetByIdAsync(userId);
            Library library = new() {
                Name = name,
            };

            await _libraryRepository.CreateAsync(library);
            await _membershipService.CreateAsync(user, library, MembershipRole.Manager);
        }

        public IEnumerable<LibraryMembership> GetMemberships(int id) {
            return _libraryRepository.GetMemberships(id);
        }

        public async Task<Library> GetByIdAsync(int id) {
            return await _libraryRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Library>> GetAllAsync() {
            return await _libraryRepository.GetAllAsync();
        }

        public async Task UpdateAsync(int id, string name) {
            Library library = new Library() {
                Name = name,
                Id = id
            };

            await _libraryRepository.UpdateAsync(library);
        }

        public async Task DeleteAsync(int id) {
            await _libraryRepository.DeleteAsync(id);
        }

        public IEnumerable<LibraryBook> GetBooks(int id) {
            return _libraryRepository.GetBooks(id);
        }

        public async Task AddBookToLibraryAsync(int libraryId, int bookId) {
            var libraryBook = new LibraryBook {
                LibraryId = libraryId,
                BookId = bookId
            };

            await _libraryRepository.AddBookAsync(libraryBook);
        }

        public async Task<LibraryBook> GetLibraryBookByIdAsync(int libraryBookId) {
            return await _libraryRepository.GetLibraryBookByIdAsync(libraryBookId);
        }
    }
}
