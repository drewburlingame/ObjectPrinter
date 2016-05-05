using System;

namespace ObjectPrinter.Log4Net
{
    /// <summary>
    /// Extensions for types
    /// </summary>
	internal static class TypeExtensions
	{
	    /// <summary>
	    /// Determines if a type belongs to log4net or Common.Logging
	    /// </summary>
		internal static bool IsTypeFromLoggingFramework(this Type type)
		{
			var ns = type.Namespace;
	        return !string.IsNullOrEmpty(ns)
	               && (ns.StartsWith("log4net")
	                   || ns.StartsWith("Common.Logging"));
		}
	}
}