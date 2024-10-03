namespace Utils.Encryption {
    public interface IEncryption {
        string Encrypt(string input);
        bool Compare(string input, string hash);
    }
}