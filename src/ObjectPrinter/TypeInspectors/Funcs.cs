using System;
using System.Linq;
using ObjectPrinter.Utilties;

namespace ObjectPrinter.TypeInspectors
{
	public static class Funcs
	{
		public static Func<object, Type, bool> IsException
		{
			get { return (o, type) => typeof (Exception).IsAssignableFrom(type); }
		}

		public static Func<object, Type, bool> IncludeMsBuiltInNamespaces
		{
			get { return IncludeNamespaces("System", "Microsoft"); }
		}

		public static Func<object, Type, bool> IncludeNamespaces(params string[] namespaces)
		{
			if (namespaces.IsNullOrEmpty())
			{
				throw new ArgumentException("namespaces cannot be null or empty");
			}

			if (namespaces.Length == 1) //optimization
			{
				string ns = namespaces[0];
				return (o, type) => IsTheNamespaceYoureLookingFor(ns, type);
			}

			return (o, type) => namespaces.Any(ns => IsTheNamespaceYoureLookingFor(ns, type));
		}

		public static Func<object, Type, bool> ExcludeMsBuiltInNamespaces
		{
			get { return ExcludeNamespaces("System", "Microsoft"); }
		}

		public static Func<object, Type, bool> ExcludeNamespaces(params string[] namespaces)
		{
			if (namespaces.IsNullOrEmpty())
			{
				throw new ArgumentException("namespaces cannot be null or empty");
			}

			if (namespaces.Length == 1) //optimization
			{
				string ns = namespaces[0];
				return (o, type) => !IsTheNamespaceYoureLookingFor(ns, type);
			}

			return (o, type) => !namespaces.Any(ns => IsTheNamespaceYoureLookingFor(ns, type));
		}

		static bool IsTheNamespaceYoureLookingFor(string ns, Type type)
		{
			return string.IsNullOrEmpty(type.Namespace) ? ns == string.Empty : type.Namespace.StartsWith(ns);
		}

		public static Func<object, ObjectInfo, bool> ExcludeNullMembers
		{
			get { return (o, info) => info.Value != null; }
		}
	}
}