using System;
using ObjectPrinter.Utilties;

namespace ObjectPrinter
{
    internal abstract class BaseEnumerablePrinter
    {
        private readonly IndentableTextWriter _output;
        private readonly Action<ObjectInfo> _write;
        private bool _isEmpty;

        protected BaseEnumerablePrinter(IndentableTextWriter output, Action<ObjectInfo> write)
        {
            if (output == null) throw new ArgumentNullException("output");
            if (write == null) throw new ArgumentNullException("write");
            _output = output;
            _write = write;
        }

        protected void WriteEmptyChildren()
        {
            _output.WriteLine("{}");
        }

        protected void StartChildWrapper()
        {
            _output.WriteLine();
            _output.WriteLine("{");
            _output.Indent();
        }

        protected void EndChildWrapper()
        {
            _output.Outdent();
            //no need to writeline because the object itself will 
            _output.Write("}");
        }

        protected abstract void Traverse();

        protected void WriteOne(string key, object value)
        {
            WriteOne(new ObjectInfo { Name = key, Value = value });
        }

        protected void WriteOne(ObjectInfo objectInfo)
        {
            if (_isEmpty)
            {
                StartChildWrapper();
                _isEmpty = false;
            }
            _write(objectInfo);
        }

        protected void WriteAll()
        {
            //makes the print prettier.  needed because IEnumerable has no Count or Length property
            _isEmpty = true;

            Traverse();

            if (_isEmpty)
            {
                WriteEmptyChildren();
            }
            else
            {
                EndChildWrapper();
            }
        }
    }
}