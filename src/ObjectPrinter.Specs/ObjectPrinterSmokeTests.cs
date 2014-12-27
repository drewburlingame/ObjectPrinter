using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace ObjectPrinter.Specs
{
    [TestFixture]
	public class ObjectPrinterSmokeTests
    {
        [TearDown]
        public void Teardown()
        {
            Config.Inspectors.IncludeCountsForCollections = false;
        }

	    [Test]
	    public void SmokeTest_XmlNode_should_return_inner_text()
        {
            var xml = "<bus:exception xmlns:bus=\"http://developer.cognos.com/schemas/bibus/3/\"><severity>error</severity><errorCode>cmBadProp</errorCode><bus:message><messageString>CM-REQ-4010 The property \"mobileDeviceID\" is unknown. Remove it or replace it with a valid property.</messageString></bus:message></bus:exception>";
	        var doc = new XmlDocument();
	        doc.LoadXml(xml);
	        var output = doc.DumpToString();
	        output.Should().Be(xml);
	    }

	    [Test]
		public void SmokeTest_XNode_should_return_inner_text()
		{
            var xml = "<bus:exception xmlns:bus=\"http://developer.cognos.com/schemas/bibus/3/\">\r\n  <severity>error</severity>\r\n  <errorCode>cmBadProp</errorCode>\r\n  <bus:message>\r\n    <messageString>CM-REQ-4010 The property \"mobileDeviceID\" is unknown. Remove it or replace it with a valid property.</messageString>\r\n  </bus:message>\r\n</bus:exception>";
			var doc = XElement.Load(new StringReader(xml));
			var output = doc.DumpToString();
			output.Should().Be(xml);
		    //Console.Out.WriteLine(output);
		}

        [Test]
        public void SmokeTest_empty_collections()
        {
            var stringWithReturns = "this is\r\na string\n\twith tabs\nand returns";
            var boringString = "i'm a bore";

            var child1 = new ObjectPrintable
            {
                String = boringString,
                Id = 17,
            };
            var child2 = new ObjectPrintable
            {
                String = stringWithReturns,
                Id = 33
            };

            var parent = new ObjectPrintable("Whos your daddy?")
            {
                String = "Parent",
                Id = 100,
                Child = child1,
                Array = new ObjectPrintable[0],
                Dictionary = new Dictionary<int, ObjectPrintable>(),
                Hashtable = new Hashtable(),
                NVC = new NameValueCollection()
            };

            child1.Parent = parent;
            child2.Parent = parent;

            var expected = @"[ObjectPrintable]: hashcode { $ignore$ }
{
	ToString() : Whos your daddy?
	String : Parent
	Id : 100
	Uri : {NULL}
	Child : [ObjectPrintable]: hashcode { $ignore$ }
	{
		String : i'm a bore
		Id : 17
		Uri : {NULL}
		Child : {NULL}
		Parent : avoid circular loop for this [ObjectPrintable]: hashcode { $ignore$ }
		Array : {NULL}
		Dictionary : {NULL}
		Hashtable : {NULL}
		NVC : {NULL}
		SomeEnumMember : SomeEnum.Enum22
	}
	Parent : {NULL}
	Array : [ObjectPrintable[]]: hashcode { $ignore$ }{}
	Dictionary : [Dictionary`2]: hashcode { $ignore$ }{}
	Hashtable : [Hashtable]: hashcode { $ignore$ }{}
	NVC : [NameValueCollection]: hashcode { $ignore$ }{}
	SomeEnumMember : SomeEnum.Enum22
}";

            shouldBeSame(expected, parent);
        }

        [Test]
		public void SmokeTest_general_use_case_including_recursion()
        {
            string expected = @"[ObjectPrintable]: hashcode { $ignore$ }
{
	ToString() : Whos your daddy?
	String : Parent
	Id : 100
	Uri : {NULL}
	Child : [ObjectPrintable]: hashcode { $ignore$ }
	{
		String : i'm a bore
		Id : 17
		Uri : {NULL}
		Child : {NULL}
		Parent : avoid circular loop for this [ObjectPrintable]: hashcode { $ignore$ }
		Array : {NULL}
		Dictionary : {NULL}
		Hashtable : {NULL}
		NVC : {NULL}
		SomeEnumMember : SomeEnum.Enum22
	}
	Parent : {NULL}
	Array : [ObjectPrintable[]]: hashcode { $ignore$ }
	{
		[ObjectPrintable]: hashcode { $ignore$ }
		{
			String : i'm a bore
			Id : 17
			Uri : {NULL}
			Child : {NULL}
			Parent : avoid circular loop for this [ObjectPrintable]: hashcode { $ignore$ }
			Array : {NULL}
			Dictionary : {NULL}
			Hashtable : {NULL}
			NVC : {NULL}
			SomeEnumMember : SomeEnum.Enum22
		}
		[ObjectPrintable]: hashcode { $ignore$ }
		{
			String : this is
				a string
					with tabs
				and returns
			Id : 33
			Uri : {NULL}
			Child : {NULL}
			Parent : avoid circular loop for this [ObjectPrintable]: hashcode { $ignore$ }
			Array : {NULL}
			Dictionary : {NULL}
			Hashtable : {NULL}
			NVC : {NULL}
			SomeEnumMember : SomeEnum.Enum22
		}
	}
	Dictionary : [Dictionary`2]: hashcode { $ignore$ }
	{
		17 : [ObjectPrintable]: hashcode { $ignore$ }
		{
			String : i'm a bore
			Id : 17
			Uri : {NULL}
			Child : {NULL}
			Parent : avoid circular loop for this [ObjectPrintable]: hashcode { $ignore$ }
			Array : {NULL}
			Dictionary : {NULL}
			Hashtable : {NULL}
			NVC : {NULL}
			SomeEnumMember : SomeEnum.Enum22
		}
		33 : [ObjectPrintable]: hashcode { $ignore$ }
		{
			String : this is
				a string
					with tabs
				and returns
			Id : 33
			Uri : {NULL}
			Child : {NULL}
			Parent : avoid circular loop for this [ObjectPrintable]: hashcode { $ignore$ }
			Array : {NULL}
			Dictionary : {NULL}
			Hashtable : {NULL}
			NVC : {NULL}
			SomeEnumMember : SomeEnum.Enum22
		}
	}
	Hashtable : [Hashtable]: hashcode { $ignore$ }
	{
		17 : [ObjectPrintable]: hashcode { $ignore$ }
		{
			String : i'm a bore
			Id : 17
			Uri : {NULL}
			Child : {NULL}
			Parent : avoid circular loop for this [ObjectPrintable]: hashcode { $ignore$ }
			Array : {NULL}
			Dictionary : {NULL}
			Hashtable : {NULL}
			NVC : {NULL}
			SomeEnumMember : SomeEnum.Enum22
		}
		33 : [ObjectPrintable]: hashcode { $ignore$ }
		{
			String : this is
				a string
					with tabs
				and returns
			Id : 33
			Uri : {NULL}
			Child : {NULL}
			Parent : avoid circular loop for this [ObjectPrintable]: hashcode { $ignore$ }
			Array : {NULL}
			Dictionary : {NULL}
			Hashtable : {NULL}
			NVC : {NULL}
			SomeEnumMember : SomeEnum.Enum22
		}
	}
	NVC : [NameValueCollection]: hashcode { $ignore$ }
	{
		stringWithReturns : this is
			a string
				with tabs
			and returns
		boringString : i'm a bore
	}
	SomeEnumMember : SomeEnum.Enum22
}";

            shouldBeSame(expected, build_general_use_case_including_recursion());
        }

        [Test]
        public void SmokeTest_general_use_case_including_recursion_and_counts_for_collections()
        {
            Config.Inspectors.IncludeCountsForCollections = true;
            string expected = @"[ObjectPrintable]: hashcode { $ignore$ }
{
	ToString() : Whos your daddy?
	String : Parent
	Id : 100
	Uri : {NULL}
	Child : [ObjectPrintable]: hashcode { $ignore$ }
	{
		String : i'm a bore
		Id : 17
		Uri : {NULL}
		Child : {NULL}
		Parent : avoid circular loop for this [ObjectPrintable]: hashcode { $ignore$ }
		Array : {NULL}
		Dictionary : {NULL}
		Hashtable : {NULL}
		NVC : {NULL}
		SomeEnumMember : SomeEnum.Enum22
	}
	Parent : {NULL}
	Array : [ObjectPrintable[]]: hashcode { $ignore$ }
	{
		Count : 2
		[ObjectPrintable]: hashcode { $ignore$ }
		{
			String : i'm a bore
			Id : 17
			Uri : {NULL}
			Child : {NULL}
			Parent : avoid circular loop for this [ObjectPrintable]: hashcode { $ignore$ }
			Array : {NULL}
			Dictionary : {NULL}
			Hashtable : {NULL}
			NVC : {NULL}
			SomeEnumMember : SomeEnum.Enum22
		}
		[ObjectPrintable]: hashcode { $ignore$ }
		{
			String : this is
				a string
					with tabs
				and returns
			Id : 33
			Uri : {NULL}
			Child : {NULL}
			Parent : avoid circular loop for this [ObjectPrintable]: hashcode { $ignore$ }
			Array : {NULL}
			Dictionary : {NULL}
			Hashtable : {NULL}
			NVC : {NULL}
			SomeEnumMember : SomeEnum.Enum22
		}
	}
	Dictionary : [Dictionary`2]: hashcode { $ignore$ }
	{
		Count : 2
		17 : [ObjectPrintable]: hashcode { $ignore$ }
		{
			String : i'm a bore
			Id : 17
			Uri : {NULL}
			Child : {NULL}
			Parent : avoid circular loop for this [ObjectPrintable]: hashcode { $ignore$ }
			Array : {NULL}
			Dictionary : {NULL}
			Hashtable : {NULL}
			NVC : {NULL}
			SomeEnumMember : SomeEnum.Enum22
		}
		33 : [ObjectPrintable]: hashcode { $ignore$ }
		{
			String : this is
				a string
					with tabs
				and returns
			Id : 33
			Uri : {NULL}
			Child : {NULL}
			Parent : avoid circular loop for this [ObjectPrintable]: hashcode { $ignore$ }
			Array : {NULL}
			Dictionary : {NULL}
			Hashtable : {NULL}
			NVC : {NULL}
			SomeEnumMember : SomeEnum.Enum22
		}
	}
	Hashtable : [Hashtable]: hashcode { $ignore$ }
	{
		Count : 2
		17 : [ObjectPrintable]: hashcode { $ignore$ }
		{
			String : i'm a bore
			Id : 17
			Uri : {NULL}
			Child : {NULL}
			Parent : avoid circular loop for this [ObjectPrintable]: hashcode { $ignore$ }
			Array : {NULL}
			Dictionary : {NULL}
			Hashtable : {NULL}
			NVC : {NULL}
			SomeEnumMember : SomeEnum.Enum22
		}
		33 : [ObjectPrintable]: hashcode { $ignore$ }
		{
			String : this is
				a string
					with tabs
				and returns
			Id : 33
			Uri : {NULL}
			Child : {NULL}
			Parent : avoid circular loop for this [ObjectPrintable]: hashcode { $ignore$ }
			Array : {NULL}
			Dictionary : {NULL}
			Hashtable : {NULL}
			NVC : {NULL}
			SomeEnumMember : SomeEnum.Enum22
		}
	}
	NVC : [NameValueCollection]: hashcode { $ignore$ }
	{
		Count : 2
		stringWithReturns : this is
			a string
				with tabs
			and returns
		boringString : i'm a bore
	}
	SomeEnumMember : SomeEnum.Enum22
}";
            shouldBeSame(expected, build_general_use_case_including_recursion());
        }

        private static ObjectPrintable build_general_use_case_including_recursion()
        {
            var stringWithReturns = "this is\r\na string\n\twith tabs\nand returns";
            var boringString = "i'm a bore";

            var child1 = new ObjectPrintable
                {
                    String = boringString,
                    Id = 17,
                };
            var child2 = new ObjectPrintable
                {
                    String = stringWithReturns,
                    Id = 33
                };

            var parent = new ObjectPrintable("Whos your daddy?")
                {
                    String = "Parent",
                    Id = 100,
                    Child = child1,
                    Array = new[] {child1, child2},
                    Dictionary = new Dictionary<int, ObjectPrintable>
                        {
                            {child1.Id, child1},
                            {child2.Id, child2}
                        },
                    Hashtable = new Hashtable
                        {
                            {child1.Id, child1},
                            {child2.Id, child2}
                        },
                    NVC = new NameValueCollection
                        {
                            {"stringWithReturns", stringWithReturns},
                            {"boringString", boringString}
                        }
                };

            child1.Parent = parent;
            child2.Parent = parent;
            return parent;
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
}");
		}

        [Test]
        public void given_NonSerializableWrapper_only_context_is_printed()
        {
            var ex = new Exception("some ex");
            ex.SetContext("addl message", "some addl message");
            ex.SetContext("non-serializable-obj", new ObjectWithPublicFields{Id = 5, TestInteger = 10, TestString = "string"});

            var expected = @"[Exception]: hashcode { $ignore$ }
{
	Message : some ex
	StackTrace : {NULL}
	Source : {NULL}
	TargetSite : {NULL}
	HelpLink : {NULL}
	HResult : -2146233088
	Data : [ListDictionaryInternal]: hashcode { $ignore$ }
	{
		addl message : some addl message
		non-serializable-obj : [ObjectWithPublicFields]: hashcode { $ignore$ }
		{
			Id : 5
			TestInteger : 10
			TestString : string
		}
	}
	InnerException : {NULL}
}";

            shouldBeSame(expected, ex);
        }

        private static void shouldBeSame(string expected, object objectToPrint)
        {
            var sb = new StringBuilder();
            try
            {
                objectToPrint.DumpTo(new StringWriter(sb));
            }
            catch (Exception)
            {
                Console.Out.WriteLine(sb.ToString());
                throw;
            }
            var actual = sb.ToString();

            var hashcodesRegex = new Regex("hashcode { [0-9]* }");
            actual = hashcodesRegex.Replace(actual, "hashcode { $ignore$ }");

            //replace tabs & newlines to easily compare white spaces
            //expected = expected.Replace(Environment.NewLine, "_n").Replace("\t", "_t");
            //actual = actual.Replace(Environment.NewLine, "_n").Replace("\t", "_t");

            //Console.Out.WriteLine("lengths: expected={0} actual={1}", expected.Length, actual.Length);
            Console.Out.WriteLine("EXPECTED:");
            Console.Out.WriteLine(expected);
            Console.Out.WriteLine("ACTUAL:");
            Console.Out.WriteLine(actual);

            actual.Should().Be(expected);
        }

		public class ObjectPrintable
		{
            public enum SomeEnum{Enum1,Enum22,Enum333};

			private readonly string _customToString;
			public string String { get; set; }
			public int Id { get; set; }
            public Uri Uri { get; set; }
			public ObjectPrintable Child { get; set; }
            public ObjectPrintable Parent { get; set; }
            public ObjectPrintable[] Array { get; set; }
            public Dictionary<int, ObjectPrintable> Dictionary { get; set; }
            public Hashtable Hashtable { get; set; }
            public NameValueCollection NVC { get; set; }
            public SomeEnum SomeEnumMember { get; set; }

            public ObjectPrintable()
            {
                SomeEnumMember = SomeEnum.Enum22;
            }

			public ObjectPrintable(string customToString) : this()
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
#pragma warning disable 0414
			private string _testString = "Ignored by printer";	// Unused varible needed for tests
#pragma warning restore 0414

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