using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services {
    public interface ILibraryService {
        Task CreateAsync(string name);
        Task UpdateAsync(int id, string name);
        Task DeleteAsync(int id);
        IEnumerable<LibraryBook> GetBooks(int id);
        IEnumerable<LibraryMembership> GetMemberships(int id);
        Task<Library> GetByIdAsync(int id);
        Task<IEnumerable<Library>> GetAllAsync();
    }

    public class LibraryService : ILibraryService {
        private readonly ILibraryRepository _libraryRepository;
        private readonly ILibraryMembershipService _membershipService;


        public LibraryService(ILibraryRepository libraryRepository, 
                                ILibraryMembershipService membershipsService) {
            _libraryRepository = libraryRepository;
            _membershipService = membershipsService;
        }

        public async Task CreateAsync(string name) {
            User user = null; // get current user

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
    }
}
