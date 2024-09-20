using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities {
    public enum MembershipRole {
        Manager,
        Staff 
    }

    public class LibraryMemberships {
        public int Id {  get; set; }
    
        public User User { get; set; }
        public int UserId { get; set; }

        public Library Library { get; set; }
        public int LibraryId { get; set; }

        public MembershipRole Role { get; set; }
    }
}
