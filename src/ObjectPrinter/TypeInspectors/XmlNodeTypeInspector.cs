using System;
using System.Collections.Generic;
using System.Xml;

namespace ObjectPrinter.TypeInspectors
{
    public class XmlNodeTypeInspector : ITypeInspector
    {
        public bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect)
        {
            return typeof(XmlNode).IsAssignableFrom(typeOfObjectToInspect);
        }

        public IEnumerable<ObjectInfo> GetMemberList(object objectToInspect, Type typeOfObjectToInspect)
        {
            yield return new ObjectInfo(((XmlNode)objectToInspect).InnerXml);
        }
    }
}