using System;
using System.Collections.Generic;
using System.Linq;

namespace numl.Model
{
    public class StringProperty : Property
    {
        public string Separator { get; set; }
        public StringSplitType SplitType { get; set; }
        public string[] Dictionary { get; set; }
        public string[] Exclude { get; set; }
    }
}
