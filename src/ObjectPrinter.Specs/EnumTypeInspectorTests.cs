using System;
using FluentAssertions;
using NUnit.Framework;

namespace ObjectPrinter.Specs.EnumTypeInspectorSpecs
{
    [TestFixture]
    public class EnumTypeInspectorTests
    {
        [Flags]
        public enum ImFlagged
        {
            Val1, Val2, Val3
        }
        public enum ImNotFlagged
        {
            Val1, Val2, Val3
        }

        [Test]
        public void Should_include_all_the_many_specified_values()
        {
            var flagged = ImFlagged.Val1 | ImFlagged.Val2;
            var output = flagged.DumpToString();
            //Console.Out.WriteLine(output);
            output.Should().Contain("Val1");
            output.Should().Contain("Val2");
            output.Should().NotContain("Val3");
            output.Should().NotContain("ObjectPrinter.Specs");
            output.Should().Contain("ImFlagged");
        }

        [Test]
        public void Should_be_only_the_specified_values()
        {
            var notFlagged = ImNotFlagged.Val2;
            var output = notFlagged.DumpToString();
            //Console.Out.WriteLine(output);
            output.Should().Be("ImNotFlagged.Val2");
        }
    }
}