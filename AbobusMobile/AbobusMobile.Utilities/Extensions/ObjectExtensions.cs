using AbobusMobile.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Utilities.Extensions
{
    public static class ObjectExtensions
    {
        public static void ValidateIsNull(this object obj)
        {
            if (obj != null)
            {
                throw new ValidationException($"{nameof(obj)} is not null");
            }
        }

        public static void ValidateIsNotNull(this object obj)
        {
            if (obj == null)
            {
                throw new ValidationException($"{nameof(obj)} is null");
            }
        }

        public static bool IsNull(this object obj)
            => obj == null;

        public static bool IsNotNull(this object obj)
            => !obj.IsNull();
    }
}
