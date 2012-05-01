using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using numl.Data;
using numl.Model;
using numl.Supervised;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace numl.Tests
{
    [TestFixture]
    public class SerializationTests
    {
        public string _base = Directory.GetCurrentDirectory();

        [Test]
        public void Serialize_Property_Test()
        {
            var data = House.GetData();
            var description = Description.Create<House>();

            XmlSerializer serializer = new XmlSerializer(typeof(Property));
            string file = _base + "\\property_serialization.xml";

            if (File.Exists(file))
                File.Delete(file);

            using (var stream = File.OpenWrite(file))
                serializer.Serialize(stream, description[0]);

            Property p;
            using (var stream = File.OpenRead(file))
                p = (Property)serializer.Deserialize(stream);
        }

        [Test]
        public void Serialize_String_Property_Test()
        {
            var description = Description.Create<Feed>();

            description.BuildDictionaries(Feed.GetData());
            ((StringProperty)description[1]).Exclude = new[] { "THE", "IS", "HE", "SHE"};

            XmlSerializer serializer = new XmlSerializer(typeof(StringProperty));
            string file = _base + "\\string_property_serialization.xml";

            if (File.Exists(file))
                File.Delete(file);

            using (var stream = File.OpenWrite(file))
                serializer.Serialize(stream, description[1]);

            Property p;
            using (var stream = File.OpenRead(file))
                p = (Property)serializer.Deserialize(stream);
        }

        [Test]
        public void Serialize_Description_With_Strings_Test()
        {
            var description = Description.Create<Feed>();
            description.BuildDictionaries(Feed.GetData());
            ((StringProperty)description[1]).Exclude = new[] { "THE", "IS", "HE", "SHE" };

            XmlSerializer serializer = new XmlSerializer(description.GetType());
            string file = _base + "\\description_serialization.xml";

            if (File.Exists(file))
                File.Delete(file);
            
            using (var stream = File.OpenWrite(file))
                serializer.Serialize(stream, description);

            Description d;
            using (var stream = File.OpenRead(file))
                d = (Description)serializer.Deserialize(stream);
        }

        [Test]
        public void Serialize_Description_With_Empty_Label_Test()
        {
            var description = Description.Create<Feed>();
            description.BuildDictionaries(Feed.GetData());
            ((StringProperty)description[1]).Exclude = new[] { "THE", "IS", "HE", "SHE" };

            description.Label = null;

            XmlSerializer serializer = new XmlSerializer(description.GetType());
            string file = _base + "\\description_serialization_empty_label.xml";

            if (File.Exists(file))
                File.Delete(file);

            using (var stream = File.OpenWrite(file))
                serializer.Serialize(stream, description);

            Description d;
            using (var stream = File.OpenRead(file))
                d = (Description)serializer.Deserialize(stream);
        }

        [Test]
        public void Serialize_DT_Test()
        {
            var data = House.GetData();
            var description = Description.Create<House>();
            var generator = new DecisionTreeGenerator { Depth = 50 };
            var model = generator.Generate(description, data);
            string file = _base + "\\DT_MODEL_TEST.xml";

            if (File.Exists(file))
                File.Delete(file);

            model.Save(file);
        }

        [Test]
        public void Serialize_Kernel_Perceptron_Test()
        {
            var data = Student.GetData();
            var description = Description.Create<Student>();
            var generator = new KernelPerceptronGenerator();
            var model = generator.Generate(description, data);

            string file = _base + "\\KP_MODEL_TEST.xml";

            if (File.Exists(file))
                File.Delete(file);

            model.Save(file);
        }

        [Test]
        public void Serialize_Linear_Regression_Test()
        {
            var data = Student.GetData();
            var description = Description.Create<Student>();
            var generator = new LinearRegressionGenerator();
            var model = generator.Generate(description, data);

            string file = _base + "\\LIN_REG_MODEL_TEST.xml";

            if (File.Exists(file))
                File.Delete(file);

            model.Save(file);
        }

        [Test]
        public void Serialize_Perceptron_Test()
        {
            var data = Student.GetData();
            var description = Description.Create<Student>();
            var generator = new PerceptronGenerator();
            var model = generator.Generate(description, data);

            string file = _base + "\\PERC_MODEL_TEST.xml";

            if (File.Exists(file))
                File.Delete(file);

            model.Save(file);
        }
    }
}
