using System;
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
        Task RemoveBookFromLibraryAsync(int libraryId, int libraryBookId);
        Task<IEnumerable<User>> GetLibraryManagersAsync(int libraryId);
        Task AddMemberAsync(int libraryId, string userId);
        Task RemoveMemberAsync(int libraryId, string userId);
        Task DeleteLibraryAsync(int libraryId, string userId);
        Task RemoveBookFromLibraryAsync(int libraryId, int bookId, string userId);
        Task<bool> IsLibraryManagerAsync(int libraryId, string userId);
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
            var library = await _libraryRepository.GetByIdAsync(id);
            if (library == null)
            {
                throw new NotFoundException();
            }
            return library;
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

        public async Task RemoveBookFromLibraryAsync(int libraryId, int libraryBookId) {
            var library = await _libraryRepository.GetByIdAsync(libraryId);
            if (library == null) {
                throw new NotFoundException();
            }

            var libraryBook = library.Books.FirstOrDefault(lb => lb.Id == libraryBookId);
            if (libraryBook == null) {
                throw new NotFoundException();
            }

            library.Books.Remove(libraryBook);
            await _libraryRepository.UpdateAsync(library);
        }

        public async Task AddMemberAsync(int libraryId, string userId)
        {
            var library = await _libraryRepository.GetByIdAsync(libraryId);
            if (library == null)
                throw new NotFoundException();
            
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException();
            

            await _membershipService.CreateAsync(user, library, MembershipRole.Member);
            
            var managers = await GetLibraryManagersAsync(libraryId);
          
        }

        public async Task RemoveMemberAsync(int libraryId, string userId) {
            await _membershipService.DeleteAsync(libraryId, userId);
        }

        public async Task<IEnumerable<User>> GetLibraryManagersAsync(int libraryId) {
            var library = await _libraryRepository.GetByIdAsync(libraryId);
            if (library == null)
                throw new NotFoundException();

            return library.Memberships
                .Where(m => m.Role == MembershipRole.Manager)
                .Select(m => m.User)
                .ToList();
        }

        public async Task DeleteLibraryAsync(int libraryId, string userId) {
            var isManager = await IsLibraryManagerAsync(libraryId, userId);
            if (!isManager) {
                throw new UnauthorizedAccessException("Only library managers can delete libraries.");
            }

            await _libraryRepository.DeleteAsync(libraryId);
        }

        public async Task RemoveBookFromLibraryAsync(int libraryId, int bookId, string userId) {
            var isManager = await IsLibraryManagerAsync(libraryId, userId);
            if (!isManager) {
                throw new UnauthorizedAccessException("Only library managers can remove books from libraries.");
            }

            var library = await _libraryRepository.GetByIdAsync(libraryId);
            var libraryBook = library.Books.FirstOrDefault(lb => lb.BookId == bookId);
            if (libraryBook == null)
                throw new NotFoundException();
            

            library.Books.Remove(libraryBook);
            await _libraryRepository.UpdateAsync(library);
        }

        public async Task<bool> IsLibraryManagerAsync(int libraryId, string userId)
        {
            var membership = await _membershipService.GetByLibraryAndUserIdAsync(libraryId, userId);
            return membership != null && membership.Role == MembershipRole.Manager;
        }
    }
}
