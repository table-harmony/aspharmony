namespace DataAccessLayer.Entities {
    public enum MembershipRole {
        Manager,
        Member
    }

    public class LibraryMembership {
        public int Id {  get; set; }
    
        public User User { get; set; }
        public string UserId { get; set; }

        public Library Library { get; set; }
        public int LibraryId { get; set; }

        public MembershipRole Role { get; set; }
        public ICollection<BookLoan> BookLoans { get; set; }
    }
}
