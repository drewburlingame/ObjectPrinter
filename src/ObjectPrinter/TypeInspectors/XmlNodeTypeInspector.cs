using System;
using System.Collections.Generic;
using System.Xml;

namespace ObjectPrinter.TypeInspectors
{
    public class XmlNodeTypeInspector : BaseTypedInspector<XmlNode>
    {
        protected override IEnumerable<ObjectInfo> OnGetMemberList(XmlNode objectToInspect, Type typeOfObjectToInspect)
        {
            yield return new ObjectInfo{Value = objectToInspect.InnerXml};
        }
    }
}