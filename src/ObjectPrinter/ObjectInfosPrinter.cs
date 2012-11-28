using System;
using System.Collections.Generic;
using ObjectPrinter.Utilties;

namespace ObjectPrinter
{
    internal class ObjectInfosPrinter : BaseEnumerablePrinter
    {
        public ObjectInfosPrinter(IndentableTextWriter output, Action<ObjectInfo> write)
            : base(output, write)
        {
        }

        private IEnumerable<ObjectInfo> _objToAppend;

        internal void Write(IEnumerable<ObjectInfo> objToAppend)
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

            foreach (var obj in _objToAppend)
            {
                WriteOne(obj);
            }
        }
    }
}