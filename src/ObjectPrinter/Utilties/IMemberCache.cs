using System;
using System.Collections.Generic;
using System.Reflection;

namespace ObjectPrinter.Utilties
{
    /// <summary>
    /// Caches the member information used to inspect a type
    /// </summary>
    public interface IMemberCache
    {
        /// <summary>
        /// Returns the properties to inspect
        /// </summary>
        IEnumerable<PropertyInfo> GetProperties(Type type);

        /// <summary>
        /// Returns the fields to inspect
        /// </summary>
        IEnumerable<FieldInfo> GetFields(Type type);

        /// <summary>
        /// Returns the methods to inspect
        /// </summary>
        IEnumerable<MethodInfo> GetMethods(Type type);
    }
}