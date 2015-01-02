using numl.Model;
using numl.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Tests.SerializationTests
{
    [TestFixture, Category("Serialization")]
    public class DescriptorSerializationTests : BaseSerialization
    {
        public static IEnumerable<string> WordStrings
        {
            get
            {
                yield return "TH@e QUi#CK bRow!N fox 12";
                yield return "s@upeR B#roWN BEaR";
                yield return "1829! UGlY FoX";
                yield return "ThE QuICk beAr";
            }
        }

        [Test]
        public void Descriptor_Dictionary_Serialization_Test()
        {
            Descriptor d = new Descriptor();

            var dictionary = StringHelpers.BuildWordDictionary(WordStrings)
                                          .Select(k => k.Key)
                                          .ToArray();

            d.Features = new Property[]
            {
                new Property { Name = "Age", Type = typeof(int) },
                new Property { Name = "Height", Type = typeof(decimal) },
                new StringProperty { Name = "Words", Dictionary = dictionary },
                new Property { Name = "Weight", Type = typeof(double) },
                new Property { Name = "Good", Type = typeof(bool) },
                
            };

            Serialize(d);
        }

        [Test]
        public void Descriptor_Full_Serialization_Test()
        {
            var dictionary = StringHelpers.BuildWordDictionary(WordStrings)
                                          .Select(k => k.Key)
                                          .ToArray();

            Descriptor d = Descriptor.New("MyDescriptor")
                                     .With("NumberProp").As(typeof(double))
                                     .With("EnumerableProp").AsEnumerable(5)
                                     .With("DateTimeProp").AsDateTime(DatePortion.Date | DatePortion.Time)
                                     .With("StringProp").AsString(StringSplitType.Word)
                                     .With("StringEnumProp").AsStringEnum()
                                     .Learn("TargetProp").As(typeof(bool));

            ((StringProperty)d["StringProp"]).Dictionary = dictionary;
            ((StringProperty)d["StringEnumProp"]).Dictionary = dictionary;

            Serialize(d);
        }
    }
}
