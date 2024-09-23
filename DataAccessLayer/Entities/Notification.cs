using System;

namespace DataAccessLayer.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}