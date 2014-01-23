using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ObjectPrinter.Utilties;

namespace ObjectPrinter.TypeInspectors
{
	public class InspectAllTypeInspector : ITypeInspector
	{
		/// <summary>If ShouldInspectType returns true, this inspector will handle the type.</summary>
		public Func<object, Type, bool> ShouldInspectType { get; set; }

		/// <summary>If ShouldEvaluateMember returns true, the value will be evaluated for the member. (EvaluateMember runs before IncludeMember)</summary>
		public Func<object, MemberInfo, bool> ShouldEvaluateMember { get; set; }

        /// <summary>If ShouldIncludeMember returns true (after member is evaluated), the member will be printed out. (EvaluateMember runs before IncludeMember)</summary>
		public Func<object, ObjectInfo, bool> ShouldIncludeMember { get; set; }

	    private IMemberCache _memberCache;
	    private BindingFlags _memberBindingFlags;
	    private bool _enableCaching;

        /// <summary>
        /// BindingFlags used to reflect members
        /// If not specified, Config.InspectAllTypeInspector.MemberBindingFlags (false) is used.
        /// </summary>
	    public BindingFlags MemberBindingFlags
	    {
	        get { return _memberBindingFlags; }
	        set
	        {
	            _memberBindingFlags = value;
                ResetCache();
	        }
	    }

        /// <summary>
        /// Enable caching of reflected values for each reflected type
        /// If not specified, Config.InspectAllTypeInspector.EnableCaching (false) is used.
        /// </summary>
        public bool EnableCaching
        {
            get { return _enableCaching; }
            set
            {
                _enableCaching = value;
                ResetCache();
            }
        }

        private void ResetCache()
        {
            _memberCache = MemberCacheFactory.Instance.Get(_enableCaching, _memberBindingFlags);
        }

        /// <summary>
        /// Return methods in addition to properties and fields
        /// If not specified, Config.InspectAllTypeInspector.IncludeMethods (false) is used.
        /// </summary>
        public bool IncludeMethods { get; set; }

        /// <summary>
        /// When ToString() is overridden, returns the value
        /// If not specified, Config.InspectAllTypeInspector.IncludeToStringWhenOverridden (false) is used.
        /// </summary>
		public bool IncludeToStringWhenOverridden { get; set; }

		public InspectAllTypeInspector()
		{
		    EnableCaching = Config.InspectAllTypeInspector.Default.EnableCaching;
			MemberBindingFlags = Config.InspectAllTypeInspector.Default.MemberBindingFlags;
			IncludeMethods = Config.InspectAllTypeInspector.Default.IncludeMethods;
			IncludeToStringWhenOverridden = Config.InspectAllTypeInspector.Default.IncludeToStringWhenOverridden;
		}

	    /// <summary>If ShouldInspect returns true, this type inspector will be used to inspect the type</summary>
		public virtual bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect)
		{
			return ShouldInspectType == null || ShouldInspectType(objectToInspect, typeOfObjectToInspect);
		}

        /// <summary>If ShouldEvaluate returns true, the value will be evaluated for the member</summary>
		protected virtual bool ShouldEvaluate(object instance, MemberInfo member)
		{
			return ShouldEvaluateMember == null || ShouldEvaluateMember(instance, member);
		}

        /// <summary>If ShouldInclude returns true (after member is evaluated), the member will be printed out</summary>
		protected virtual bool ShouldInclude(object instance, ObjectInfo info)
		{
			return ShouldIncludeMember == null || ShouldIncludeMember(instance, info);
		}

		public virtual IEnumerable<ObjectInfo> GetMemberList(object objectToInspect, Type typeOfObjectToInspect)
		{
			var type = objectToInspect.GetType();

		    var cache = _memberCache;

			var members = new List<ObjectInfo>();

			if (IncludeToStringWhenOverridden)
			{
				var toString = objectToInspect.ToString();
				if (string.CompareOrdinal(toString, objectToInspect.GetType().ToString()) != 0)
				{
					members.Add(new ObjectInfo("ToString()", toString));
				}
			}

		    members.AddRange(
		        cache.GetProperties(type)
		                    .Where(m => ShouldEvaluate(objectToInspect, m))
		                    .Select(p => new ObjectInfo(p.Name, ParsePropertyInfo(objectToInspect, p)))
		                    .Where(o => ShouldInclude(objectToInspect, o))
		        );

		    members.AddRange(
                cache.GetFields(type)
		                    .Where(m => ShouldEvaluate(objectToInspect, m))
		                    .Select(f => new ObjectInfo(f.Name, ParseFieldInfo(objectToInspect, f)))
		                    .Where(o => ShouldInclude(objectToInspect, o))
		        );

			if (IncludeMethods)
			{
			    members.AddRange(
                    cache.GetMethods(type)
			                    .Where(m => ShouldEvaluate(objectToInspect, m))
			                    .Select(m => new ObjectInfo(m.Name, ParseMethodInfo(objectToInspect, m)))
			                    .Where(o => ShouldInclude(objectToInspect, o))
			        );
			}

			return members;
		}

		protected virtual object ParseMethodInfo(object objectToInspect, MethodInfo methodInfo)
		{
			return methodInfo.ToString();
		}

		protected virtual object ParseFieldInfo(object objectToInspect, FieldInfo fieldInfo)
		{
			return ParseObjectValue(() => fieldInfo.GetValue(objectToInspect));
		}

		protected virtual object ParsePropertyInfo(object instance, PropertyInfo property)
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