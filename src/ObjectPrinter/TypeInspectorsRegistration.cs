using System;
using System.Collections.Generic;
using ObjectPrinter.TypeInspectors;

namespace ObjectPrinter
{
    /// <summary>
    /// Use this class to customize type inspector registration.
    /// Order inspectors will be applied:
    /// - Enum
    /// - Exception
    /// - Enumerables
    /// - Custom
    /// - CatchAll
    /// </summary>
    public class TypeInspectorsRegistration
    {
        readonly List<ITypeInspector> inspectors = new List<ITypeInspector>();

        /// <summary>in order: XmlNode, Dictionar, NameValueCollection & Enumerable</summary>
        public static IEnumerable<ITypeInspector> DefaultEnumerableInspectors = new ITypeInspector[]
            {
                new XmlNodeTypeInspector(), 
                new DictionaryTypeInspector(), 
                new NameValueCollectionTypeInspector(), 
                new EnumerableTypeInspector(), 
            };

        private ITypeInspector _enumTypeInspector;
        private ITypeInspector _exceptionTypeInspector;
        private ITypeInspector[] _enumerableTypeInspectors;

        private bool _inspectAllMsTypes;

        public TypeInspectorsRegistration OverrideEnumInspector(ITypeInspector enumTypeInspector)
        {
            _enumTypeInspector = enumTypeInspector;
            return this;
        }

        public TypeInspectorsRegistration OverrideExceptionInspector(ITypeInspector exceptionTypeInspector)
        {
            _exceptionTypeInspector = exceptionTypeInspector;
            return this;
        }

        /// <summary>DefaultEnumerableInspectors is used if not overridden</summary>
        public TypeInspectorsRegistration OverrideEnumerableInspectors(params ITypeInspector[] enumerableTypeInspectors)
        {
            _enumerableTypeInspectors = enumerableTypeInspectors;
            return this;
        }

        ///<summary>
        /// If not specified, ToString() is called on all types in 'System' and 'Microsoft' 
        /// namespaces not already covered by a type inspector.  Calling this will cause
        /// those types to be inspected by the CatchAll inspector
        ///</summary>
        public TypeInspectorsRegistration InspectAllMsTypes()
        {
            _inspectAllMsTypes = true;
            return this;
        }

        /// <summary>Inspectors to be used run before the CatchAll inspector</summary>
        public TypeInspectorsRegistration RegisterInspector(ITypeInspector inspector)
        {
            if (inspector == null) throw new ArgumentNullException("inspector");
            inspectors.Add(inspector);
            return this;
        }

        public IEnumerable<ITypeInspector> GetRegisteredInspectors()
        {
            yield return _enumTypeInspector ?? new EnumTypeInspector();
            yield return _exceptionTypeInspector ?? new ExceptionTypeInspector();

            foreach (var inspector in _enumerableTypeInspectors ?? DefaultEnumerableInspectors)
            {
                yield return inspector;
            }

            foreach (var inspector in inspectors)
            {
                yield return inspector;
            }

            if (!_inspectAllMsTypes)
            {
                yield return new ToStringTypeInspector {ShouldInspectType = Funcs.IncludeMsBuiltInNamespaces};
            }

            yield return ObjectPrinterConfig.CatchAllTypeInspector;
        }
    }
}