using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Math
{
    public class Range
    {
        public double Min { get; set; }
        public double Max { get; set; }

        public static Range Make(double min, double max)
        {
            return new Range { Min = min, Max = max };
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1})", Min, Max);
        }
    }
}
