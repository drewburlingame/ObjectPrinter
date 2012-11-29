using System;

namespace ObjectPrinter.Utilties
{
	/// <summary>
	/// A way to defer execution of the object printer until the last possible minute.
	/// Useful for logging to prevent littering code with IsLevelEnabled calls.
	/// </summary>
	internal class LazyString
	{
		readonly Func<string> _callback;

		public LazyString(Func<string> callback)
		{
			if (callback == null) throw new ArgumentNullException("callback");
			_callback = callback;
		}

		public override string ToString()
		{
			return _callback();
		}
	}
}