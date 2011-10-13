using System;
using System.Text;

namespace ObjectPrinter.Utilties
{
	/// <summary>
	/// A container for a StringBuilder that keeps track of the tabs that should be added to the beginning of each line.
	/// This allows classes to append to a string without needing to know how many tabs to append.
	/// </summary>
	public class IndentAwareStringBuilder
	{
		private StringBuilder _innerStringBuilder = new StringBuilder(1500);
		private readonly string _tab;
		private readonly string _newline;
		private readonly char[] _newlineArray;
		private string _indent;
		private string _newlineReplacement;
		private bool _lineAlreadyStarted;

		private int TabDepth { get; set; }

		/// <summary>Instantiates an IndentAwareStringBuilder with default tab and newline values</summary>
		public IndentAwareStringBuilder()
			: this("\t", Environment.NewLine)
		{
		}

		/// <summary>Instantiates an IndentAwareStringBuilder with provided tab and newline values</summary>
		public IndentAwareStringBuilder(string tab, string newline)
		{
			_tab = tab;
			_newline = newline;
			_newlineArray = _newline.ToCharArray();
		}

		/// <summary>Gets or sets the length of the current IndentAwareStringBuilder</summary>
		public int Length
		{
			get { return _innerStringBuilder.Length; }
			set { _innerStringBuilder.Length = value; }
		}

		/// <summary>Starts a new line in the StringBuilder.</summary>
		public void StartLine()
		{
			if (_lineAlreadyStarted)
			{
				EndLine();
			}
			_innerStringBuilder.Append(_indent);
			_lineAlreadyStarted = true;
		}

		/// <summary>Appends the provided value to the end of the current line and then ends the current line.</summary>
		public void EndLineWith(string value)
		{
			Append(value);
			EndLine();
		}

		/// <summary>Ends the current line.</summary>
		public void EndLine()
		{
			if (LastEntryIsNotNewLine())
			{
				_innerStringBuilder.Append(_newline);
			}
			_lineAlreadyStarted = false;
		}

		private bool LastEntryIsNewLine()
		{
			if (_innerStringBuilder.Length == 0)
				return false;

			return !LastEntryIsNotNewLineImpl();
		}

		private bool LastEntryIsNotNewLine()
		{
			if (_innerStringBuilder.Length == 0)
				return false;

			return LastEntryIsNotNewLineImpl();
		}

		private bool LastEntryIsNotNewLineImpl()
		{
			//optimization for typical use case where newline is /n
			//this avoids the cost of the loop
			if (_newlineArray.Length == 1)
			{
				return _newlineArray[0] != _innerStringBuilder[_innerStringBuilder.Length - 1];
			}

			for (int i = 1; i <= _newlineArray.Length; i++)
			{
				if (_newlineArray[_newlineArray.Length - i] != _innerStringBuilder[_innerStringBuilder.Length - i])
					return true;
			}
			return false;
		}

		/// <summary>Appends the value to the current line</summary>
		public void Append(object value)
		{
			if (value == null)
			{
				return;
			}
			if (_innerStringBuilder.Length == 0 || LastEntryIsNewLine())
			{
				StartLine();
			}
			_innerStringBuilder.Append(value.ToString().Replace(_newline, _newlineReplacement));
		}

		/// <summary>Creates a new line, appends the value and then ends that line.</summary>
		public void AppendLine(object value)
		{
			StartLine();
			Append(value);
			EndLine();
		}

		/// <summary>Adds a blank line</summary>
		public void AppendBlankLine()
		{
			EndLine();
			AppendLine(String.Empty);
		}

		/// <summary>Adds 1 to TabDepth</summary>
		public void IncrementTabDepth()
		{
			TabDepth++;
			SetIndentString();
		}

		/// <summary>Subtracts 1 to TabDepth</summary>
		public void DecrementTabDepth()
		{
			TabDepth--;
			SetIndentString();
		}

		private void SetIndentString()
		{
			_indent = _tab.Repeat(TabDepth);
			_newlineReplacement = _newline + _tab + _indent;
		}

		/// <summary>Returns a <see cref="String"/> that represents the current IndentAwareStringBuilder</summary>
		public override string ToString()
		{
			return _innerStringBuilder.ToString();
		}
	}
}