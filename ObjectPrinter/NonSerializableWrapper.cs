using System;
using System.Runtime.Serialization;

namespace ObjectPrinter
{
	[Serializable]
	public class NonSerializableWrapper : ISerializable
	{
		public object Context;

		[NonSerialized] 
		public bool PrintOnSerialize;

		public void GetObjectData(SerializationInfo info, StreamingContext context)
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