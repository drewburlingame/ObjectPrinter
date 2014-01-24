using FluentAssertions;
using NUnit.Framework;
using ObjectPrinter.TypeInspectors;

namespace ObjectPrinter.Specs
{
    [TestFixture]
    public class ObscuredTests
    {
        [Test]
        public void Default_settings_should_not_obscure()
        {
            var unobscuredValue = new TestClass().DumpToString();
            unobscuredValue.Should().Contain("hidden");
            unobscuredValue.Should().NotContain("obscured");
        }

        [Test]
        public void ShouldObscureValue_should_replace_values_with_obscured()
        {
            var obscuringInspector = new InspectAllTypeInspector
            {
                ShouldObscureValue = (o, memberInfo, objectInfo) => Funcs.MemberContainsPassword(memberInfo)
            };
            string obscuredValue = new TestClass().DumpToString(new ObjectPrinterConfig { Inspectors = new[] { obscuringInspector } });

            obscuredValue.Should().Contain("obscured");
            obscuredValue.Should().NotContain("hidden");
        }

        [Test]
        public void ShouldObscureValue_should_replace_values_with_ObscureValueText()
        {
            var obscuringInspector = new InspectAllTypeInspector
            {
                ShouldObscureValue = (o, memberInfo, objectInfo) => Funcs.MemberContainsPassword(memberInfo),
                ObscureValueText = "lalafishies"
            };
            string obscuredValue = new TestClass().DumpToString(new ObjectPrinterConfig { Inspectors = new[] { obscuringInspector } });

            obscuredValue.Should().Contain("lalafishies");
            obscuredValue.Should().NotContain("hidden");
        }

        private class TestClass
        {
            public string Password { get { return "hidden"; } }
            public string Pwd { get { return "hidden"; } }
            public string Salt { get { return "hidden"; } }
            public string ConnString { get { return "hidden"; } }
            public string ConnectionString { get { return "hidden"; } }
            public string ShowMe { get { return "shown"; } }
        }
    }
}