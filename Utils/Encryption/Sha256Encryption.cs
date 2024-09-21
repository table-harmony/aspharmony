using System;
using System.Security.Cryptography;
using System.Text;

namespace Utils.Encryption
{
    public class Sha256Encryption : IEncryption {
        private const int SaltSize = 16; // 128 bits

        public string Encrypt(string input) {
            string salt = GenerateSalt();
            string hash = ComputeHash(input, salt);
            return $"{hash}:{salt}";
        }

        public bool Compare(string input, string hashWithSalt) {
            var parts = hashWithSalt.Split(':');
            if (parts.Length != 2)
                return false;

            string storedHash = parts[0];
            string storedSalt = parts[1];

            string computedHash = ComputeHash(input, storedSalt);
            return computedHash == storedHash;
        }

        private string GenerateSalt() {
            using (var rng = new RNGCryptoServiceProvider()) {
                byte[] saltBytes = new byte[SaltSize];
                rng.GetBytes(saltBytes);
                return Convert.ToBase64String(saltBytes);
            }
        }

        private string ComputeHash(string input, string salt) {
            using (var sha256 = SHA256.Create()) {
                string inputWithSalt = input + salt;
                byte[] inputBytes = Encoding.UTF8.GetBytes(inputWithSalt);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}