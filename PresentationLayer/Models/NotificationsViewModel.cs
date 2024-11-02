using DataAccessLayer.Entities;

namespace PresentationLayer.Models
{
    public class NotificationsViewModel
    {
        public required PaginatedList<Notification> Notifications { get; set; }
        public required List<SenderOptionViewModel> SenderOptions { get; set; }
        public int PageSize { get; set; }
        public bool UnreadOnly { get; set; }
    }
} 