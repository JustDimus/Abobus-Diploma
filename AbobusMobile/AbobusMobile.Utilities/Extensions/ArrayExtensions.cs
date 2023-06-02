using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Utilities.Extensions
{
    public static class ArrayExtensions
    {
        public static T[] Add<T>(this T[] first, T[] second)
        {
            first.ValidateIsNotNull();

            if (second == null )
            {
                return first;
            }

            var mergedArray = new T[first.Length + second.Length];
            first.CopyTo(mergedArray, 0);
            second.CopyTo(mergedArray, first.Length);

            return mergedArray;
        }
    }
}
