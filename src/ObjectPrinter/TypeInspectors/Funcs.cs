using System;
using System.Linq;
using System.Reflection;
using ObjectPrinter.Utilties;

namespace ObjectPrinter.TypeInspectors
{
	public static class Funcs
	{
        /// <summary>typeof (Exception).IsAssignableFrom(type)</summary>
		public static Func<object, Type, bool> IsException
		{
			get { return (o, type) => typeof (Exception).IsAssignableFrom(type); }
		}

        /// <summary>IncludeNamespaces("System", "Microsoft")</summary>
		public static Func<object, Type, bool> IncludeMsBuiltInNamespaces
		{
			get { return IncludeNamespaces("System", "Microsoft"); }
		}

        /// <summary>ExcludeNamespaces("System", "Microsoft")</summary>
		public static Func<object, Type, bool> ExcludeMsBuiltInNamespaces
		{
			get { return ExcludeNamespaces("System", "Microsoft"); }
		}

	    /// <summary>includes any namespace starting with one of the namespaces provided</summary>
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

        /// <summary>Excludes any namespace starting with one of the namespaces provided</summary>
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

		public static Func<object, Type, bool> IncludeTypes(bool inherit, params Type[] types)
		{
			if (types.IsNullOrEmpty())
			{
				throw new ArgumentException("types cannot be null or empty");
			}

			if (types.Length == 1) //optimization
			{
				return (o, type) => IsTheTypeYoureLookingFor(types[0], type, inherit);
			}

			return (o, type) => types.Any(t => IsTheTypeYoureLookingFor(t, type, inherit));
		}

		public static Func<object, Type, bool> ExcludeTypes(bool inherit, params Type[] types)
		{
			if (types.IsNullOrEmpty())
			{
				throw new ArgumentException("types cannot be null or empty");
			}

			if (types.Length == 1) //optimization
			{
				return (o, type) => !IsTheTypeYoureLookingFor(types[0], type, inherit);
			}

			return (o, type) => !types.Any(t => IsTheTypeYoureLookingFor(t, type, inherit));
		}

		static bool IsTheTypeYoureLookingFor(Type typeToFind, Type type, bool inherit)
		{
            return inherit ? typeToFind.IsAssignableFrom(type) : typeToFind == type;
		}

		public static Func<object, ObjectInfo, bool> ExcludeNullMembers
		{
			get { return (o, info) => info.Value != null; }
		}

        /// <summary>
        /// returns true if MemberInfo.name is contains any of the following strings, 
        /// ignoring case: password, pwd, salt, connectionstring, connstring
        /// </summary>
        public static Func<MemberInfo, bool> MemberContainsPassword
        {
            get
            {
                return (memberInfo) =>
                       memberInfo.Name.IndexOf("password", StringComparison.OrdinalIgnoreCase) >= 0
                       || memberInfo.Name.IndexOf("pwd", StringComparison.OrdinalIgnoreCase) >= 0
                       || memberInfo.Name.IndexOf("salt", StringComparison.OrdinalIgnoreCase) >= 0
                       || memberInfo.Name.IndexOf("connectionstring", StringComparison.OrdinalIgnoreCase) >= 0
                       || memberInfo.Name.IndexOf("connstring", StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }
	}
}