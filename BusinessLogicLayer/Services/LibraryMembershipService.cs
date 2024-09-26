using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public interface ILibraryMembershipService {
        Task CreateAsync(LibraryMembership membership);
        IEnumerable<LibraryMembership> GetLibraryMembers(int libraryId);
        Task DeleteAsync(int libraryId, string userId);
        Task DeleteAsync(int id);
        Task<LibraryMembership> GetMembershipAsync(int libraryId, string userId);
        Task<LibraryMembership> GetMembershipAsync(int id);

        event EventHandler<LibraryMembershipEventArgs> MemberJoined;
    }

    public class LibraryMembershipService : ILibraryMembershipService {
        private readonly ILibraryMembershipRepository _membershipRepository;
        private readonly ILibraryRepository _libraryRepository;
        private readonly IUserRepository _userRepository;

        public event EventHandler<LibraryMembershipEventArgs> MemberJoined;

        public LibraryMembershipService(ILibraryMembershipRepository membershipRepository,
                                        ILibraryRepository libraryRepository,
                                        IUserRepository userRepository) {
            _membershipRepository = membershipRepository;
            _libraryRepository = libraryRepository;
            _userRepository = userRepository;
        }

        public async Task CreateAsync(LibraryMembership membership) {
            await _membershipRepository.CreateAsync(membership);

            var library = await _libraryRepository.GetLibraryAsync(membership.LibraryId);
            var user = await _userRepository.GetByIdAsync(membership.UserId);

            OnMemberJoined(new LibraryMembershipEventArgs(user, library));
        }

        protected virtual void OnMemberJoined(LibraryMembershipEventArgs e) {
            MemberJoined?.Invoke(this, e);
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

    public class LibraryMembershipEventArgs : EventArgs {
        public User User { get; }
        public Library Library { get; }

        public LibraryMembershipEventArgs(User user, Library library) {
            User = user;
            Library = library;
        }
    }
}