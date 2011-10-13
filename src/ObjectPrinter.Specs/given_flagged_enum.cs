

using System;
using Machine.Specifications;
using ObjectPrinter.TypeInspectors;

namespace ObjectPrinter.Specs
{
	[Subject(typeof(EnumTypeInspector))]
	public class given_flagged_enum_value_with_both_Val1_and_Val2
	{
		[Flags]
		public enum ImFlagged
		{
			Val1, Val2, Val3
		}

		Establish context = () =>
		                    	{
		                    		_flagged = ImFlagged.Val1 | ImFlagged.Val2; 
		                    	};

		Because of = () =>
		             	{
		             		_output = _flagged.DumpToString();
		             		Console.Out.WriteLine(_output);
		             	};

		It should_print_Val1 = () => _output.ShouldContain("Val1");
		It should_print_Val2 = () => _output.ShouldContain("Val2");
		It should_not_print_namespace = () => _output.ShouldNotContain("ObjectPrinter.Specs");
		It should_print_enum_type_name = () => _output.ShouldContain("ImFlagged");

		static ImFlagged _flagged;
		static string _output;
	}
}