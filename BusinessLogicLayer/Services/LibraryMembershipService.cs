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
        Task UpdateAsync(LibraryMembership membership);
        Task<LibraryMembership?> GetMembershipAsync(int libraryId, string userId);
        Task<LibraryMembership?> GetMembershipAsync(int id);
    }

    public class LibraryMembershipService(
        ILibraryMembershipRepository membershipRepository,
        IEventPublisher eventPublisher) : ILibraryMembershipService {

        public async Task CreateAsync(LibraryMembership membership) {
            await membershipRepository.CreateAsync(membership);

            membership = await GetMembershipAsync(membership.Id);
            eventPublisher.PublishUserJoinedLibrary(membership);
        }

        public async Task DeleteAsync(int libraryId, string userId) {
            var membership = await GetMembershipAsync(libraryId, userId);
            if (membership != null) {
                await membershipRepository.DeleteAsync(libraryId, userId);
                eventPublisher.PublishUserLeftLibrary(membership);
            }
        }

        public async Task DeleteAsync(int id) {
            var membership = await GetMembershipAsync(id);
            if (membership != null) {
                await membershipRepository.DeleteAsync(id);
                eventPublisher.PublishUserLeftLibrary(membership);
            }
        }

        public IEnumerable<LibraryMembership> GetLibraryMembers(int libraryId) {
            return membershipRepository.GetLibraryMembers(libraryId);
        }

        public async Task<LibraryMembership?> GetMembershipAsync(int libraryId, string userId) {
            return await membershipRepository.GetMembershipAsync(libraryId, userId);
        }

        public async Task<LibraryMembership?> GetMembershipAsync(int id) {
            return await membershipRepository.GetMembershipAsync(id);
        }

        public async Task UpdateAsync(LibraryMembership membership) {
            await membershipRepository.UpdateAsync(membership);
        }
    }
}