using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services
{
    public interface ILibraryMembershipService
    {
        Task CreateAsync(User user, Library library, MembershipRole role);
        // Add other methods as needed
    }

    public class LibraryMembershipService : ILibraryMembershipService
    {
        private readonly ILibraryMembershipRepository _libraryMembershipRepository;

        public LibraryMembershipService(ILibraryMembershipRepository libraryMembershipRepository)
        {
            _libraryMembershipRepository = libraryMembershipRepository;
        }

        public async Task CreateAsync(User user, Library library, MembershipRole role)
        {
            var membership = new LibraryMembership
            {
                UserId = user.Id,
                LibraryId = library.Id,
                Role = role
            };

            await _libraryMembershipRepository.CreateAsync(membership);
        }

        // Implement other methods as needed
    }
}