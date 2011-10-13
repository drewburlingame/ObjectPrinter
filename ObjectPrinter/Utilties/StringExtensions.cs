using System.Linq;
using System.Text;

namespace ObjectPrinter.Utilties
{
	public static class StringExtensions
	{
		public static string Repeat(this string value, int times)
		{
			return Enumerable.Repeat(value, times).JoinToString("");
		}
	}
}