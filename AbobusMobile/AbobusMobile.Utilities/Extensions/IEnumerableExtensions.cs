using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Utilities.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<TEntity> ContinueWith<TEntity>(this IEnumerable<TEntity> first, IEnumerable<TEntity> second)
        {
            foreach (var entity in first)
            {
                yield return entity;
            }

            foreach (var entity in second)
            {
                yield return entity;
            }

            yield break;
        }
    }
}
