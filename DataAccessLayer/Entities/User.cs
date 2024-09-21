using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Entities {
    public enum UserRole {
        Admin,
        Member
    };

    public class User : IdentityUser {
        public UserRole Role { get; set; }
        public ICollection<Book> Books { get; set; }
        public ICollection<LibraryMembership> Memberships { get; set; }
    }
}
