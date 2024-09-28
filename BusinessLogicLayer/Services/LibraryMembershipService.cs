using BusinessLogicLayer.Events;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services
{
    public interface ILibraryMembershipService {
        Task CreateAsync(LibraryMembership membership);
        IEnumerable<LibraryMembership> GetLibraryMembers(int libraryId);
        Task DeleteAsync(int libraryId, string userId);
        Task DeleteAsync(int id);
        Task<LibraryMembership> GetMembershipAsync(int libraryId, string userId);
        Task<LibraryMembership> GetMembershipAsync(int id);
    }

    public class LibraryMembershipService : ILibraryMembershipService {
        private readonly ILibraryMembershipRepository _membershipRepository;
        private readonly IEventPublisher _eventPublisher;

        public LibraryMembershipService(
            ILibraryMembershipRepository membershipRepository,
            IEventPublisher eventPublisher)
        {
            _membershipRepository = membershipRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task CreateAsync(LibraryMembership membership) {
            await _membershipRepository.CreateAsync(membership);

            membership = await GetMembershipAsync(membership.Id);
            _eventPublisher.PublishUserJoinedLibrary(membership);
        }

        public async Task DeleteAsync(int libraryId, string userId) {
            var membership = await GetMembershipAsync(libraryId, userId);
            if (membership != null) {
                await _membershipRepository.DeleteAsync(libraryId, userId);
                _eventPublisher.PublishUserLeftLibrary(membership);
            }
        }

        public async Task DeleteAsync(int id) {
            var membership = await GetMembershipAsync(id);
            if (membership != null) {
                await _membershipRepository.DeleteAsync(id);
                _eventPublisher.PublishUserLeftLibrary(membership);
            }
        }

        public IEnumerable<LibraryMembership> GetLibraryMembers(int libraryId) {
            return _membershipRepository.GetLibraryMembers(libraryId);
        }

        public async Task<LibraryMembership> GetMembershipAsync(int libraryId, string userId) {
            return await _membershipRepository.GetMembershipAsync(libraryId, userId);
        }

        public async Task<LibraryMembership> GetMembershipAsync(int id) {
            return await _membershipRepository.GetMembershipAsync(id);
        }
    }
}