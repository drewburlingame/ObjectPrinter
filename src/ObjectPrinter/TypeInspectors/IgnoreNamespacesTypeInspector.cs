using System;
using System.Linq;

namespace ObjectPrinter.TypeInspectors
{
	/// <summary>
	/// returns the ToString() represenation for every object within one of the given namespaces.
	/// </summary>
	public class IgnoreNamespacesTypeInspector : ToStringTypeInspector
	{
		readonly string[] _namespaces;

		public IgnoreNamespacesTypeInspector(params string[] namespaces)
		{
			if (namespaces == null)
			{
				throw new ArgumentNullException("namespaces");
			}
			if (namespaces.Length == 0)
			{
				throw new ArgumentNullException("namespaces", "namespaces cannot be empty");
			}
			_namespaces = namespaces;
		}

		public override bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect)
		{
			return typeOfObjectToInspect.Namespace != null 
			       && _namespaces.Any(ns => typeOfObjectToInspect.Namespace.StartsWith(ns));
		}
	}
}