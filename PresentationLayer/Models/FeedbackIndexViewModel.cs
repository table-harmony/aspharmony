using DataAccessLayer.Entities;

namespace PresentationLayer.Models {
    public class FeedbackIndexViewModel {
        public PaginatedList<Feedback> Feedbacks { get; set; }
        public string SearchString { get; set; }
        public int PageSize { get; set; }
    }
}