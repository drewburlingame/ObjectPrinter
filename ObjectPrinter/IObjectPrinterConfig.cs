using System;
using ObjectPrinter.TypeInspectors;

namespace ObjectPrinter
{
	public interface IObjectPrinterConfig
	{
		string Tab { get; }
		string NewLine { get; }
		int MaxDepth { get; }
		ITypeInspector GetInspector(object objectToInspect, Type typeOfObjectToInspect);
	}
}