using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities {
    public class User : Entity {
        public string Email { get; set; }
        public string Password { get; set; }


    }

}
