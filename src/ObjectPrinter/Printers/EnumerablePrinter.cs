using System;
using System.Collections;
using ObjectPrinter.Utilties;

namespace ObjectPrinter
{
    internal class EnumerablePrinter : BaseEnumerablePrinter
    {
        public EnumerablePrinter(IndentableTextWriter output, Action<ObjectInfo> write) 
            : base(output, write)
        {
        }

        private IEnumerable _objToAppend;

        internal void Write(IEnumerable objToAppend)
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
                WriteOne(null, obj);
            }
        }
    }
}