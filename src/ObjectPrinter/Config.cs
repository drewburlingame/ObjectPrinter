using System;
using System.Collections.Generic;
using System.Reflection;
using ObjectPrinter.TypeInspectors;
using ObjectPrinter.Utilties;

namespace ObjectPrinter
{
    /// <summary>
    /// The entry point for ObjectPrinter configuration
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// Configs defining inspectors
        /// </summary>
        public static class Inspectors
        {
            private static ITypeInspector _catchAllTypeInspector = new TypeInspectors.InspectAllTypeInspector();
            private static bool _includeCountsForCollections;
            private static readonly TypeInspectorsRegistration DefaultRegistration;
            private static TypeInspectorsRegistration _userProvidedRegistration;
            internal static IEnumerable<ITypeInspector> DefaultInspectors;

            static Inspectors()
            {
                DefaultRegistration = new TypeInspectorsRegistration();
                ReloadInspectors();
            }

            ///<summary>
            /// The catch-all inspector to use if no inspector is found to inspect a given type.
            ///</summary>
            public static ITypeInspector CatchAllTypeInspector
            {
                get { return _catchAllTypeInspector; }
                set
                {
                    if (value == null) throw new ArgumentNullException("value");
                    _catchAllTypeInspector = value;
                    ReloadInspectors();
                }
            }

            /// <summary>
            /// The registration that will be used to default ObjectPrinterConfig.Inspectors
            /// </summary>
            public static TypeInspectorsRegistration Registration
            {
                get { return _userProvidedRegistration ?? DefaultRegistration; }
                set
                {
                    if (value == null) throw new ArgumentNullException("value");
                    _userProvidedRegistration = value;
                    ReloadInspectors();
                }
            }

            /// <summary>
            /// The default setting for including counts for collections
            /// </summary>
            public static bool IncludeCountsForCollections
            {
                get { return _includeCountsForCollections; }
                set
                {
                    if(_includeCountsForCollections != value)
                    {
                        _includeCountsForCollections = value;
                        ReloadInspectors();
                    }
                }
            }

            private static void ReloadInspectors()
            {
                DefaultInspectors = Registration.GetRegisteredInspectors();
            }
        }

        /// <summary>
        /// Defaults for ObjectPrinterConfig
        /// </summary>
        public static class Printer
        {
            ///<summary>The default tab character</summary>
            public static string Tab = "\t";

            ///<summary>The default newline character</summary>
            public static string NewLine = Environment.NewLine;

            ///<summary>
            /// The default depth to recurse into the object graph.  Default is 10.
            /// This guards against self-referencing value types, like those found in Linq2Sql data definitions.
            ///</summary>
            public static int MaxDepth = 10;

            ///<summary>
            /// The default setting for including ObjectPrinter logging in the output
            ///</summary>
            public static bool IncludeLogging = false;

            /// <summary>
            /// The default setting for caching Exception output
            /// </summary>
            public static bool EnableExceptionCaching = false;
        }

        /// <summary>
        /// Configs for the InspectAllTypeInspector, the default CatchAllTypeInspector 
        /// </summary>
        public static class InspectAllTypeInspector
        {
            /// <summary>
            /// Used to override default caching mechanism when InspectAllTypeInspector.Default.EnableCaching is true.
            /// Any instance created will be reused per BindingFlags for the life of the AppDomain
            /// </summary>
            public static Func<BindingFlags, IMemberCache> GetMemberCacheDelegate = MemberCacheFactory.DefaultGetCacheDelegate;

            /// <summary>
            /// Defaults
            /// </summary>
            public static class Default
            {
                static Default()
                {
                    ObscureValueText = "{obscured}";
                }

                /// <summary>BindingFlags used to reflect members</summary>
                public static BindingFlags MemberBindingFlags = BindingFlags.Instance
                                                                | BindingFlags.Public
                                                                | BindingFlags.GetProperty
                                                                | BindingFlags.GetField;

                /// <summary>Enable caching of reflected values for each reflected type</summary>
                public static bool EnableCaching = true;

                /// <summary>Return methods in addition to properties and fields</summary>
                public static bool IncludeMethods = false;

                /// <summary>When ToString() is overridden, returns the value</summary>
                public static bool IncludeToStringWhenOverridden = true;

                /// <summary>When true, the value of the ObjectInfo is replaced with ObscureValueText</summary>
                public static Func<object, MemberInfo, ObjectInfo, bool> ShouldObscureValue { get; set; }

                /// <summary>The text to display when a value has been obscured.  default: {obscured}</summary>
                public static string ObscureValueText { get; set; }
            }
        }
    }
}