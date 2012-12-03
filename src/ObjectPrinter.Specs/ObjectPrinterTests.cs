using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace ObjectPrinter.Specs
{
    [TestFixture]
	public class ObjectPrinterTests
    {
        string _xmlString = "<bus:exception xmlns:bus=\"http://developer.cognos.com/schemas/bibus/3/\"><severity>error</severity><errorCode>cmBadProp</errorCode><bus:message><messageString>CM-REQ-4010 The property \"mobileDeviceID\" is unknown. Remove it or replace it with a valid property.</messageString></bus:message></bus:exception>";

	    [Test]
	    public void SmokeTest_XmlNode_should_return_inner_text()
	    {
	        var doc = new XmlDocument();
	        doc.LoadXml(_xmlString);
	        var output = doc.DumpToString();
	        output.Should().Be(_xmlString);
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
		public void SmokeTest_general_use_case_including_recursion()
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

			var actual = parent.DumpToString();
            string expected = @"[ObjectPrintable]: hashcode { 100 }
{
	ToString() : Whos your daddy?
	String : Parent
	Id : 100
	Uri : {NULL}
	Child : [ObjectPrintable]: hashcode { 17 }
	{
		String : i'm a bore
		Id : 17
		Uri : {NULL}
		Child : {NULL}
		Parent : avoid circular loop for this [ObjectPrintable]: hashcode { 100 }
		Array : {NULL}
		Dictionary : {NULL}
		Hashtable : {NULL}
		NVC : {NULL}
	}
	Parent : {NULL}
	Array : [ObjectPrintable[]]: hashcode { $Array_Hashcode$ }
	{
		[ObjectPrintable]: hashcode { 17 }
		{
			String : i'm a bore
			Id : 17
			Uri : {NULL}
			Child : {NULL}
			Parent : avoid circular loop for this [ObjectPrintable]: hashcode { 100 }
			Array : {NULL}
			Dictionary : {NULL}
			Hashtable : {NULL}
			NVC : {NULL}
		}
		[ObjectPrintable]: hashcode { 33 }
		{
			String : this is
				a string
					with tabs
				and returns
			Id : 33
			Uri : {NULL}
			Child : {NULL}
			Parent : avoid circular loop for this [ObjectPrintable]: hashcode { 100 }
			Array : {NULL}
			Dictionary : {NULL}
			Hashtable : {NULL}
			NVC : {NULL}
		}
	}
	Dictionary : [Dictionary`2]: hashcode { $Dictionary_Hashcode$ }
	{
		17 : [ObjectPrintable]: hashcode { 17 }
		{
			String : i'm a bore
			Id : 17
			Uri : {NULL}
			Child : {NULL}
			Parent : avoid circular loop for this [ObjectPrintable]: hashcode { 100 }
			Array : {NULL}
			Dictionary : {NULL}
			Hashtable : {NULL}
			NVC : {NULL}
		}
		33 : [ObjectPrintable]: hashcode { 33 }
		{
			String : this is
				a string
					with tabs
				and returns
			Id : 33
			Uri : {NULL}
			Child : {NULL}
			Parent : avoid circular loop for this [ObjectPrintable]: hashcode { 100 }
			Array : {NULL}
			Dictionary : {NULL}
			Hashtable : {NULL}
			NVC : {NULL}
		}
	}
	Hashtable : [Hashtable]: hashcode { $Hashtable_Hashcode$ }
	{
		17 : [ObjectPrintable]: hashcode { 17 }
		{
			String : i'm a bore
			Id : 17
			Uri : {NULL}
			Child : {NULL}
			Parent : avoid circular loop for this [ObjectPrintable]: hashcode { 100 }
			Array : {NULL}
			Dictionary : {NULL}
			Hashtable : {NULL}
			NVC : {NULL}
		}
		33 : [ObjectPrintable]: hashcode { 33 }
		{
			String : this is
				a string
					with tabs
				and returns
			Id : 33
			Uri : {NULL}
			Child : {NULL}
			Parent : avoid circular loop for this [ObjectPrintable]: hashcode { 100 }
			Array : {NULL}
			Dictionary : {NULL}
			Hashtable : {NULL}
			NVC : {NULL}
		}
	}
	NVC : [NameValueCollection]: hashcode { $NVC_Hashcode$ }
	{
		stringWithReturns : this is
			a string
				with tabs
			and returns
		boringString : i'm a bore
	}
}"
.Replace("$Array_Hashcode$", parent.Array.GetHashCode().ToString())
.Replace("$Dictionary_Hashcode$", parent.Dictionary.GetHashCode().ToString())
.Replace("$Hashtable_Hashcode$", parent.Hashtable.GetHashCode().ToString())
.Replace("$NVC_Hashcode$", parent.NVC.GetHashCode().ToString())
;
			//replace tabs & newlines to easily compare white spaces
//			expected = expected.Replace(Environment.NewLine, "_n").Replace("\t", "_t");
//			actual = actual.Replace(Environment.NewLine, "_n").Replace("\t", "_t");
//			Console.Out.WriteLine(expected);
//			Console.Out.WriteLine(actual);
//			Console.Out.WriteLine(actual.Length);
//			Console.Out.WriteLine(expected.Length);
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
}");
		}

		public class ObjectPrintable
		{
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