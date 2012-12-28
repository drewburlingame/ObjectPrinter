using System;
using System.Collections.Generic;
using ObjectPrinter.Utilties;

namespace ObjectPrinter.Printers
{
    internal class ObjectInfosPrinter
    {
        private readonly IndentableTextWriter _output;
        private readonly Action<ObjectInfo> _write;

        public ObjectInfosPrinter(IndentableTextWriter output, Action<ObjectInfo> write)
        {
            if (output == null) throw new ArgumentNullException("output");
            if (write == null) throw new ArgumentNullException("write");
            _output = output;
            _write = write;
        }

        public void Write(IEnumerable<ObjectInfo> objToAppend)
        {
            //makes the print prettier.  needed because IEnumerable has no Count or Length property
            var isEmpty = true;

            foreach (var obj in objToAppend)
            {
                if (isEmpty)
                {
                    StartChildWrapper();
                    isEmpty = false;
                }
                _write(obj);
            }

            if (isEmpty)
            {
                WriteEmptyChildren();
            }
            else
            {
                EndChildWrapper();
            }
        }

        private void WriteEmptyChildren()
        {
            _output.Write("{}");
        }

        private void StartChildWrapper()
        {
            _output.WriteLine();
            _output.WriteLine("{");
            _output.Indent();
        }

        private void EndChildWrapper()
        {
            _output.Outdent();
            //no need to writeline because the object itself will 
            _output.Write("}");
        }
    }
}