﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities {
    public enum UserRole {
        Admin,
        Member
    };

    public class User {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        
        public ICollection<Book> Books { get; set; }
        public ICollection<LibraryMembership> Memberships { get; set; }

    }

}
