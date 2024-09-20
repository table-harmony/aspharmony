namespace Utils.Encryption {
    public interface IEncryption {
        string Encrypt(string password);
        bool Compare(string input, string hash);
    }
}