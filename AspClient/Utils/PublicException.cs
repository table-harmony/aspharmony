using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspClient.Utils {
    public class PublicException : Exception {
        public PublicException(string message) : base(message) {
        }
    }
}