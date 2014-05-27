using System;
using System.IO;
using System.Text;

namespace ObjectPrinter.Utilties
{
    /// <summary>
    /// A text writer that can remember indents across new lines
    /// </summary>
    public class IndentableTextWriter : TextWriter
    {
        private readonly TextWriter _innerWriter;
        private readonly string _tab;
        private readonly string _newline;

        private int _tabDepth;
        private bool _tabsNext;
        private bool _lastCharacterWasCarriageReturn;

        ///<summary></summary>
        public IndentableTextWriter(TextWriter innerWriter)
            : this(innerWriter, "\t", Environment.NewLine)
        {
        }

        ///<summary></summary>
        public IndentableTextWriter(TextWriter innerWriter, string tab, string newline)
        {
            _innerWriter = innerWriter;
            _tab = tab;
            _newline = newline;
        }

        ///<summary>Add an indent to be prepended to every line</summary>
        public void Indent()
        {
            _tabDepth++;
        }

        ///<summary>Remove an indent to be prepended to every line</summary>
        public void Outdent()
        {
            _tabDepth--;
        }

        ///<summary>Write this value</summary>
        public override void Write(char value)
        {
            if (value == '\r')
            {
                _lastCharacterWasCarriageReturn = true;
                //we're overriding the newline so ignore carriage returns
                return;
            }

            if (value == '\n')
            {
                WriteNewLine();
            }
            else
            {
                if (_lastCharacterWasCarriageReturn)
                {
                    WriteNewLine();
                }
                if (_tabsNext)
                {
                    WriteTabs();
                } 
                _innerWriter.Write(value);
            }
        }

        private void WriteNewLine()
        {
            _innerWriter.Write(_newline);
            _tabsNext = true;
            _lastCharacterWasCarriageReturn = false;
        }

        private void WriteTabs()
        {
            for (int i = 0; i < _tabDepth; i++)
            {
                _innerWriter.Write(_tab);
            }
            _tabsNext = false;
        }

        ///<summary>The encoding to use</summary>
        public override Encoding Encoding
        {
            get { return _innerWriter.Encoding; }
        }
    }
}