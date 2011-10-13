using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ObjectPrinter.Utilties
{
	public static class EnumerableExtensions
	{
		public static string JoinToString(this IEnumerable enumerable, string separator = ",")
		{
			var sb = new StringBuilder();
			foreach (var o in enumerable)
			{
				if (o != null)
				{
					sb.Append(o);
				}
				sb.Append(separator);
			}
			return sb.ToString();
		}

		public static string JoinToString<T>(this IEnumerable<T> enumerable, string separator = ",")
		{
			var sb = new StringBuilder();
			foreach (var o in enumerable)
			{
				if (o != null)
				{
					sb.Append(o);
				}
				sb.Append(separator);
			}
			sb.Length = sb.Length - separator.Length;
			return sb.ToString();
		}

		public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
		{
			return enumerable == null || enumerable.IsEmpty();
		}

		public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
		{
			return !enumerable.GetEnumerator().MoveNext();
		}

		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			foreach (var item in enumerable)
			{
				action(item);
			}

			return enumerable;
		}
	}
}