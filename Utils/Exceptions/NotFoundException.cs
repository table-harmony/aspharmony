using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Exceptions {
    public class NotFoundException : PublicException {
        public const string NOT_FOUND_MESSAGE = "Resource not found";
        public NotFoundException() : base(NOT_FOUND_MESSAGE) { }
    }
}
