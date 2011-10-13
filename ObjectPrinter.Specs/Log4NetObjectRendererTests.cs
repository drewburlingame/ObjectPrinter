using System;
using System.Globalization;
using System.IO;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using ObjectPrinter.Log4Net;
using log4net.Util;

namespace ObjectPrinter.Specs
{

	/// <summary>This fixture will walk through the usage of logging exceptions using the ObjectRenderer</summary>
	[TestFixture]
	public class Log4NetObjectRendererTests
	{
		[Test]
		public void When_object_is_from_log4net_should_call_to_string()
		{
			var log4NetObj = new SystemStringFormat(CultureInfo.InvariantCulture, "should see {0}", "this string");
			string message = RenderObjectToString(log4NetObj);
			message.Should().Be("should see this string");
		}

		[Test]
		public void should_render_common_system_value_types_to_string()
		{
			RenderObjectToString("this message").Should().Be("this message");
			RenderObjectToString(1).Should().Be("1");
			RenderObjectToString(true).Should().Be("True");
			RenderObjectToString(5m).Should().Be("5");
			var guid = Guid.NewGuid();
			RenderObjectToString(guid).Should().Be(guid.ToString());
			var now = DateTime.Now;
			RenderObjectToString(now).Should().Be(now.ToString());
		}

		[Test]
		public void Should_render_properties_on_exceptions()
		{
			var output = RenderObjectToString(new InformativeException("the message")
			{
				AHelpfulTip = "a tip",
				IsARedHerring = true
			});

			output.Should().Contain("AHelpfulTip");
			output.Should().Contain("a tip");

			output.Should().Contain("IsARedHerring");
			output.Should().Contain("True");
		}

		private string RenderObjectToString(object obj)
		{
			var sb = new StringBuilder();
			using (var writer = new StringWriter(sb))
			{
				new Log4NetObjectRenderer().RenderObject(null, obj, writer);
			}
			return sb.ToString();
		}

		public class InformativeException : Exception
		{
			public InformativeException(string message)
				: base(message)
			{
			}

			public string AHelpfulTip { get; set; }
			public bool IsARedHerring { get; set; }
		}
	}
}
