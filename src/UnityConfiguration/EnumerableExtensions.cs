using System;
using System.Collections.Generic;

namespace UnityConfiguration
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var obj in enumerable)
            {
                action(obj);
            }
        }
    }
}