using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ObjectPrinter.TypeInspectors
{
	public class DefaultTypeInspector : ITypeInspector
	{
		public static BindingFlags DefaultMemberBindingFlags = BindingFlags.Instance
		                                                         | BindingFlags.Public
		                                                         | BindingFlags.GetProperty
		                                                         | BindingFlags.GetField;

		public static bool DefaultIncludeMethods = false;

		protected virtual BindingFlags MemberBindingFlags { get { return DefaultMemberBindingFlags; } }
		protected bool IncludeMethods { get { return DefaultIncludeMethods; } }
		protected bool IncludeToStringWhenOverridden { get { return true; } }

		public virtual bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect)
		{
			return true;
		}

		public virtual IEnumerable<ObjectInfo> GetMemberList(object objectToInspect, Type typeOfObjectToInspect)
		{
			var type = objectToInspect.GetType();
			BindingFlags bindingFlags = MemberBindingFlags;

			var members = new List<ObjectInfo>();

			if (IncludeToStringWhenOverridden)
			{
				var toString = objectToInspect.ToString();
				if (string.CompareOrdinal(toString, objectToInspect.GetType().ToString()) != 0)
				{
					members.Add(new ObjectInfo {Name = "ToString()", Value = toString, Inspector = null});
				}
			}

			members.AddRange(
				type.GetProperties(bindingFlags)
					.Select(p => new ObjectInfo
					             	{
					             		Name = p.Name,
					             		Value = ParsePropertyInfo(p, objectToInspect),
					             		Inspector = null
					             	})
				);

			members.AddRange(
				type.GetFields(bindingFlags)
					.Select(f => new ObjectInfo
					             	{
					             		Name = f.Name,
					             		Value = ParseFieldInfo(f, objectToInspect),
					             		Inspector = null
					             	})
				);

			if (IncludeMethods)
			{
				members.AddRange(
					type.GetMethods(bindingFlags)
						.Select(m => new ObjectInfo
						             	{
						             		Name = m.Name,
						             		Value = ParseMethodInfo(m, objectToInspect),
						             		Inspector = null
						             	})
					);
			}

			return members;
		}

		protected virtual object ParseMethodInfo(MethodInfo methodInfo, object objectToInspect)
		{
			return methodInfo.ToString();
		}

		protected virtual object ParseFieldInfo(FieldInfo fieldInfo, object objectToInspect)
		{
			return ParseObjectValue(() => fieldInfo.GetValue(objectToInspect));
		}

		protected virtual object ParsePropertyInfo(PropertyInfo property, object instance)
		{
			return ParseObjectValue(() => property.GetValue(instance, null));
		}

		private static object ParseObjectValue(Func<object> func)
		{
			object value;
			try
			{
				value = func();
			}
			catch (TargetInvocationException tie)
			{
				value = tie.InnerException is NotImplementedException
				        	? "{not implemented}"
				        	: "{not available}";
			}
			catch (NotImplementedException /* nie */)
			{
				value = "{not implemented}";
			}
			catch (Exception e)
			{
				value = "{encountered error: " + e.GetType().Name + "}";
				//throw new InvalidOperationException(instance.GetType() + " - " + property + Environment.NewLine, e);
			}
			return value;
		}

	}
}