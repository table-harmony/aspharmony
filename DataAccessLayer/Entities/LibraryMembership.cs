namespace DataAccessLayer.Entities {
    public enum MembershipRole {
        Manager,
        Staff
    }

    public class LibraryMembership {
        public int Id {  get; set; }
    
        public User User { get; set; }
        public int UserId { get; set; }

        public Library Library { get; set; }
        public int LibraryId { get; set; }

        public MembershipRole Role { get; set; }
    }
}
