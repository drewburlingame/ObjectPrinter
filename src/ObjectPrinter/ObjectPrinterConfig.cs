using System;
using System.Collections.Generic;
using System.Linq;
using ObjectPrinter.Log4Net;
using ObjectPrinter.TypeInspectors;

namespace ObjectPrinter
{
	public class ObjectPrinterConfig : IObjectPrinterConfig
	{
		public static ITypeInspector DefaultTypeInspector = new InspectAllTypeInspector();

		public static string DefaultTab = "\t";
		public static string DefaultNewLine = Environment.NewLine;
		public static bool DefaultIncludeLogging = false;

		private static ITypeInspector[] _inspectorsWithAllTypesInspected = new ITypeInspector[]
		            {
		                new EnumTypeInspector(),
		                new ExceptionTypeInspector(),
		                new Log4NetTypeInspector(),
		                DefaultTypeInspector
		            };

		private static ITypeInspector[] _inspectorsWithMsTypesToStringed = new ITypeInspector[]
			      	{
			      		new EnumTypeInspector(),
			      		new ExceptionTypeInspector(),
						new Log4NetTypeInspector(),
			      		new ToStringTypeInspector { ShouldInspectType = Funcs.IncludeMsBuiltInNamespaces },
			      		DefaultTypeInspector
			      	};

		public static IEnumerable<ITypeInspector> InspectorsWithAllTypesInspected
		{
			get { return _inspectorsWithAllTypesInspected; }
		}

		public static IEnumerable<ITypeInspector> InspectorsWithMsTypesToStringed
		{
			get { return _inspectorsWithMsTypesToStringed; }
		}

		/// <summary>
		/// Delegate to return the inspectors to use and the order to use them in.
		/// Change this at runtime to update the inspectors used and their order.
		/// </summary>
		public static Func<IEnumerable<ITypeInspector>> GetInspectors = () => InspectorsWithMsTypesToStringed;

		public string Tab { get; set; }
		public string NewLine { get; set; }
		public int MaxDepth { get; set; }
		public bool IncludeLogging { get; set; }

		/// <summary>
		/// The type inspectors to be used during the printing of an object.
		/// It will generally be easier to update the GetInspectors static factory delegate.
		/// </summary>
		public IEnumerable<ITypeInspector> Inspectors { get; set; }

		public ObjectPrinterConfig()
		{
			Tab = DefaultTab;
			NewLine = DefaultNewLine;
			MaxDepth = 10;
			IncludeLogging = DefaultIncludeLogging;
			Inspectors = GetInspectors();
		}

		public virtual ITypeInspector GetInspector(object objectToInspect, Type typeOfObjectToInspect)
		{
			return Inspectors
			       	.FirstOrDefault(inspector => inspector.ShouldInspect(objectToInspect, typeOfObjectToInspect))
			       ?? DefaultTypeInspector;
		}
	}
}