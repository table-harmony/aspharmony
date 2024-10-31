namespace Utils.Exceptions {
    public class SenderException : PublicException {
        public const string SENDER_MESSAGE = "Unable to send message";
        public SenderException() : base(SENDER_MESSAGE) { }
    }
}
