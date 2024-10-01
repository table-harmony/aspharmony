namespace DataAccessLayer.Entities {
    public enum LabelType {
        Idea,
        Issue,
        Question,
        Complaint,
        Feature,
        Other
    };

    public class Feedback {
        public int Id { get; set; }  // Primary key

        public string UserId { get; set; }  // Foreign key
        public User User { get; set; }
        
        public string Title { get; set; }
        public string Description { get; set; }
        public LabelType Label {  get; set; }

        public DateTime CreatedAt { get; set; }
    }
}