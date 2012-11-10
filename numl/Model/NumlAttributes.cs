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
using numl.Model;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace numl.Model
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public abstract class NumlAttribute : Attribute 
    {
        public virtual Property GenerateProperty(PropertyInfo property)
        {
            Property p;
            Type type = property.PropertyType;
            if (type == typeof(string))
                p = new StringProperty();
            else if (type == typeof(DateTime))
                p = new DateTimeProperty();
            else
                p = new Property();

            p.Type = type;
            p.Name = property.Name;

            return p;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FeatureAttribute : NumlAttribute { }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LabelAttribute : NumlAttribute 
    {
        public override Property GenerateProperty(PropertyInfo property)
        {
            var p = base.GenerateProperty(property);
            if (p is StringProperty) // if it is a label, must be enum
                ((StringProperty)p).AsEnum = true;
            return p;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StringFeatureAttribute : FeatureAttribute
    {
        public StringSplitType SplitType { get; set; }
        public string Separator { get; set; }
        public string ExclusionFile { get; set; }
        public bool AsEnum { get; set; }

        public StringFeatureAttribute()
        {
            AsEnum = false;
            SplitType = StringSplitType.Word;
            Separator = " ";
        }

        public StringFeatureAttribute(StringSplitType splitType, string separator = " ", string exclusions = null)
        {
            SplitType = splitType;
            Separator = separator;
            ExclusionFile = exclusions;
        }

        public StringFeatureAttribute(bool asEnum)
        {
            AsEnum = asEnum;
        }

        public override Property GenerateProperty(PropertyInfo property)
        {
            var sp = new StringProperty
            {
                Name = property.Name,
                SplitType = SplitType,
                Separator = Separator,
                AsEnum = AsEnum
            };

            sp.ImportExclusions(ExclusionFile);

            return sp;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StringLabelAttribute : LabelAttribute { }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateFeatureAttribute : FeatureAttribute
    {
        // TODO: ADD RELEVANT
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ArrayAttribute : FeatureAttribute
    {
        // TODO: ADD RELEVANT
    }
}
