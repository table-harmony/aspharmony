using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Exceptions {
    public class AuthenticationException : PublicException {
        public const string AUTHENTICATION_MESSAGE = "You must be logged in to view this content";
        public AuthenticationException() : base(AUTHENTICATION_MESSAGE) { }
    }
}
