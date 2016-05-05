using System.IO;
using ObjectPrinter.Utilties;
using log4net.ObjectRenderer;

namespace ObjectPrinter.Log4Net
{
    /// <summary>
    /// class used by log4net to render objects
    /// </summary>
	public class Log4NetObjectRenderer : IObjectRenderer
	{
		/// <summary></summary>
		public void RenderObject(RendererMap rendererMap, object obj, TextWriter writer)
		{
			if (obj == null)
			{
				return;
			}

			var objAsString = obj as string;
			if (obj is string)
			{
				writer.Write(objAsString);
				return;
			}

			if (obj is LazyString)
			{
				writer.Write(obj.ToString());
				return;
			}

			if (obj.GetType().IsTypeFromLoggingFramework())
			{
				//if we get a log4net object, log4net will expect us to call ToString to get the format
				writer.Write(obj.ToString());
				return;
			}

            obj.DumpTo(writer);
		}
	}
}