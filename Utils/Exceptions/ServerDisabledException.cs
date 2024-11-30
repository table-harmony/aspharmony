namespace Utils.Exceptions;

public class ServerDisabledException : PublicException {
    public const string SERVER_DISABLED_MESSAGE = "Server is disabled";
    public ServerDisabledException() : base(SERVER_DISABLED_MESSAGE) { }
    public ServerDisabledException(string serverName) : base($"The Server '{serverName}' is disabled") { }
}