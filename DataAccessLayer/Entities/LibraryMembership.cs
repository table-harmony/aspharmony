using System.Collections.Generic;

namespace DataAccessLayer.Entities {
    public enum MembershipRole {
        Manager,
        Member
    }

    public class LibraryMembership {
        public int Id { get; set; } // Primary key
        public MembershipRole Role { get; set; }
        public ICollection<BookLoan> BookLoans { get; set; }
        
        public string UserId { get; set; } // Foreign key
        public User User { get; set; }
        
        public int LibraryId { get; set; } // Foreign key
        public Library Library { get; set; }
    }
}
