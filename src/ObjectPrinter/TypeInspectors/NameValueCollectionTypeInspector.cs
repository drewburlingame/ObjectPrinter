using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ObjectPrinter.TypeInspectors
{
    public class NameValueCollectionTypeInspector : BaseTypedInspector<NameValueCollection>
    {
        protected override IEnumerable<ObjectInfo> OnGetMemberList(NameValueCollection objectToInspect, Type typeOfObjectToInspect)
        {
            for (int i = 0; i < objectToInspect.Count; i++)
            {
                yield return new ObjectInfo { Name = objectToInspect.Keys[i], Value = objectToInspect[i] };
            }
        }
    }
}