using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services
{
    public interface ILibraryMembershipService {
        Task CreateAsync(User user, Library library, MembershipRole role);
    }

    public class LibraryMembershipService : ILibraryMembershipService {
        private readonly ILibraryMembershipRepository _membershipRepository;

        public LibraryMembershipService(ILibraryMembershipRepository membershipRepository) {
            _membershipRepository = membershipRepository;
        }

        public async Task CreateAsync(User user, Library library, MembershipRole role) {
            var membership = new LibraryMembership {
                UserId = user.Id,
                LibraryId = library.Id,
                Role = role,
            };

            await _membershipRepository.CreateAsync(membership);
        }
    }
}