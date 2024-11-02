namespace DataAccessLayer.Entities {    
    public enum Label {
        Idea,
        Issue,
        Feature,
        Complaint,
        Other
    }

    public class Feedback {
        public int Id { get; set; }  // Primary key

        public string UserId { get; set; }  // Foreign key
        public User User { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public Label Label { get; set; }
    }
}
