using System;
using System.Runtime.Serialization;

namespace ObjectPrinter.Utilties
{
	/// <summary>
    /// A NonSerializableWrapper is a wrapper to contain non-serializable objects within Exception.Data
    /// which requires all items to be serializable.  
    /// If the item is serialized and PrintOnSerialize is true, the object is converted
    /// to a string using the ObjectPrinter otherwise the item is lost
	/// </summary>
	[Serializable]
	public class NonSerializableWrapper : ISerializable
	{
		/// <summary>
		/// The wrapped item
		/// </summary>
		public object Context;

		/// <summary>
		/// determines whether Context is dumped or dropped
		/// </summary>
		[NonSerialized] 
		public bool PrintOnSerialize;

	    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}

			if (Context == null)
			{
				info.AddValue("Context", null);
			}
			else if(PrintOnSerialize)
			{
				info.AddValue("Context", Context.DumpToString());
			}
			else
			{
				info.AddValue("Context", "Unable to serialize object of type: " + Context.GetType());
			}
		}
	}
}