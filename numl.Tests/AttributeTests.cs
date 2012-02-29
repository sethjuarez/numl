using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using numl.Tests.Data;
using numl.Model;

namespace numl.Tests
{
    [TestFixture]
    public class AttributeTests
    {
        [Test]
        public void Test_Fake1_Attributes()
        {
            var description = NConvert.ToDescription<Fake1>();

            Assert.IsTrue(description is LabeledDescription);
        }
    }
}
