namespace Utils.Encryption {
    public interface IEncryption {
        public abstract string Encrypt(string password);
        public abstract bool Compare(string input, string hash);
    }
}