using DataAccessLayer.Entities;

namespace PresentationLayer.Models
{
    public class AccountViewModel
    {
        public User User { get; set; }
        public List<SenderOptionViewModel> SenderOptions { get; set; }
    }

    public class SenderOptionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}
