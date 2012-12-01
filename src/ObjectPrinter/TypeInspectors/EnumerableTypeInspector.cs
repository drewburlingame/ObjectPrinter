using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ObjectPrinter.TypeInspectors
{
    public class EnumerableTypeInspector : BaseTypedInspector<IEnumerable>
    {
        protected override IEnumerable<ObjectInfo> OnGetMemberList(IEnumerable objectToInspect, Type typeOfObjectToInspect)
        {
            return from Object obj in objectToInspect
                   select new ObjectInfo { Value = obj };
        }
    }
}