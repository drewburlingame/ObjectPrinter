using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ObjectPrinter.TypeInspectors
{
    public class DictionaryTypeInspector : BaseTypedInspector<IDictionary>
    {
        protected override IEnumerable<ObjectInfo> OnGetMemberList(IDictionary objectToInspect, Type typeOfObjectToInspect)
        {
            foreach (DictionaryEntry entry in objectToInspect)
            {
                yield return new ObjectInfo(entry.Key.ToString(), entry.Value);
            }
        }
    }
}