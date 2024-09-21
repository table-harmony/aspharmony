namespace Utils.Exceptions {
    public class NotFoundException : PublicException {
        public const string NOT_FOUND_MESSAGE = "Resource not found";
        public NotFoundException() : base(NOT_FOUND_MESSAGE) { }
    }
}
