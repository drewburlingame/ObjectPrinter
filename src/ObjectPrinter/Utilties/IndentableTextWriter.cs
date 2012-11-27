using System;
using System.IO;
using System.Text;

namespace ObjectPrinter.Utilties
{
    public class IndentableTextWriter : TextWriter
    {
        private readonly TextWriter _innerWriter;
        private readonly string _tab;
        private readonly string _newline;

        private int _tabDepth;
        private bool _tabsNext;

        public IndentableTextWriter(TextWriter innerWriter)
            : this(innerWriter, "\t", Environment.NewLine)
        {
        }

        public IndentableTextWriter(TextWriter innerWriter, string tab, string newline)
        {
            _innerWriter = innerWriter;
            _tab = tab;
            _newline = newline;
        }

        public void Indent()
        {
            _tabDepth++;
        }

        public void Outdent()
        {
            _tabDepth--;
        }

        public override void Write(char value)
        {
            if (value == '\r')
            {
                //we're overriding the newline so ignore carriage returns
                return;
            }
            if (value == '\n')
            {
                _innerWriter.Write(_newline);
                _tabsNext = true;
            }
            else
            {
                if (_tabsNext)
                {
                    WriteTabs();
                }
                _innerWriter.Write(value);
            }
        }

        private void WriteTabs()
        {
            if (_tabsNext)
            {
                for (int i = 0; i < _tabDepth; i++)
                {
                    _innerWriter.Write(_tab);
                }
                _tabsNext = false;
            }
        }

        public override Encoding Encoding
        {
            get { return _innerWriter.Encoding; }
        }
    }
}