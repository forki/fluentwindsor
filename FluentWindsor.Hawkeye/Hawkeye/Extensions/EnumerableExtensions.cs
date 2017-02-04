using System;
using System.Collections.Generic;

namespace FluentlyWindsor.Hawkeye.Extensions
{
	public static class EnumerableExtensions
    {
        public static int IndexOf<T>(this IEnumerable<T> items, Func<T, bool> eval)
        {
            int index = 0;
            foreach (var item in items)
            {
                if (eval(item))
                    return index;
                index++;
            }
            throw new Exception("Could not find item");
        }
    }
}