using System;
using System.IO;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using ObjectPrinter.Utilties;

namespace ObjectPrinter.Specs
{
    [TestFixture]
    public class IndentAwareTextWriterTests
    {
        [Test]
        public void should_alter_tab_depth_with_indent_and_outdent()
        {
            var sb = new StringBuilder();
            var writer = new IndentableTextWriter(new StringWriter(sb), "\t", "\n");

            writer.Write("a small");
            writer.WriteLine(" test");
            writer.Indent();
            writer.WriteLine("determines\n");
            writer.Outdent();
            writer.Write("if\r");
            writer.Indent();
            writer.WriteLine("this");
            writer.WriteLine("\twill");
            writer.Indent();
            writer.WriteLine("work");

            sb.ToString().Should().Be("a small test\n\tdetermines\n\nif\n\tthis\n\t\twill\n\t\twork\n");
            Console.Out.WriteLine(sb.ToString());
        }
    }
}