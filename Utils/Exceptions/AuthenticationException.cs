namespace Utils.Exceptions {
    public class AuthenticationException : PublicException {
        public const string AUTHENTICATION_MESSAGE = "You must be logged in to view this content";
        public AuthenticationException() : base(AUTHENTICATION_MESSAGE) { }
    }
}
