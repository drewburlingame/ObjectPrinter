using System;
using System.Collections;
using System.Collections.Generic;

namespace ObjectPrinter.Utilties
{
	internal static class EnumerableExtensions
	{
		internal static string JoinToString(this IEnumerable enumerable, string separator = ",")
		{
		    return string.Join(separator, enumerable);
		}

		internal static string JoinToString<T>(this IEnumerable<T> enumerable, string separator = ",")
        {
            return string.Join(separator, enumerable);
		}

		internal static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
		{
			return enumerable == null || enumerable.IsEmpty();
		}

		internal static bool IsEmpty<T>(this IEnumerable<T> enumerable)
		{
			return !enumerable.GetEnumerator().MoveNext();
		}

		internal static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			foreach (var item in enumerable)
			{
				action(item);
			    yield return item;
			}
		}
	}
}