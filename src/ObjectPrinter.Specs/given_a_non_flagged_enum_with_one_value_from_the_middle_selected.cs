using System;
using Machine.Specifications;
using ObjectPrinter.TypeInspectors;

namespace ObjectPrinter.Specs
{
	[Subject(typeof(EnumTypeInspector))]
	public class given_a_non_flagged_enum_with_one_value_from_the_middle_selected
	{
		public enum ImNotFlagged
		{
			Val1, Val2, Val3
		}

		Establish context = () =>
		                    	{
									_value = ImNotFlagged.Val2;
		                    	};

		Because of = () =>
		             	{
		             		_output = _value.DumpToString();
		             		Console.Out.WriteLine(_output);
		             	};

		It should_not_print_Val1 = () => _output.ShouldNotContain("Val1");
		It should_print_Val2 = () => _output.ShouldContain("Val2");
		It should_not_print_Val3 = () => _output.ShouldNotContain("Val3");
		It should_not_print_namespace = () => _output.ShouldNotContain("ObjectPrinter.Specs");
		It should_print_enum_type_name = () => _output.ShouldContain("ImFlagged");

		static ImNotFlagged _value;
		static string _output;
	}
}