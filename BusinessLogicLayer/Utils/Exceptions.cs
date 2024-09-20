using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Utils {
    public class PublicException(string message) : Exception(message) {
    }

    public class NotFoundException : PublicException {
        public const string NOT_FOUND_MESSAGE = "Resource not found";

        public NotFoundException() : base(NOT_FOUND_MESSAGE) {}
    }

    public class AuthorizationException : PublicException {
        public const string AUTHORIZATION_MESSAGE = "You are not authorized to view this content";
        
        public AuthorizationException() : base(AUTHORIZATION_MESSAGE) { }
    }

    public class AuthenticationException : PublicException {
        public const string AUTHENTICATION_MESSAGE = "You must be logged in to view this content";

        public AuthenticationException() : base(AUTHENTICATION_MESSAGE) { }
    }


}
