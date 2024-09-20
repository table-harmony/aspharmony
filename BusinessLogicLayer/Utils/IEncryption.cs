using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Utils
{
    public interface IEncryption
    {
        public abstract string Encrypt(string password);
        public abstract bool Compare(string input, string hash);
    }
}
