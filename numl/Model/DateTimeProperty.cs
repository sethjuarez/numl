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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Collections;

namespace numl.Model
{
    [Flags]
    public enum DateTimeFeature
    {
        Year = 0x0001,
        DayOfYear = 0x0002,
        Month = 0x0008,
        Day = 0x0010,
        DayOfWeek = 0x0020,
        Hour = 0x0040,
        Minute = 0x0080,
        Second = 0x0100,
        Millisecond = 0x0200
    }

    [Flags]
    public enum DatePortion
    {
        Date = 0x0001,
        DateExtended = 0x0002,
        Time = 0x0008,
        TimeExtended = 0x0010
    }

    public class DateTimeProperty : Property
    {
        public DateTimeFeature Features { get; private set; }

        public DateTimeProperty()
            : base()
        {
            Initialize(DatePortion.DateExtended);
        }

        public DateTimeProperty(DatePortion portion)
            : base()
        {
            Initialize(portion);
        }


        public DateTimeProperty(DateTimeFeature features)
            : base()
        {
            Type = typeof(DateTime);
            Features = features;
        }

        private int _length = -1;
        public override int Length
        {
            get
            {
                if (_length == -1)
                {
                    _length = 0;
                    if (Features.HasFlag(DateTimeFeature.Year))
                        _length++;
                    if (Features.HasFlag(DateTimeFeature.DayOfYear))
                        _length++;
                    if (Features.HasFlag(DateTimeFeature.Month))
                        _length++;
                    if (Features.HasFlag(DateTimeFeature.Day))
                        _length++;
                    if (Features.HasFlag(DateTimeFeature.DayOfWeek))
                        _length++;
                    if (Features.HasFlag(DateTimeFeature.Hour))
                        _length++;
                    if (Features.HasFlag(DateTimeFeature.Minute))
                        _length++;
                    if (Features.HasFlag(DateTimeFeature.Second))
                        _length++;
                    if (Features.HasFlag(DateTimeFeature.Millisecond))
                        _length++;
                }

                return _length;
            }
        }

        private void Initialize(DatePortion portion)
        {
            Type = typeof(DateTime);
            Features = 0;
            if (portion.HasFlag(DatePortion.Date))
                Features |= DateTimeFeature.Year | DateTimeFeature.Month |
                           DateTimeFeature.Day;

            if (portion.HasFlag(DatePortion.DateExtended))
                Features |= DateTimeFeature.DayOfYear | DateTimeFeature.DayOfWeek;

            if (portion.HasFlag(DatePortion.Time))
                Features |= DateTimeFeature.Hour | DateTimeFeature.Minute;

            if (portion.HasFlag(DatePortion.TimeExtended))
                Features |= DateTimeFeature.Second | DateTimeFeature.Millisecond;
        }

        public override IEnumerable<double> Convert(object o)
        {
            if (o.GetType() == typeof(DateTime))
            {
                // tedious I know...
                // be thankful I wrote it for you...
                var d = (DateTime)o;
                if (Features.HasFlag(DateTimeFeature.Year))
                    yield return d.Year;
                if (Features.HasFlag(DateTimeFeature.DayOfYear))
                    yield return d.DayOfYear;
                if (Features.HasFlag(DateTimeFeature.Month))
                    yield return d.Month;
                if (Features.HasFlag(DateTimeFeature.Day))
                    yield return d.Day;
                if (Features.HasFlag(DateTimeFeature.DayOfWeek))
                    yield return (int)d.DayOfWeek;
                if (Features.HasFlag(DateTimeFeature.Hour))
                    yield return d.Hour;
                if (Features.HasFlag(DateTimeFeature.Minute))
                    yield return d.Minute;
                if (Features.HasFlag(DateTimeFeature.Second))
                    yield return d.Second;
                if (Features.HasFlag(DateTimeFeature.Millisecond))
                    yield return d.Millisecond;
            }
            else throw new InvalidCastException("Object is not a date");
        }

    }
}
