using System;
using System.Collections.Generic;
using System.Xml;

namespace ObjectPrinter.TypeInspectors
{
    /// <summary>
    /// returns the InnerXml of the node as a string
    /// </summary>
    public class XmlNodeTypeInspector : ITypeInspector
    {
        ///<summary></summary>
        public bool ShouldInspect(object objectToInspect, Type typeOfObjectToInspect)
        {
            return typeof(XmlNode).IsAssignableFrom(typeOfObjectToInspect);
        }

        ///<summary></summary>
        public IEnumerable<ObjectInfo> GetMemberList(object objectToInspect, Type typeOfObjectToInspect)
        {
            yield return new ObjectInfo(((XmlNode)objectToInspect).InnerXml);
        }
    }
}