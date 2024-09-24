using DataAccessLayer.Entities;
using System.Collections.Generic;

namespace PresentationLayer.Models
{
    public class ProfileViewModel
    {
        public UpdatePasswordViewModel UpdatePasswordViewModel { get; set; }
        public IEnumerable<Notification> Notifications { get; set; }
    }
}