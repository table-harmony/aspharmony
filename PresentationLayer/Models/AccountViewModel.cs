using DataAccessLayer.Entities;

namespace PresentationLayer.Models
{
    public class AccountViewModel {
        public int UnreadNotificationsCount { get; set; }
        public UpdateProfileViewModel UpdateProfile { get; set; }
        public UpdatePasswordViewModel UpdatePassword { get; set; }
    }

    public class SenderOptionViewModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}
