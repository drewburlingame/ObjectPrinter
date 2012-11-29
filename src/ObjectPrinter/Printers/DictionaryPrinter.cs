using System;
using System.Collections;
using ObjectPrinter.Utilties;

namespace ObjectPrinter.Printers
{
    internal class DictionaryPrinter : BaseEnumerablePrinter
    {
        public DictionaryPrinter(IndentableTextWriter output, Action<ObjectInfo> write)
            : base(output, write)
        {
        }

        private IDictionary _objToAppend;

        internal void Write(IDictionary objToAppend)
        {
            _objToAppend = objToAppend;
            WriteAll();
        }

        protected override void Traverse()
        {
            if (_objToAppend == null)
            {
                return;
            }

            foreach (DictionaryEntry obj in _objToAppend)
            {
                WriteOne(obj.Key.ToString(), obj.Value);
            }
        }
    }
}