using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Encryption {
    public class Sha256Encryption : IEncryption {
        //TODO: add salt encryption
        public string Encrypt(string input) {
            using (SHA256 hash = SHA256.Create()) {
                byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();

                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));

                return builder.ToString();
            }
        }

        public bool Compare(string input, string hash) {
            string hashedInput = Encrypt(input);
            return hashedInput == hash;
        }
    }
}
