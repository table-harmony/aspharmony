using DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public interface ILibraryMembershipService
    {
        Task CreateAsync(User user, Library library, MembershipRole role);
        Task<IEnumerable<LibraryMembership>> GetMembersByLibraryIdAsync(int libraryId);
        Task DeleteAsync(int libraryId, string userId);
        Task<bool> IsMemberAsync(int libraryId, string userId);
        Task<LibraryMembership> GetByLibraryAndUserIdAsync(int libraryId, string userId); // Add this method
    }
}