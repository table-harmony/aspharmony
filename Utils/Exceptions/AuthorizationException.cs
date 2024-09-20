using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Exceptions {
    public class AuthorizationException : PublicException {
        public const string AUTHORIZATION_MESSAGE = "You are not authorized to view this content";
        public AuthorizationException() : base(AUTHORIZATION_MESSAGE) { }
    }
}
