using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace ObjectPrinter.Specs
{
	[TestFixture]
	public class ObjectPrinterTests
	{
		[Test]
		public void SmokeTest_general_use_case_including_recursion()
		{
			var child1 = new ObjectPrintable
							{
								String = "Child 1",
								Id = 1
							};
			var child2 = new ObjectPrintable
							{
								String = "Child 2",
								Id = 2
							};

			var parent = new ObjectPrintable("Whos your daddy?")
							{
								String = "Parent",
								Id = 100,
								ObjectPrintableChild = child1,
								ObjectPrintableArray = new[] { child1, child2 },
								ObjectPrintableDictionary = new Dictionary<int, ObjectPrintable> { { child1.Id, child1 }, { child2.Id, child2 } }
							};

			child1.ObjectPrintableParent = parent;
			child2.ObjectPrintableParent = parent;

			var actual = parent.DumpToString();
			string expected = @"[ObjectPrintable]: hashcode { 100 }
{
	ToString() : Whos your daddy?
	String : Parent
	Id : 100
	ObjectPrintableChild : [ObjectPrintable]: hashcode { 1 }
	{
		String : Child 1
		Id : 1
		ObjectPrintableChild : {NULL}
		ObjectPrintableParent : avoid circular loop for this [ObjectPrintable]: hashcode { 100 }
		ObjectPrintableDictionary : {NULL}
		ObjectPrintableArray : {NULL}
	}
	ObjectPrintableParent : {NULL}
	ObjectPrintableDictionary : 
	{
		1 : [ObjectPrintable]: hashcode { 1 }
		{
			String : Child 1
			Id : 1
			ObjectPrintableChild : {NULL}
			ObjectPrintableParent : avoid circular loop for this [ObjectPrintable]: hashcode { 100 }
			ObjectPrintableDictionary : {NULL}
			ObjectPrintableArray : {NULL}
		}
		2 : [ObjectPrintable]: hashcode { 2 }
		{
			String : Child 2
			Id : 2
			ObjectPrintableChild : {NULL}
			ObjectPrintableParent : avoid circular loop for this [ObjectPrintable]: hashcode { 100 }
			ObjectPrintableDictionary : {NULL}
			ObjectPrintableArray : {NULL}
		}
	}
	ObjectPrintableArray : 
	{
		[ObjectPrintable]: hashcode { 1 }
		{
			String : Child 1
			Id : 1
			ObjectPrintableChild : {NULL}
			ObjectPrintableParent : avoid circular loop for this [ObjectPrintable]: hashcode { 100 }
			ObjectPrintableDictionary : {NULL}
			ObjectPrintableArray : {NULL}
		}
		[ObjectPrintable]: hashcode { 2 }
		{
			String : Child 2
			Id : 2
			ObjectPrintableChild : {NULL}
			ObjectPrintableParent : avoid circular loop for this [ObjectPrintable]: hashcode { 100 }
			ObjectPrintableDictionary : {NULL}
			ObjectPrintableArray : {NULL}
		}
	}
}
";
			//replace tabs & newlines to easily compare white spaces
//			expected = expected.Replace(Environment.NewLine, "_n").Replace("\t", "_t");
//			actual = actual.Replace(Environment.NewLine, "_n").Replace("\t", "_t");
//			Console.Out.WriteLine(expected);
			Console.Out.WriteLine(actual);
			Console.Out.WriteLine(actual.Length);
			Console.Out.WriteLine(expected.Length);
			actual.Should().Be(expected);
		}

		[Test]
		public void ObjectPrinter_includes_public_fields_in_output()
		{
			var testObject = new ObjectWithPublicFields
								{
									Id = 100,
									TestInteger = 25,
									TestString = "This is a test"
								};

			var printedTestObject = testObject.DumpToString();
			printedTestObject.Should().Be(
				@"[ObjectWithPublicFields]: hashcode { 100 }
{
	Id : 100
	TestInteger : 25
	TestString : This is a test
}
");
		}

		public class ObjectPrintable
		{
			private readonly string _customToString;
			public string String { get; set; }
			public int Id { get; set; }
			public ObjectPrintable ObjectPrintableChild { get; set; }
			public ObjectPrintable ObjectPrintableParent { get; set; }
			public Dictionary<int, ObjectPrintable> ObjectPrintableDictionary { get; set; }
			public ObjectPrintable[] ObjectPrintableArray { get; set; }

			public ObjectPrintable() { }

			public ObjectPrintable(string customToString)
			{
				_customToString = customToString;
			}

			public override int GetHashCode()
			{
				return Id;
			}

			public override string ToString()
			{
				return string.IsNullOrEmpty(_customToString)
						? base.ToString()
						: _customToString;
			}
		}

		internal class ObjectWithPublicFields
		{
			private string _testString = "Ignored by printer";	// Unused varible needed for tests
			public int TestInteger;
			public string TestString;

			public int Id { get; set; }

			public override int GetHashCode()
			{
				return Id;
			}
		}
	}
}