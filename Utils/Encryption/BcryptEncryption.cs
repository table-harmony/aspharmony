using System;
using BCrypt.Net;

namespace Utils.Encryption
{
    public class BcryptEncryption : IEncryption {
        private const int WorkFactor = 12; // Adjust this based on your security needs

        public string Encrypt(string input) {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(WorkFactor);
            return BCrypt.Net.BCrypt.HashPassword(input, salt);
        }

        public bool Compare(string input, string hashWithSalt) {
            if (string.IsNullOrEmpty(hashWithSalt) || 
                !hashWithSalt.StartsWith("$2a$") && 
                !hashWithSalt.StartsWith("$2b$"))
                return false; // Invalid hash format
          
            try {
                return BCrypt.Net.BCrypt.Verify(input, hashWithSalt);
            }
            catch (Exception) {
                return false;
            }
        }
    }
}