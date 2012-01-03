using System;
using numl.Math;
using numl.Model;
using System.Data;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using numl.Data;


namespace numl.Tests
{
    [TestFixture]
    public class ConversionTests
    {
        [Test]
        public void Test_Vector_Conversion_Simple_Numbers()
        {
            Description d = new Description();
            d.Features = new Property[]
            {
                new Property { Name = "Age", Type = typeof(int)},
                new Property { Name = "Height", Type = typeof(double)},
                new Property { Name = "Weight", Type = typeof(decimal)},
                new Property { Name = "Good", Type = typeof(bool)},
            };

            var o = new { Age = 23, Height = 6.21, Weight = 220m, Good = false };
            Vector truth = new[] { 23, 6.21, 220, -1 };
            Vector target = o.ToVector(d);
            Assert.AreEqual(truth, target);
        }

        [Test]
        public void Test_Vector_DataRow_Conversion_Simple_Numbers()
        {
            Description d = new Description();
            d.Features = new Property[]
            {
                new Property { Name = "Age", Type = typeof(int)},
                new Property { Name = "Height", Type = typeof(double)},
                new Property { Name = "Weight", Type = typeof(decimal)},
                new Property { Name = "Good", Type = typeof(bool)},
            };

            DataTable table = new DataTable("student");
            var age = table.Columns.Add("Age", typeof(int));
            var height = table.Columns.Add("Height", typeof(double));
            var weight = table.Columns.Add("Weight", typeof(decimal));
            var good = table.Columns.Add("Good", typeof(bool));

            DataRow row = table.NewRow();
            row[age] = 23;
            row[height] = 6.21;
            row[weight] = 220m;
            row[good] = false;

            Vector truth = new[] { 23, 6.21, 220, -1 };
            Vector target = row.ToVector(d);
            Assert.AreEqual(truth, target);
        }

        [Test]
        public void Test_Vector_Dictionary_Conversion_Simple_Numbers()
        {
            Description d = new Description();
            d.Features = new Property[]
            {
                new Property { Name = "Age", Type = typeof(int)},
                new Property { Name = "Height", Type = typeof(double)},
                new Property { Name = "Weight", Type = typeof(decimal)},
                new Property { Name = "Good", Type = typeof(bool)},
            };

            Dictionary<string, object> o = new Dictionary<string, object>();
            o["Age"] = 23;
            o["Height"] = 6.21;
            o["Weight"] = 220m;
            o["Good"] = false;

            Vector truth = new[] { 23, 6.21, 220, -1 };
            Vector target = o.ToVector(d);
            Assert.AreEqual(truth, target);
        }

        [Test]
        public void Test_Matrix_Conversion_Simple_Numbers()
        {
            Description d = new Description();
            d.Features = new Property[]
            {
                new Property { Name = "Age", Type = typeof(int)},
                new Property { Name = "Height", Type = typeof(double)},
                new Property { Name = "Weight", Type = typeof(decimal)},
                new Property { Name = "Good", Type = typeof(bool)},
            };

            var o = new[]
            {
                new { Age = 23, Height = 6.21, Weight = 220m, Good = false },
                new { Age = 67, Height = 5.11, Weight = 300m, Good = true },
                new { Age = 32, Height = 5.69, Weight = 130m, Good = false },
                new { Age = 12, Height = 4.56, Weight = 85m, Good = true },
            };

            Matrix truth = new[,]
                {{ 23, 6.21, 220, -1 },
                 { 67, 5.11, 300,  1 },
                 { 32, 5.69, 130, -1 },
                 { 12, 4.56,  85,  1 }};

            Matrix target = o.ToMatrix(d);
            Assert.AreEqual(truth, target);
        }

        [Test]
        public void Test_Matrix_Conversion_Numbers_Strings()
        {
            Description d = new Description();
            d.Features = new Property[]
            {
                new StringProperty { Name = "Content", Type = typeof(string), SplitType = StringSplitType.Word },
                new Property { Name = "Number", Type = typeof(double)},
            };

            var feed = Feed.GetData();
            Matrix target = feed.ToMatrix(d);
        }
    }
}
