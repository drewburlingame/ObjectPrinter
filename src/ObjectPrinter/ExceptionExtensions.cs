using System;
using ObjectPrinter.Utilties;

namespace ObjectPrinter
{
	public static class ExceptionExtensions
	{
        /// <summary>
        /// Adds context in the form of objects to the Data property of an exception.
        /// This context will be printed with the exception.
        /// </summary>
        /// <param name="e">The exception to add context to</param>
        /// <param name="name">The name to describe the context</param>
        /// <param name="context">The context item.</param>
        /// <param name="ifNotSerializablePrintOnSerialize">
        /// When true, if the object is not serializable, the object will be dumped 
        /// to string and added to the exception before serialization.  default=true
        /// </param>
	    public static void SetContext(this Exception e, string name, object context, bool ifNotSerializablePrintOnSerialize = true)
		{
			if (context != null && !context.GetType().IsSerializable)
			{
				context = new NonSerializableWrapper {Context = context, PrintOnSerialize = ifNotSerializablePrintOnSerialize};
			}
			e.Data[name] = context;
		}
	}
}