using System.Linq;
using System.Text;

namespace ObjectPrinter.Utilties
{
	internal static class StringExtensions
	{
		internal static string Repeat(this string value, int times)
		{
			return Enumerable.Repeat(value, times).JoinToString("");
		}
	}
}