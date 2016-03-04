using System;
using numl.Model;
using System.Linq;
using numl.Tests.Data;
using NUnit.Framework;
using System.Collections.Generic;
using numl.Serialization;

namespace numl.Tests.SerializationTests.DescriptorSerialization
{
    [TestFixture, Category("Serialization")]
    public class PropertySerializationTests : BaseSerialization
    {
        [Test]
        public void Standard_Property_Save_And_Load()
        {
            Property p = new Property();
            p.Name = "MyProp";
            p.Type = typeof(decimal);
            p.Discrete = false;
            p.Start = 5;

            Serialize(p);
            var property = Deserialize<Property>();
            Assert.AreEqual(p, property);
        }

        [Test]
        public void String_Property_Save_And_Load()
        {
            StringProperty p = new StringProperty();
            p.Name = "MyProp";
            p.Start = 5;

            p.Dictionary = new string[] { "ONE", "TWO", "THREE", "FOUR" };
            p.Exclude = new string[] { "FIVE", "SIX", "SEVEN" };

            Serialize(p);
            var property = Deserialize<StringProperty>();
            Assert.AreEqual(p, property);
        }

        [Test]
        public void Enumerable_Property_Save_And_Load()
        {
            EnumerableProperty p = new EnumerableProperty(100);
            p.Name = "MyProp";
            p.Type = typeof(decimal);
            p.Discrete = false;
            p.Start = 5;

            Serialize(p);
            var property = Deserialize<EnumerableProperty>();
            Assert.AreEqual(p, property);
        }

        [Test]
        public void DateTime_Property_Save_And_Load()
        {
            DateTimeProperty p =
                new DateTimeProperty(DateTimeFeature.DayOfWeek |
                                     DateTimeFeature.Second |
                                     DateTimeFeature.Minute);
            p.Name = "MyProp";
            p.Discrete = false;
            p.Start = 5;

            Serialize(p);
            var property = Deserialize<DateTimeProperty>();
            Assert.AreEqual(p, property);
        }
    }
}
