using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using numl.AI.Collections;

using Xunit;

using MATH = System.Math;
using System.Diagnostics;

namespace numl.Tests.CollectionTests
{
    [Trait("CollectionTests", "CollectionTests")]
    public class CollectionsTests
    {
        [Fact]
        public void NSortedList_Sort_Test()
        {
            for (int run = 0; run < 2; run++)
            {
                bool reverse = (run != 0);

                var sorted = new NSortedList<int>(-1, reverse);

                int length = Math.Probability.Sampling.GetUniform(10, 1024);

                for (int i = 0; i < length; i++)
                {
                    int val = Math.Probability.Sampling.GetUniform(0, length - 1);
                    sorted.Add(val);
                }

                int remove = (int) MATH.Ceiling(length / 2.0);
                for (int i = 0; i < remove; i++)
                {
                    int index = Math.Probability.Sampling.GetUniform(0, sorted.Count - 1);
                    sorted.Remove(index);
                }

                bool result = false;
                for (int i = 1; i < (length - remove); i++)
                {
                    if (reverse)
                    {
                        result = sorted[i - 1] >= sorted[i];
                        Assert.True(result, $"Item at {i - 1} was larger than expected");
                    }
                    else
                    {
                        result = sorted[i - 1] <= sorted[i];
                        Assert.True(result, $"Item at {i - 1} was smaller than expected");
                    }
                }
            }
        }

        [Fact]
        public void NSortedList_Limit_Reverse_Test()
        {
            for (int run = 0; run < 2; run++)
            {
                int length = Math.Probability.Sampling.GetUniform(10, 1024);
                bool reverse = (run != 0);

                var sorted = new NSortedList<int>(length, reverse);
                
                for (int i = 0; i < length; i++)
                {
                    int val = Math.Probability.Sampling.GetUniform(0, length - 1);
                    sorted.Add(val);
                }

                length = (length / 2);

                sorted.Limit = length;

                Assert.True(sorted.Count == length, "Length exceeds specified limit");

                for (int i = 0; i < length / 2; i++)
                {
                    int val = Math.Probability.Sampling.GetUniform(0, length - 1);
                    sorted.Add(val);
                }

                Assert.True(sorted.Count == length, "Length exceeds specified limit after additions");

                bool result = false;
                for (int i = 1; i < sorted.Count; i++)
                {
                    if (reverse)
                    {
                        result = sorted[i - 1] >= sorted[i];
                        Assert.True(result, $"Item at {i - 1} was larger than expected");
                    }
                    else
                    {
                        result = sorted[i - 1] <= sorted[i];
                        Assert.True(result, $"Item at {i - 1} was smaller than expected");
                    }
                }
            }
        }

        [Fact]
        public void NSortedList_Size_Test()
        {
            var sorted = new NSortedList<int>();

            int length = Math.Probability.Sampling.GetUniform(10, 1024);

            var dict = new Dictionary<int, int>();

            for (int i = 0; i < length; i++)
            {
                int val = Math.Probability.Sampling.GetUniform(0, length - 1);
                sorted.Add(val);

                dict[val] = (dict.ContainsKey(val) ? dict[val] + 1 : 1);
            }

            int remove = (int) MATH.Ceiling(length / 2.0);
            for (int i = 0; i < remove; i++)
            {
                int val = Math.Probability.Sampling.GetUniform(0, sorted.Count - 1);

                if (dict.ContainsKey(val))
                {
                    sorted.Remove(val);

                    dict[val] = 0;

                    Assert.False(sorted.Contains(val));
                }
            }

            var contains = (dict.Where(w => w.Value == 0 && sorted.Contains(w.Key)));

            bool result = (contains.Count() == 0);

            Assert.True(result, "Length was not equal to the elements in the collection");

            bool lresult = (sorted.Select(s => s).Count() == sorted.Count);

            Assert.True(lresult, "Enumerator extends beyond number of accessible items in collection");
        }

        [Fact]
        public void NSortedList_Speed_Test()
        {
            int epochs = 10000;

            Stopwatch timer = new Stopwatch();

            timer.Start();

            var nsorted = new NSortedList<Foo>();
            
            for (int i = 0; i < epochs; i++)
            {
                var foo = new Foo();
                nsorted.Add(foo);
            }

            timer.Stop();

            long ts1 = timer.ElapsedMilliseconds;

            timer.Stop();
            Foo._Id = 0;

            timer.Start();

            var sorted = new SortedList<int, Foo>();

            for (int i = 0; i < epochs; i++)
            {
                var foo = new Foo();
                sorted.Add(foo.Weight, foo);
            }

            timer.Stop();

            long ts2 = timer.ElapsedMilliseconds;

            Assert.True(ts1 < ts2);
        }

        public class Foo : IComparable
        {
            internal static int _Id = 0;

            public int Weight { get; set; }

            public Foo()
            {
                this.Weight = (++_Id);
            }

            public int CompareTo(object obj)
            {
                var other = (Foo) obj;

                return this.Weight.CompareTo(other.Weight);
            }
        }
    }
}
