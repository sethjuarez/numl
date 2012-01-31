using System;
using System.Collections.Generic;
using System.Linq;

namespace numl.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StringLabelAttribute
        : StringAttribute
    {

    }
}
