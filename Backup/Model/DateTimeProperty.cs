// file:	Model\DateTimeProperty.cs
//
// summary:	Implements the date time property class
using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace numl.Model
{
    /// <summary>Features available for the DateTime property.</summary>
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

    /// <summary>Date portions available for the DateTime property.</summary>
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

    /// <summary>DateTime Property. Used as a feature expansion mechanism.</summary>
    [XmlRoot("DateTimeProperty"), Serializable]
    public class DateTimeProperty : Property
    {
        /// <summary>Gets or sets the features.</summary>
        /// <value>The features.</value>
        public DateTimeFeature Features { get; private set; }

        /// <summary>Default constructor.</summary>
        public DateTimeProperty()
            : base()
        {
            Initialize(DatePortion.Date | DatePortion.DateExtended);
        }
        /// <summary>Constructor.</summary>
        /// <param name="portion">The portion.</param>
        public DateTimeProperty(DatePortion portion)
            : base()
        {
            Initialize(portion);
        }
        /// <summary>Constructor.</summary>
        /// <param name="features">The features.</param>
        public DateTimeProperty(DateTimeFeature features)
            : base()
        {
            Type = typeof(DateTime);
            Features = features;
        }

        /// <summary>The length.</summary>
        private int _length = -1;
        /// <summary>Length of property.</summary>
        /// <value>The length.</value>
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
        /// <summary>Convert the numeric representation back to the original type.</summary>
        /// <param name="val">.</param>
        /// <returns>An object.</returns>
        public override object Convert(double val)
        {
            return val;
        }
        /// <summary>Initializes this object.</summary>
        /// <param name="portion">The portion.</param>
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
        /// <summary>
        /// Retrieve the list of expanded columns. If there is a one-to-one correspondence between the
        /// type and its expansion it will return a single value/.
        /// </summary>
        /// <returns>
        /// An enumerator that allows foreach to be used to process the columns in this collection.
        /// </returns>
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
        /// <summary>Convert an object to a list of numbers.</summary>
        /// <exception cref="InvalidCastException">Thrown when an object cannot be cast to a required
        /// type.</exception>
        /// <param name="o">Object.</param>
        /// <returns>Lazy list of doubles.</returns>
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
        /// <summary>Converts an object into its XML representation.</summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is
        /// serialized.</param>
        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
            writer.WriteAttributeString("Features", ((int)Features).ToString());
        }
        /// <summary>Generates an object from its XML representation.</summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is
        /// deserialized.</param>
        public override void ReadXml(XmlReader reader)
        {
            base.ReadXml(reader);
            Features = (DateTimeFeature)int.Parse(reader.GetAttribute("Features"));
        }
    }
}
