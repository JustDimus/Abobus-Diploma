using AbobusMobile.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Utilities.Extensions
{
    public static class GuidExtensions
    {
        public static void ValidateNotEmpty(this Guid guid)
        {
            if (guid == Guid.Empty)
            {
                throw new ValidationException(nameof(guid));
            }
        }

        public static bool IsNotEmpty(this Guid guid)
            => guid != Guid.Empty;
    }
}
