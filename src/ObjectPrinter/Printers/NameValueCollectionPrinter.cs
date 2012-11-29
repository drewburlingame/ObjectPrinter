using System;
using System.Collections.Specialized;
using ObjectPrinter.Utilties;

namespace ObjectPrinter
{
    internal class NameValueCollectionPrinter : BaseEnumerablePrinter
    {
        public NameValueCollectionPrinter(IndentableTextWriter output, Action<ObjectInfo> write)
            : base(output, write)
        {
        }

        private NameValueCollection _objToAppend;

        internal void Write(NameValueCollection objToAppend)
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

            for (int i = 0; i < _objToAppend.Count; i++)
            {
                WriteOne(_objToAppend.Keys[i], _objToAppend[i]);
            }
        }
    }
}