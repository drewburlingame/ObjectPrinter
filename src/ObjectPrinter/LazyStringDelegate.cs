using System;

namespace ObjectPrinter
{
	/// <summary>
	/// A way to defer execution of the object printer until the last possible minute.
	/// Useful for logging to prevent littering code with IsLevelEnabled calls.
	/// </summary>
	public class LazyStringDelegate
	{
		readonly Func<string> _callback;

		public LazyStringDelegate(Func<string> callback)
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