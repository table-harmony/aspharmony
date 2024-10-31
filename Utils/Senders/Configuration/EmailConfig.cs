namespace Utils.Senders.Configuration {
    public record EmailConfig {
        public required string From { get; set; }
        public required string Username { get; init; }
        public required string Password { get; init; }
        public required string Host { get; init; }
        public required int Port { get; init; }
    }
}