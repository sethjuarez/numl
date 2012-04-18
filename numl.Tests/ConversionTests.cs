/*
 Copyright (c) 2012 Seth Juarez

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 THE SOFTWARE.
*/

using System;
using numl.Data;
using numl.Math;
using numl.Model;
using System.Data;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;

namespace numl.Tests
{
    [TestFixture]
    public class ConversionTests
    {
        [Test]
        public void Test_Fast_Reflection_Get_Standard()
        {
            var o = new Student
            {
                Age = 23,
                Friends = 12,
                GPA = 3.2,
                Grade = numl.Data.Grade.A,
                Name = "Jordan Spears",
                Tall = true,
                Nice = false
            };

            var age = R.Get(o, "Age");
            Assert.AreEqual(23, (int)age);
            var friends = R.Get(o, "Friends");
            Assert.AreEqual(12, (int)friends);
            var gpa = R.Get(o, "GPA");
            Assert.AreEqual(3.2, (double)gpa);
            var grade = R.Get(o, "Grade");
            Assert.AreEqual(numl.Data.Grade.A, (numl.Data.Grade)grade);
            var name = R.Get(o, "Name");
            Assert.AreEqual("Jordan Spears", (string)name);
            var tall = R.Get(o, "Tall");
            Assert.AreEqual(true, (bool)tall);
            var nice = R.Get(o, "Nice");
            Assert.AreEqual(false, (bool)nice);
        }

        [Test]
        public void Test_Fast_Reflection_Set_Standard()
        {
            var o = new Student
            {
                Age = 23,
                Friends = 12,
                GPA = 3.2,
                Grade = numl.Data.Grade.A,
                Name = "Jordan Spears",
                Tall = true,
                Nice = false
            };

            R.Set(o, "Age", 25);
            Assert.AreEqual(25, o.Age);

            R.Set(o, "Friends", 1);
            Assert.AreEqual(1, o.Friends);

            R.Set(o, "GPA", 1.2);
            Assert.AreEqual(1.2, o.GPA);

            R.Set(o, "Grade", numl.Data.Grade.C);
            Assert.AreEqual(numl.Data.Grade.C, o.Grade);

            R.Set(o, "Name", "Seth Juarez");
            Assert.AreEqual("Seth Juarez", o.Name);

            R.Set(o, "Tall", false);
            Assert.AreEqual(false, o.Tall);

            R.Set(o, "Nice", true);
            Assert.AreEqual(true, o.Nice);
        }

        [Test]
        public void Test_Fast_Reflection_Get_Dictionary()
        {
            var o = new Dictionary<string, object>();
            o["Age"] = 23;
            o["Friends"] = 12;
            o["GPA"] = 3.2;
            o["Grade"] = numl.Data.Grade.A;
            o["Name"] = "Jordan Spears";
            o["Tall"] = true;
            o["Nice"] = false;

            var age = R.Get(o, "Age");
            Assert.AreEqual(23, (int)age);
            var friends = R.Get(o, "Friends");
            Assert.AreEqual(12, (int)friends);
            var gpa = R.Get(o, "GPA");
            Assert.AreEqual(3.2, (double)gpa);
            var grade = R.Get(o, "Grade");
            Assert.AreEqual(numl.Data.Grade.A, (numl.Data.Grade)grade);
            var name = R.Get(o, "Name");
            Assert.AreEqual("Jordan Spears", (string)name);
            var tall = R.Get(o, "Tall");
            Assert.AreEqual(true, (bool)tall);
            var nice = R.Get(o, "Nice");
            Assert.AreEqual(false, (bool)nice);
        }

        [Test]
        public void Test_Fast_Reflection_Set_Dictionary()
        {
            var o = new Dictionary<string, object>();
            o["Age"] = 23;
            o["Friends"] = 12;
            o["GPA"] = 3.2;
            o["Grade"] = numl.Data.Grade.A;
            o["Name"] = "Jordan Spears";
            o["Tall"] = true;
            o["Nice"] = false;

            R.Set(o, "Age", 25);
            Assert.AreEqual(25, o["Age"]);

            R.Set(o, "Friends", 1);
            Assert.AreEqual(1, o["Friends"]);

            R.Set(o, "GPA", 1.2);
            Assert.AreEqual(1.2, o["GPA"]);

            R.Set(o, "Grade", numl.Data.Grade.C);
            Assert.AreEqual(numl.Data.Grade.C, o["Grade"]);

            R.Set(o, "Name", "Seth Juarez");
            Assert.AreEqual("Seth Juarez", o["Name"]);

            R.Set(o, "Tall", false);
            Assert.AreEqual(false, o["Tall"]);

            R.Set(o, "Nice", true);
            Assert.AreEqual(true, o["Nice"]);
        }

        [Test]
        public void Test_Vector_Conversion_Simple_Numbers()
        {
            Description d = new Description();
            d.Features = new Property[]
            {
                new Property { Name = "Age", },
                new Property { Name = "Height", },
                new Property { Name = "Weight", },
                new Property { Name = "Good", },
            };

            var o = new { Age = 23, Height = 6.21, Weight = 220m, Good = false };
            Vector truth = new[] { 23, 6.21, 220, -1 };
            Vector target = d.ToVector(o);
            Assert.AreEqual(truth, target);
        }

        [Test]
        public void Test_Vector_DataRow_Conversion_Simple_Numbers()
        {
            Description d = new Description();
            d.Features = new Property[]
            {
                new Property { Name = "Age", },
                new Property { Name = "Height", },
                new Property { Name = "Weight", },
                new Property { Name = "Good", },
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
            Vector target = d.ToVector(row);
            Assert.AreEqual(truth, target);
        }

        [Test]
        public void Test_Vector_Dictionary_Conversion_Simple_Numbers()
        {
            Description d = new Description();
            d.Features = new Property[]
            {
                new Property { Name = "Age", },
                new Property { Name = "Height", },
                new Property { Name = "Weight", },
                new Property { Name = "Good", },
            };

            Dictionary<string, object> o = new Dictionary<string, object>();
            o["Age"] = 23;
            o["Height"] = 6.21;
            o["Weight"] = 220m;
            o["Good"] = false;

            Vector truth = new[] { 23, 6.21, 220, -1 };
            Vector target = d.ToVector(o);
            Assert.AreEqual(truth, target);
        }

        [Test]
        public void Test_Matrix_Conversion_Simple_Numbers()
        {
            Description d = new Description();
            d.Features = new Property[]
            {
                new Property { Name = "Age" },
                new Property { Name = "Height" },
                new Property { Name = "Weight" },
                new Property { Name = "Good" },
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

            Matrix target = d.ToMatrix(o);
            Assert.AreEqual(truth, target);
        }

        [Test]
        public void Test_Matrix_Labeled_Conversion_Simple_Numbers()
        {
            LabeledDescription d = new LabeledDescription
            {
                Features = new Property[]
                {
                    new Property { Name = "Age", },
                    new Property { Name = "Height", },
                    new Property { Name = "Weight", },
                },
                Label = new Property { Name = "Good",  }
            };

            var o = new[]
            {
                new { Age = 23, Height = 6.21, Weight = 220m, Good = false },
                new { Age = 67, Height = 5.11, Weight = 300m, Good = true },
                new { Age = 32, Height = 5.69, Weight = 130m, Good = false },
                new { Age = 12, Height = 4.56, Weight = 85m, Good = true },
            };

            Matrix truth = new[,]
                {{ 23, 6.21, 220 },
                 { 67, 5.11, 300 },
                 { 32, 5.69, 130 },
                 { 12, 4.56,  85 }};

            Vector yTruth = new[] { -1, 1, -1, 1 };

            var target = d.ToExamples(o);

            Assert.AreEqual(truth, target.Item1);
            Assert.AreEqual(yTruth, target.Item2);
        }

        [Test, ExpectedException(typeof(InvalidOperationException))] // no dictionary ;)
        public void Test_Matrix_Conversion_Numbers_Strings_Invalid_Dictionary()
        {
            Description d = new Description();
            d.Features = new Property[]
            {
                new Property { Name = "Cluster", },
                new StringProperty { Name = "Content",  },
                new Property { Name = "Number", },
            };

            var feed = Feed.GetData();
            Matrix target = d.ToMatrix(feed);
            Matrix truth = Feed.GetDataMatrix();
            Assert.AreEqual(truth, target);
        }

        [Test]
        public void Test_Matrix_Conversion_Numbers_Strings()
        {
            Description d = new Description();
            d.Features = new Property[]
            {
                new Property { Name = "Cluster", },
                new StringProperty { Name = "Content",  },
                new Property { Name = "Number", },
            };

            var feed = Feed.GetData();
            StringProperty p = d.Features[1] as StringProperty;
            p.BuildDictionary(feed);
            // sort for test
            p.Dictionary = p.Dictionary.OrderBy(s => s).ToArray();

            Matrix target = d.ToMatrix(feed);
            Matrix truth = Feed.GetDataMatrix();
            Assert.AreEqual(truth, target);
        }
    }
}
