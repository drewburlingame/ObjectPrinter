using System;
using NUnit.Framework;

namespace ObjectPrinter.Specs
{
    [TestFixture]
    public class ExceptionExtensionsTests
    {
        [Test]
        public void SetContext_should_work_with_null_context_item()
        {
            var ex = new Exception();
            ex.SetContext("context", null);
        }

        [Test]
        public void SetContext_should_work_with_non_serializabl_context_item()
        {
            var ex = new Exception();
            ex.SetContext("context", new {name="non-serializable"});
        }
    }
}