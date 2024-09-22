using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services
{
    public interface ILibraryMembershipService {
        Task CreateAsync(User user, Library library, MembershipRole role);
        Task<IEnumerable<LibraryMembership>> GetMembersByLibraryIdAsync(int libraryId);
        Task DeleteAsync(int libraryId, string userId);
        Task<bool> IsMemberAsync(int libraryId, string userId);
    }

    public class LibraryMembershipService : ILibraryMembershipService
    {
        private readonly ILibraryMembershipRepository _libraryMembershipRepository;

        public LibraryMembershipService(ILibraryMembershipRepository libraryMembershipRepository) {
            _libraryMembershipRepository = libraryMembershipRepository;
        }

        public async Task CreateAsync(User user, Library library, MembershipRole role) {
            var membership = new LibraryMembership {
                UserId = user.Id,
                LibraryId = library.Id,
                Role = role
            };

            await _libraryMembershipRepository.CreateAsync(membership);
        }

        public async Task<IEnumerable<LibraryMembership>> GetMembersByLibraryIdAsync(int libraryId)
        {
            return await _libraryMembershipRepository.GetMembersByLibraryIdAsync(libraryId);
        }

        public async Task DeleteAsync(int libraryId, string userId)
        {
            await _libraryMembershipRepository.DeleteAsync(libraryId, userId);
        }

        public async Task<bool> IsMemberAsync(int libraryId, string userId) {
            var membership = await _libraryMembershipRepository.GetByLibraryAndUserIdAsync(libraryId, userId);
            return membership != null;
        }
    }
}