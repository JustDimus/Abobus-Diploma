using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AbobusMobile.Utilities.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmail(this string value)
            => !string.IsNullOrWhiteSpace(value)
            && Regex.IsMatch(value, "^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$");

        public static bool IsNotNullOrWhiteSpace(this string value) => !string.IsNullOrWhiteSpace(value);
    }
}
