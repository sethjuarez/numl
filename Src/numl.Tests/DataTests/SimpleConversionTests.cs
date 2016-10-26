using System;
using numl.Utils;
using numl.Model;
using System.Linq;
using System.Dynamic;
using numl.Tests.Data;
using Xunit;
using System.Collections.Generic;

namespace numl.Tests.DataTests
{
    [Trait("Category", "Data")]
    public class SimpleConversionTests
    {
        public static IEnumerable<string> ShortStrings
        {
            get
            {
                yield return "OnE!";
                yield return "TWo2";
                yield return "t#hrE^e";
                yield return "T%hrE@e";
            }
        }

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

        [Fact]
        public void Test_Univariate_Conversions()
        {
            // boolean
            Assert.Equal(1, Ject.Convert(true));
            Assert.Equal(-1, Ject.Convert(false));

            // numeric types
            Assert.Equal(1d, Ject.Convert((Byte)1));                            // byte
            Assert.Equal(1d, Ject.Convert((SByte)1));                           // sbyte
            Assert.Equal(0.4d, Ject.Convert((Decimal)0.4m));                    // decimal
            Assert.Equal(0.1d, Ject.Convert((Double)0.1d));                     // double
            Assert.Equal(300d, Ject.Convert((Single)300f));                     // single
            Assert.Equal(1d, Ject.Convert((Int16)1));                           // int16
            Assert.Equal(2d, Ject.Convert((UInt16)2));                          // uint16
            Assert.Equal(2323432, Ject.Convert((Int32)2323432));                // int32
            Assert.Equal(2323432d, Ject.Convert((UInt32)2323432));              // uint32
            Assert.Equal(1232323434345d, Ject.Convert((Int64)1232323434345));   // int64
            Assert.Equal(1232323434345d, Ject.Convert((UInt64)1232323434345));  // uint64
            Assert.Equal(1232323434345d, Ject.Convert((long)1232323434345));    // long

            // enum
            Assert.Equal(0d, Ject.Convert(FakeEnum.Item0));
            Assert.Equal(1d, Ject.Convert(FakeEnum.Item1));
            Assert.Equal(2d, Ject.Convert(FakeEnum.Item2));
            Assert.Equal(3d, Ject.Convert(FakeEnum.Item3));
            Assert.Equal(4d, Ject.Convert(FakeEnum.Item4));
            Assert.Equal(5d, Ject.Convert(FakeEnum.Item5));
            Assert.Equal(6d, Ject.Convert(FakeEnum.Item6));
            Assert.Equal(7d, Ject.Convert(FakeEnum.Item7));
            Assert.Equal(8d, Ject.Convert(FakeEnum.Item8));
            Assert.Equal(9d, Ject.Convert(FakeEnum.Item9));

            // char
            Assert.Equal(65d, Ject.Convert('A'));

            // timespan
            Assert.Equal(300d, Ject.Convert(TimeSpan.FromSeconds(300)));
        }

        [Fact]
        public void Test_Univariate_Back_Conversions()
        {
            // boolean
            Assert.Equal(true, Ject.Convert(1, typeof(bool)));
            Assert.Equal(false, Ject.Convert(0, typeof(bool)));
            Assert.Equal(false, Ject.Convert(-1, typeof(bool)));

            // numeric types

            Assert.Equal((Byte)1, Ject.Convert(1d, typeof(Byte)));                               // byte
            Assert.Equal((SByte)1, Ject.Convert(1d, typeof(SByte)));                             // sbyte
            Assert.Equal((Decimal)0.4m, Ject.Convert(0.4d, typeof(Decimal)));                    // decimal
            Assert.Equal((Double)0.1d, Ject.Convert(0.1d, typeof(Double)));                      // double
            Assert.Equal((Single)300f, Ject.Convert(300d, typeof(Single)));                      // single
            Assert.Equal((Int16)1, Ject.Convert(1d, typeof(Int16)));                             // int16
            Assert.Equal((UInt16)2, Ject.Convert(2d, typeof(UInt16)));                           // uint16
            Assert.Equal((Int32)2323432, Ject.Convert(2323432, typeof(Int32)));                  // int32
            Assert.Equal((UInt32)2323432, Ject.Convert(2323432d, typeof(UInt32)));               // uint32
            Assert.Equal((Int64)1232323434345, Ject.Convert(1232323434345d, typeof(Int64)));     // int64
            Assert.Equal((UInt64)1232323434345, Ject.Convert(1232323434345d, typeof(UInt64)));   // uint64
            Assert.Equal((long)1232323434345, Ject.Convert(1232323434345d, typeof(long)));       // long

            // enum
            Assert.Equal(FakeEnum.Item0, Ject.Convert(0d, typeof(FakeEnum)));
            Assert.Equal(FakeEnum.Item1, Ject.Convert(1d, typeof(FakeEnum)));
            Assert.Equal(FakeEnum.Item2, Ject.Convert(2d, typeof(FakeEnum)));
            Assert.Equal(FakeEnum.Item3, Ject.Convert(3d, typeof(FakeEnum)));
            Assert.Equal(FakeEnum.Item4, Ject.Convert(4d, typeof(FakeEnum)));
            Assert.Equal(FakeEnum.Item5, Ject.Convert(5d, typeof(FakeEnum)));
            Assert.Equal(FakeEnum.Item6, Ject.Convert(6d, typeof(FakeEnum)));
            Assert.Equal(FakeEnum.Item7, Ject.Convert(7d, typeof(FakeEnum)));
            Assert.Equal(FakeEnum.Item8, Ject.Convert(8d, typeof(FakeEnum)));
            Assert.Equal(FakeEnum.Item9, Ject.Convert(9d, typeof(FakeEnum)));

            // char
            Assert.Equal('A', Ject.Convert(65d, typeof(char)));

            // timespan
            Assert.Equal(TimeSpan.FromSeconds(300), Ject.Convert(300d, typeof(TimeSpan)));
        }

        [Fact]
        public void Test_Char_Dictionary_Gen()
        {
            var d = StringHelpers.BuildCharArray(ShortStrings);
            // should have the following:
            // O, N, E, #SYM#, T, W, #NUM#, H, R
            var dict = new string[]
            {
                "O",
                "N",
                "E", 
                "#SYM#",
                "T", 
                "W", 	  
                "#NUM#",
                "H", 
                "R", 		
            };

            Assert.Equal(dict, d);
        }

        [Fact]
        public void Test_Enum_Dictionary_Gen()
        {
            var d = StringHelpers.BuildEnumArray(ShortStrings);
            // should have the following:
            // ONE, TWO2, THREE
            var dict = new string[]
            {
                "ONE", 
                "TWO2",
                "THREE",
            };

            Assert.Equal(dict, d);
        }

        [Fact]
        public void Test_Word_Dictionary_Gen()
        {
            var d = StringHelpers.BuildDistinctWordArray(WordStrings);
            // should have the following:
            var dict = new string[]
            {
                "THE", 
                "QUICK",
                "BROWN",
                "FOX", 
                "#NUM#",
                "SUPER",
                "BEAR",
                "UGLY",
            };

            Assert.Equal(dict, d);
        }

        [Fact]
        public void Test_Fast_Reflection_Get_Standard()
        {
            var o = new Student
            {
                Age = 23,
                Friends = 12,
                GPA = 3.2,
                Grade = Grade.A,
                Name = "Jordan Spears",
                Tall = true,
                Nice = false
            };

            var age = Ject.Get(o, "Age");
            Assert.Equal(23, (int)age);
            var friends = Ject.Get(o, "Friends");
            Assert.Equal(12, (int)friends);
            var gpa = Ject.Get(o, "GPA");
            Assert.Equal(3.2, (double)gpa);
            var grade = Ject.Get(o, "Grade");
            Assert.Equal(Grade.A, (Grade)grade);
            var name = Ject.Get(o, "Name");
            Assert.Equal("Jordan Spears", (string)name);
            var tall = Ject.Get(o, "Tall");
            Assert.Equal(true, (bool)tall);
            var nice = Ject.Get(o, "Nice");
            Assert.Equal(false, (bool)nice);
        }

        [Fact]
        public void Test_Fast_Reflection_Set_Standard()
        {
            var o = new Student
            {
                Age = 23,
                Friends = 12,
                GPA = 3.2,
                Grade = Grade.A,
                Name = "Jordan Spears",
                Tall = true,
                Nice = false
            };

            Ject.Set(o, "Age", 25);
            Assert.Equal(25, o.Age);

            Ject.Set(o, "Friends", 1);
            Assert.Equal(1, o.Friends);

            Ject.Set(o, "GPA", 1.2);
            Assert.Equal(1.2, o.GPA);

            Ject.Set(o, "Grade", Grade.C);
            Assert.Equal(Grade.C, o.Grade);

            Ject.Set(o, "Name", "Seth Juarez");
            Assert.Equal("Seth Juarez", o.Name);

            Ject.Set(o, "Tall", false);
            Assert.Equal(false, o.Tall);

            Ject.Set(o, "Nice", true);
            Assert.Equal(true, o.Nice);
        }

        [Fact]
        public void Test_Fast_Reflection_Get_Dictionary()
        {
            var o = new Dictionary<string, object>();
            o["Age"] = 23;
            o["Friends"] = 12;
            o["GPA"] = 3.2;
            o["Grade"] = Grade.A;
            o["Name"] = "Jordan Spears";
            o["Tall"] = true;
            o["Nice"] = false;

            var age = Ject.Get(o, "Age");
            Assert.Equal(23, (int)age);
            var friends = Ject.Get(o, "Friends");
            Assert.Equal(12, (int)friends);
            var gpa = Ject.Get(o, "GPA");
            Assert.Equal(3.2, (double)gpa);
            var grade = Ject.Get(o, "Grade");
            Assert.Equal(Grade.A, (Grade)grade);
            var name = Ject.Get(o, "Name");
            Assert.Equal("Jordan Spears", (string)name);
            var tall = Ject.Get(o, "Tall");
            Assert.Equal(true, (bool)tall);
            var nice = Ject.Get(o, "Nice");
            Assert.Equal(false, (bool)nice);
        }

        [Fact]
        public void Test_Fast_Reflection_Set_Dictionary()
        {
            var o = new Dictionary<string, object>();
            o["Age"] = 23;
            o["Friends"] = 12;
            o["GPA"] = 3.2;
            o["Grade"] = Grade.A;
            o["Name"] = "Jordan Spears";
            o["Tall"] = true;
            o["Nice"] = false;

            Ject.Set(o, "Age", 25);
            Assert.Equal(25, o["Age"]);

            Ject.Set(o, "Friends", 1);
            Assert.Equal(1, o["Friends"]);

            Ject.Set(o, "GPA", 1.2);
            Assert.Equal(1.2, o["GPA"]);

            Ject.Set(o, "Grade", Grade.C);
            Assert.Equal(Grade.C, o["Grade"]);

            Ject.Set(o, "Name", "Seth Juarez");
            Assert.Equal("Seth Juarez", o["Name"]);

            Ject.Set(o, "Tall", false);
            Assert.Equal(false, o["Tall"]);

            Ject.Set(o, "Nice", true);
            Assert.Equal(true, o["Nice"]);
        }

        [Fact]
        public void Test_Vector_Conversion_Simple_Numbers()
        {
            Descriptor d = new Descriptor();
            d.Features = new Property[]
            {
                new Property { Name = "Age", Type = typeof(int) },
                new Property { Name = "Height", Type=typeof(double) },
                new Property { Name = "Weight", Type = typeof(decimal) },
                new Property { Name = "Good", Type=typeof(bool) },
            };

            var o = new { Age = 23, Height = 6.21d, Weight = 220m, Good = false };

            var truths = new double[] { 23, 6.21, 220, -1 };
            var actual = d.Convert(o);
            Assert.Equal(truths, actual);
        }

        //[Fact]
        //public void Test_Vector_DataRow_Conversion_Simple_Numbers()
        //{
        //    Descriptor d = new Descriptor();
        //    d.Features = new Property[]
        //    {
        //        new Property { Name = "Age", },
        //        new Property { Name = "Height", },
        //        new Property { Name = "Weight", },
        //        new Property { Name = "Good", },
        //    };

        //    DataTable table = new DataTable("student");
        //    var age = table.Columns.Add("Age", typeof(int));
        //    var height = table.Columns.Add("Height", typeof(double));
        //    var weight = table.Columns.Add("Weight", typeof(decimal));
        //    var good = table.Columns.Add("Good", typeof(bool));

        //    DataRow row = table.NewRow();
        //    row[age] = 23;
        //    row[height] = 6.21;
        //    row[weight] = 220m;
        //    row[good] = false;

        //    var truths = new double[] { 23, 6.21, 220, -1 };
        //    var actual = d.Convert(row);
        //    Assert.Equal(truths, actual);
        //}

        [Fact]
        public void Test_Vector_Dictionary_Conversion_Simple_Numbers()
        {
            Descriptor d = new Descriptor();
            d.Features = new Property[]
            {
                new Property { Name = "Age", },
                new Property { Name = "Height", },
                new Property { Name = "Weight", },
                new Property { Name = "Good", },
            };

            Dictionary<string, object> item = new Dictionary<string, object>();
            item["Age"] = 23;
            item["Height"] = 6.21;
            item["Weight"] = 220m;
            item["Good"] = false;

            var truths = new double[] { 23, 6.21, 220, -1 };
            var actual = d.Convert(item);
            Assert.Equal(truths, actual);
        }

        [Fact]
        public void Test_Vector_Expando_Conversion_Simple_Numbers()
        {
            Descriptor d = new Descriptor();
            d.Features = new Property[]
            {
                new Property { Name = "Age", },
                new Property { Name = "Height", },
                new Property { Name = "Weight", },
                new Property { Name = "Good", },
            };

            dynamic item = new ExpandoObject();
            item.Age = 23;
            item.Height = 6.21;
            item.Weight = 220m;
            item.Good = false;

            var truths = new double[] { 23, 6.21, 220, -1 };
            var actual = d.Convert(item);
            Assert.Equal(truths, actual);
        }

        [Fact]
        public void Test_Vector_Conversion_Simple_Numbers_And_Strings()
        {
            Descriptor d = new Descriptor();

            var dictionary = StringHelpers.BuildDistinctWordArray(WordStrings);

            d.Features = new Property[]
            {
                new Property { Name = "Age" },
                new Property { Name = "Height" },
                new StringProperty { Name = "Words", Dictionary = dictionary },
                new Property { Name = "Weight" },
                new Property { Name = "Good" },
                
            };

            // [THE, QUICK, BROWN, FOX, #NUM#, SUPER, BEAR, UGLY]
            // [  1,     0,     1,   0,     1,     0,    0,    0]

            var o = new { Age = 23, Height = 6.21d, Weight = 220m, Good = false, Words = "the brown 432" };

            // array generated by descriptor ordering
            var truths = new double[] { 23, 6.21,  
                                        /* BEGIN TEXT */ 
                                        1, 0, 1, 0, 1, 0, 0, 0,
                                        /* END TEXT */
                                        220, -1 };
            var actual = d.Convert(o);
            Assert.Equal(truths, actual);
            // offset test
            Assert.Equal(10, d.Features[3].Start);
            Assert.Equal(11, d.Features[4].Start);
        }

        [Fact]
        public void Test_Vector_Conversion_Simple_Numbers_And_Strings_As_Enum()
        {
            Descriptor d = new Descriptor();

            var dictionary = StringHelpers.BuildDistinctWordArray(WordStrings);

            d.Features = new Property[]
            {
                new Property { Name = "Age" },
                new Property { Name = "Height" },
                new StringProperty { Name = "Words", Dictionary = dictionary, AsEnum = true },
                new Property { Name = "Weight" },
                new Property { Name = "Good" },
                
            };

            // [THE, QUICK, BROWN, FOX, #NUM#, SUPER, BEAR, UGLY]
            // [  0,     1,     2,   3,     4,     5,    6,    7]

            var o = new { Age = 23, Height = 6.21d, Weight = 220m, Good = false, Words = "QUICK" };

            // array generated by descriptor ordering
            var truths = new double[] { 23, 6.21,  
                                        /* BEGIN TEXT */ 
                                        1,
                                        /* END TEXT */
                                        220, -1 };
            var actual = d.Convert(o);
            Assert.Equal(truths, actual);


            o = new { Age = 23, Height = 6.21d, Weight = 220m, Good = false, Words = "sUpEr" };
            // array generated by descriptor ordering
            truths = new double[] { 23, 6.21,  
                                        /* BEGIN TEXT */ 
                                        5,
                                        /* END TEXT */
                                        220, -1 };
            actual = d.Convert(o);
            Assert.Equal(truths, actual);
        }

        [Fact]
        public void Test_Vector_Conversion_Simple_Numbers_And_Chars()
        {
            Descriptor d = new Descriptor();

            var dictionary = StringHelpers.BuildCharArray(ShortStrings);

            d.Features = new Property[]
            {
                new Property { Name = "Age" },
                new Property { Name = "Height" },
                new StringProperty { 
                    Name = "Chars", 
                    Dictionary = dictionary, 
                    SplitType = StringSplitType.Character 
                },
                new Property { Name = "Weight" },
                new Property { Name = "Good" },
                
            };

            // ["O",  "N",  "E", "#SYM#", "T", "W", "#NUM#", "H", "R"]
            // [  2,    1,   0,        1,   0,   0,       1,   0,   3]

            var o = new { Age = 23, Height = 6.21d, Chars = "oon!3RrR", Weight = 220m, Good = false, };

            // array generated by descriptor ordering
            var truths = new double[] { 23, 6.21,  
                                        /* BEGIN CHARS */ 
                                        2, 1, 0, 1, 0, 0, 1, 0, 3,
                                        /* END CHARS */
                                        220, -1 };
            var actual = d.Convert(o);
            Assert.Equal(truths, actual);
        }

        [Fact]
        public void Test_Vector_Conversion_Simple_Numbers_And_Chars_As_Enum()
        {
            Descriptor d = new Descriptor();

            var dictionary = StringHelpers.BuildCharArray(ShortStrings);

            d.Features = new Property[]
            {
                new Property { Name = "Age" },
                new Property { Name = "Height" },
                new StringProperty { 
                    Name = "Chars", 
                    Dictionary = dictionary, 
                    SplitType = StringSplitType.Character,
                    AsEnum = true
                },
                new Property { Name = "Weight" },
                new Property { Name = "Good" },
                
            };

            // ["O",  "N",  "E", "#SYM#", "T", "W", "#NUM#", "H", "R"]
            // [  2,    1,   0,        1,   0,   0,       1,   0,   3]

            var o = new { Age = 23, Height = 6.21d, Chars = "N", Weight = 220m, Good = false, };

            // array generated by descriptor ordering
            var truths = new double[] { 23, 6.21,  
                                        /* BEGIN CHARS */ 
                                        1,
                                        /* END CHARS */
                                        220, -1 };

            var actual = d.Convert(o);
            Assert.Equal(truths, actual);


            o = new { Age = 23, Height = 6.21d, Chars = "!", Weight = 220m, Good = false, };

            // array generated by descriptor ordering
            truths = new double[] { 23, 6.21,  
                                    /* BEGIN CHARS */ 
                                    3,
                                    /* END CHARS */
                                    220, -1 };

            actual = d.Convert(o);
            Assert.Equal(truths, actual);
        }

        [Fact]
        public void Test_Matrix_Conversion_Simple()
        {
            Descriptor d = new Descriptor();

            d.Features = new Property[]
            {
                new Property { Name = "Age" },
                new Property { Name = "Height" },
                new Property { Name = "Weight" },
                new Property { Name = "Good" },
            };

            var o = new[] {
                new { Age = 23, Height = 6.21d, Weight = 220m, Good = false },
                new { Age = 12, Height = 4.2d, Weight = 120m, Good = true },
                new { Age = 9, Height = 6.0d, Weight = 340m, Good = true },
                new { Age = 87, Height = 3.7d, Weight = 79m, Good = false }
            };

            // array generated by descriptor ordering
            // (returns IEnumerable, but should pass)
            var truths = new double[][] {
                new double[] { 23, 6.21, 220, -1 },
                new double[] { 12,  4.2,  120, 1 },
                new double[] { 9,  6.0,  340,  1 },
                new double[] { 87,  3.7,  79,  -1 }
            };

            var actual = d.Convert(o);

            var array = actual.Select(s => s.ToArray()).ToArray();
            Assert.Equal(truths, array);
        }

        [Fact]
        public void Test_Matrix_Conversion_With_Label()
        {
            Descriptor d = new Descriptor();

            d.Features = new Property[]
            {
                new Property { Name = "Age" },
                new Property { Name = "Height" },
                new Property { Name = "Weight" },
                new Property { Name = "Good" },
            };

            d.Label = new Property { Name = "Nice" };

            var o = new[] {
                new { Age = 23, Height = 6.21d, Weight = 220m, Good = false, Nice = true },
                new { Age = 12, Height = 4.2d, Weight = 120m, Good = true, Nice = false },
                new { Age = 9, Height = 6.0d, Weight = 340m, Good = true, Nice = true },
                new { Age = 87, Height = 3.7d, Weight = 79m, Good = false, Nice = false }
            };

            // array generated by descriptor ordering
            // (returns IEnumerable, but should pass)
            var truths = new double[][] {
                new double[] { 23, 6.21, 220, -1, 1 },
                new double[] { 12,  4.2,  120, 1, -1 },
                new double[] { 9,  6.0,  340,  1, 1 },
                new double[] { 87,  3.7,  79,  -1, -1 }
            };

            var actual = d.Convert(o);
            var array = actual.Select(s => s.ToArray()).ToArray();
            Assert.Equal(truths, array);
        }
    }
}
