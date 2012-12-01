using System;
using System.Collections.Generic;

namespace ObjectPrinter.TypeInspectors
{
    public abstract class BaseTypedInspector<T> : ITypeInspector
    {
        protected abstract IEnumerable<ObjectInfo> OnGetMemberList(T objectToInspect, Type typeOfObjectToInspect);

        public bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect)
        {
            return typeof (T).IsAssignableFrom(typeOfObjectToInspect);
        }

        public IEnumerable<ObjectInfo> GetMemberList(object objectToInspect, Type typeOfObjectToInspect)
        {
            return OnGetMemberList((T) objectToInspect, typeOfObjectToInspect);
        }
    }
}