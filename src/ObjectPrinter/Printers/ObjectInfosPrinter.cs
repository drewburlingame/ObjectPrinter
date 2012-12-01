using System;
using System.Collections.Generic;
using ObjectPrinter.Utilties;

namespace ObjectPrinter.Printers
{
    internal class ObjectInfosPrinter
    {
        private readonly IndentableTextWriter _output;
        private readonly Action<ObjectInfo> _write;
        private bool _isEmpty;
        private IEnumerable<ObjectInfo> _objToAppend;

        public ObjectInfosPrinter(IndentableTextWriter output, Action<ObjectInfo> write)
        {
            if (output == null) throw new ArgumentNullException("output");
            if (write == null) throw new ArgumentNullException("write");
            _output = output;
            _write = write;
        }

        internal void Write(IEnumerable<ObjectInfo> objToAppend)
        {
            _objToAppend = objToAppend;
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

        protected void Traverse()
        {
            if (_objToAppend == null)
            {
                return;
            }

            foreach (var obj in _objToAppend)
            {
                if (_isEmpty)
                {
                    StartChildWrapper();
                    _isEmpty = false;
                }
                _write(obj);
            }
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
    }
}