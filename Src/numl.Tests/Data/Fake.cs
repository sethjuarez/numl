using System;
using numl.Model;
using System.Linq;
using System.Collections.Generic;

namespace numl.Tests.Data
{
    public class FakeDate
    {
        // First four to test feature conversion
        [DateFeature(DateTimeFeature.Day)]
        public DateTime Date1 { get; set; }

        [DateFeature(DateTimeFeature.Day | DateTimeFeature.Year)]
        public DateTime Date2 { get; set; }

        [DateFeature(DateTimeFeature.Day | DateTimeFeature.DayOfYear | DateTimeFeature.Millisecond)]
        public DateTime Date3{ get; set; }

        [DateFeature(DateTimeFeature.Month | DateTimeFeature.Year | DateTimeFeature.Second | DateTimeFeature.Hour)]
        public DateTime Date4 { get; set; }

        // last four test date portion
        [DateFeature(DatePortion.Date)]
        public DateTime Date5 { get; set; }
        [DateFeature(DatePortion.Date | DatePortion.TimeExtended)]
        public DateTime Date6 { get; set; }
        [DateFeature(DatePortion.Time)]
        public DateTime Date7 { get; set; }
        [DateFeature(DatePortion.Date | DatePortion.DateExtended)]
        public DateTime Date8 { get; set; }
    }

    public class FakeDateWithError
    {
        [DateFeature(DatePortion.Date)]
        public int NotADate { get; set; }
    }

    public class FakeEnumerable
    {
        [EnumerableFeature(20)]
        public IEnumerable<int> Numbers1 { get; set; }

        [EnumerableFeature(5)]
        public double[] Numbers2 { get; set; }

        [EnumerableFeature(46)]
        public List<char> Numbers3 { get; set; }
    }

    public class FakEnumerableWithError1
    {
        [EnumerableFeature(12)]
        public int NotAnEnumerable { get; set; }
    }

    public class FakEnumerableWithError2
    {
        [EnumerableFeature(0)]
        public List<char> Numbers3 { get; set; }
    }
}
