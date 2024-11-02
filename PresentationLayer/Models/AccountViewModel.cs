using DataAccessLayer.Entities;

namespace PresentationLayer.Models
{
    public class AccountViewModel {
        public required User User { get; set; }
        public int UnreadNotificationsCount { get; set; }
    }

    public class SenderOptionViewModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}
