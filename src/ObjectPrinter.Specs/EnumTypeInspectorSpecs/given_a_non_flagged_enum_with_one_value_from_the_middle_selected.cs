using System;
using Machine.Specifications;
using ObjectPrinter.TypeInspectors;

namespace ObjectPrinter.Specs.EnumTypeInspectorSpecs
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

		It should_print_ImNotFlaggedDotVal2 = () => _output.ShouldEqual("ImNotFlagged.Val2");

		static ImNotFlagged _value;
		static string _output;
	}
}