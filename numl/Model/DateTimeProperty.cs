using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace numl.Model
{
    /// <summary>
    /// Features available for the
    /// DateTime property
    /// </summary>
    [Flags]
    public enum DateTimeFeature
    {
        /// <summary>
        /// Year
        /// </summary>
        Year = 0x0001,
        /// <summary>
        /// Day of the year (1, 366)
        /// </summary>
        DayOfYear = 0x0002,
        /// <summary>
        /// Month
        /// </summary>
        Month = 0x0008,
        /// <summary>
        /// Day
        /// </summary>
        Day = 0x0010,
        /// <summary>
        /// Day of the week (0, 6)
        /// </summary>
        DayOfWeek = 0x0020,
        /// <summary>
        /// Hour
        /// </summary>
        Hour = 0x0040,
        /// <summary>
        /// Minute
        /// </summary>
        Minute = 0x0080,
        /// <summary>
        /// Second
        /// </summary>
        Second = 0x0100,
        /// <summary>
        /// Millisecond
        /// </summary>
        Millisecond = 0x0200
    }

    /// <summary>
    /// Date portions available for
    /// the DateTime property
    /// </summary>
    [Flags]
    public enum DatePortion
    {
        /// <summary>
        /// Date (Jan. 1, 2000) -> [1, 1, 2000]
        /// </summary>
        Date = 0x0001,
        /// <summary>
        /// Extended Date (Jan. 1, 2000) -> [1, 6] (DayOfYear, DayOfWeek)
        /// </summary>
        DateExtended = 0x0002,
        /// <summary>
        /// Time 4:45pm -> [16, 45] (Hour, Minute)
        /// </summary>
        Time = 0x0008,
        /// <summary>
        /// Extended Time 4:45pm -> [0, 0] (Second, Millisecond)
        /// </summary>
        TimeExtended = 0x0010
    }

    [XmlRoot("DateTimeProperty"), Serializable]
    public class DateTimeProperty : Property
    {
        public DateTimeFeature Features { get; private set; }

        public DateTimeProperty()
            : base()
        {
            Initialize(DatePortion.Date | DatePortion.DateExtended);
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

        public override object Convert(double val)
        {
            return val;
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

        public override IEnumerable<string> GetColumns()
        {
            if (Features.HasFlag(DateTimeFeature.Year))
                yield return "Year";
            if (Features.HasFlag(DateTimeFeature.DayOfYear))
                yield return "DayOfYear";
            if (Features.HasFlag(DateTimeFeature.Month))
                yield return "Month";
            if (Features.HasFlag(DateTimeFeature.Day))
                yield return "Day";
            if (Features.HasFlag(DateTimeFeature.DayOfWeek))
                yield return "DayOfWeek";
            if (Features.HasFlag(DateTimeFeature.Hour))
                yield return "Hour";
            if (Features.HasFlag(DateTimeFeature.Minute))
                yield return "Minute";
            if (Features.HasFlag(DateTimeFeature.Second))
                yield return "Second";
            if (Features.HasFlag(DateTimeFeature.Millisecond))
                yield return "Millisecond";
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

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
            writer.WriteAttributeString("Features", ((int)Features).ToString());
        }

        public override void ReadXml(XmlReader reader)
        {
            base.ReadXml(reader);
            Features = (DateTimeFeature)int.Parse(reader.GetAttribute("Features"));
        }
    }
}
