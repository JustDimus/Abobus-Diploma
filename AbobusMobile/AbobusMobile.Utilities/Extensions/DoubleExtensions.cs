using AbobusMobile.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Utilities.Extensions
{
    public static class DoubleExtensions
    {
        public static void ValidateIsNotEmpty(this double value)
        {
            if (value == default)
            {
                throw new ValidationException(nameof(value));
            }
        }
    }
}
