using System;
using System.Collections.Generic;
using System.Reflection;

namespace ObjectPrinter.Utilties
{
    public interface IMemberCache
    {
        IEnumerable<PropertyInfo> GetProperties(Type type);
        IEnumerable<FieldInfo> GetFields(Type type);
        IEnumerable<MethodInfo> GetMethods(Type type);
    }
}