using System;
using numl.Model;
using System.Linq;
using numl.Tests.Data;
using NUnit.Framework;
using System.Collections.Generic;

namespace numl.Tests.SerializationTests
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

            var po = Deserialize<Property>();


            Assert.AreEqual(p, po);
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

            var po = Deserialize<StringProperty>();

            Assert.AreEqual(p, po);
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

            var po = Deserialize<EnumerableProperty>();

            Assert.AreEqual(p, po);
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

            var po = Deserialize<DateTimeProperty>();

            Assert.AreEqual(p, po);
        }

        [Test]
        public void DateTime_Feature_Save_And_Load()
        {
            DateTimeFeature p = DateTimeFeature.DayOfWeek |
                                DateTimeFeature.Second |
                                DateTimeFeature.Minute;

            Serialize(p);

            var po = Deserialize<DateTimeFeature>();

            Assert.AreEqual(p, po);
        }

        [Test]
        public void Descriptor_Save_And_load()
        {
            var data = Iris.Load();
            var description = Descriptor.Create<Iris>();
            // to populate dictionaries
            var examples = description.ToExamples(data);

            Serialize(description);

            var d = Deserialize<Descriptor>();

            Assert.AreEqual(description.Type, d.Type);
            Assert.AreEqual(description.Features.Length, d.Features.Length);
            for (int i = 0; i < description.Features.Length; i++)
            {
                Assert.AreEqual(description.Features[i].Type, d.Features[i].Type);
                Assert.AreEqual(description.Features[i].Name, d.Features[i].Name);
                Assert.AreEqual(description.Features[i].Start, d.Features[i].Start);
            }

            Assert.AreEqual(description.Label.Type, d.Label.Type);
            Assert.AreEqual(description.Label.Name, d.Label.Name);
            Assert.AreEqual(description.Label.Start, d.Label.Start);
        }

        [Test]
        public void Complex_Descriptor_Save_And_Load_No_Label()
        {
            var data = Generic.GetRows(100).ToArray();
            var description = Descriptor.Create<Generic>();
            // to populate dictionaries
            var examples = description.ToExamples(data);

            Serialize(description);

            var d = Deserialize<Descriptor>();

            Assert.AreEqual(description.Type, d.Type);
            Assert.AreEqual(description.Features.Length, d.Features.Length);
            for (int i = 0; i < description.Features.Length; i++)
            {
                Assert.AreEqual(description.Features[i].Type, d.Features[i].Type);
                Assert.AreEqual(description.Features[i].Name, d.Features[i].Name);
                Assert.AreEqual(description.Features[i].Start, d.Features[i].Start);
            }

            Assert.IsTrue(d.Label == null);
        }
    }
}
