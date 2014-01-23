using System;
using System.Collections.Generic;
using System.Reflection;
using ObjectPrinter.TypeInspectors;

namespace ObjectPrinter
{
    public static class Config
    {
        public static class Inspectors
        {
            private static ITypeInspector _catchAllTypeInspector = new TypeInspectors.InspectAllTypeInspector();
            private static TypeInspectorsRegistration _defaultRegistration;
            private static TypeInspectorsRegistration _userProvidedRegistration;
            internal static IEnumerable<ITypeInspector> DefaultInspectors;

            static Inspectors()
            {
                _defaultRegistration = new TypeInspectorsRegistration();
                ReloadInspectors();
            }

            ///<summary>
            /// The catch-all inspector to use if no inspector is found to inspect a given type.  
            /// "Default" as in the default in a switch statement.
            /// This will be appended to all configs.
            ///</summary>
            public static ITypeInspector CatchAllTypeInspector
            {
                get { return _catchAllTypeInspector; }
                set
                {
                    if (value == null) throw new ArgumentNullException("value");
                    _catchAllTypeInspector = value;
                    _defaultRegistration = new TypeInspectorsRegistration();
                    ReloadInspectors();
                }
            }

            /// <summary>
            /// The registration that will be used to default ObjectPrinterConfig.Inspectors
            /// </summary>
            public static TypeInspectorsRegistration Registration
            {
                get { return _userProvidedRegistration ?? _defaultRegistration; }
                set
                {
                    if (value == null) throw new ArgumentNullException("value");
                    _userProvidedRegistration = value;
                    ReloadInspectors();
                }
            }

            private static void ReloadInspectors()
            {
                DefaultInspectors = Registration.GetRegisteredInspectors();
            }
        }

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

        public static class InspectAllTypeInspector
        {
            public static class Default
            {
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
            }
        }
    }
}