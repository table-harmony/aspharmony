namespace Utils.Senders.Configuration {
    public record TwilioConfig {
        public required string AccountSid { get; init; }
        public required string AuthToken { get; init; }
        public required string SmsNumber { get; init; }
        public required string WhatsAppNumber { get; init; }
    }
}