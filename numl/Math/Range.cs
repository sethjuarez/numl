using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace numl.Math
{
    [XmlRoot("Range")]
    public class Range
    {
        [XmlAttribute("Min")]
        public double Min { get; set; }
        [XmlAttribute("Max")]
        public double Max { get; set; }

        public bool Test(double d)
        {
            return d >= Min && d < Max;
        }

        public static Range Make(double min, double max)
        {
            return new Range { Min = min, Max = max };
        }

        public static Range Make(double min)
        {
            return new Range { Min = min, Max = min + 0.00001 };
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1})", Min, Max);
        }
    }
}
