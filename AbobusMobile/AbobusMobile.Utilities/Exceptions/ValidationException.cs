using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Utilities.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message, Exception innerException = null)
            : base(message, innerException) { }
    }
}
