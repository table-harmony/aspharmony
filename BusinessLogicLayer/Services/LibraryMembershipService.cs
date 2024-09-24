using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public interface ILibraryMembershipService {
        Task CreateAsync(string userId, int libraryId, MembershipRole role);
        IEnumerable<LibraryMembership> GetLibraryMembers(int libraryId);
        Task DeleteAsync(int libraryId, string userId);
        Task DeleteAsync(int id);
        Task<LibraryMembership> GetMembershipAsync(int libraryId, string userId);
        Task<LibraryMembership> GetMembershipAsync(int id);
    }

    public class LibraryMembershipService : ILibraryMembershipService {
        private readonly ILibraryMembershipRepository _membershipRepository;

        public LibraryMembershipService(ILibraryMembershipRepository membershipRepository) {
            _membershipRepository = membershipRepository;
        }

        public async Task CreateAsync(string userId, int libraryId, MembershipRole role) {
            LibraryMembership membership = new LibraryMembership {
                UserId = userId,
                LibraryId = libraryId,
                Role = role
            };

            await _membershipRepository.CreateAsync(membership);
        }

        public IEnumerable<LibraryMembership> GetLibraryMembers(int libraryId) {
            return _membershipRepository.GetLibraryMembers(libraryId);
        }

        public async Task DeleteAsync(int libraryId, string userId) {
            await _membershipRepository.DeleteAsync(libraryId, userId);
        }

        public async Task DeleteAsync(int id) {
            await _membershipRepository.DeleteAsync(id);
        }

        public async Task<LibraryMembership> GetMembershipAsync(int libraryId, string userId) {
            return await _membershipRepository.GetMembershipAsync(libraryId, userId);
        }

        public async Task<LibraryMembership> GetMembershipAsync(int id) {
            return await _membershipRepository.GetMembershipAsync(id);
        }
    }
}