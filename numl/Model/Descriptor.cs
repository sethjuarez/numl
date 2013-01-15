using System;
using numl.Utils;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using numl.Math.LinearAlgebra;
using System.Text;

namespace numl.Model
{
    public class Descriptor
    {
        public Property[] Features { get; set; }
        public Property Label { get; set; }
        public IEnumerable<string> GetColumns()
        {
            foreach (var p in Features)
                foreach (var s in p.GetColumns())
                    yield return s;
        }

        private int _vectorLength = -1;
        /// <summary>
        /// total feature count (could be more
        /// than Features.Length)
        /// </summary>
        public int VectorLength
        {
            get
            {
                if (_vectorLength < 0)
                    _vectorLength = Features.Select(f => f.Length).Sum();

                return _vectorLength;
            }
        }
        public static Type Type { get; set; }

        public Property At(int i)
        {
            if (i < 0 || i > VectorLength)
                throw new IndexOutOfRangeException(string.Format("{0} falls outside of the appropriate range", i));

            var q = (from p in Features
                     where i >= p.Start && i < p.Start + p.Length
                     select p);

            return q.First();
        }

        public string ColumnAt(int i)
        {
            var prop = At(i);
            var offset = i - prop.Start;
            var col = prop.GetColumns().ElementAt(offset);
            return col;
        }

        public IEnumerable<double> Convert(object item)
        {
            return Convert(item, true);
        }

        public IEnumerable<double> Convert(object item, bool withLabel)
        {
            if (Features.Length == 0)
                throw new InvalidOperationException("Cannot convert item with an empty Feature set.");

            for (int i = 0; i < Features.Length; i++)
            {
                // current feature
                var feature = Features[i];

                // start start position
                if (feature.Start < 0)
                    feature.Start = i == 0 ? 0 : Features[i - 1].Start + Features[i - 1].Length;

                // retrieve item
                var o = FastReflection.Get(item, feature.Name);

                // convert item
                foreach (double val in feature.Convert(o))
                    yield return val;
            }

            // convert label (if available)
            if (Label != null && withLabel)
                foreach (double val in Label.Convert(FastReflection.Get(item, Label.Name)))
                    yield return val;

        }

        public IEnumerable<IEnumerable<double>> Convert(IEnumerable<object> items)
        {
            // Pre processing items
            foreach (Property feature in Features)
                feature.PreProcess(items);
            if (Label != null)
                Label.PreProcess(items);

            // convert items
            foreach (object o in items)
                yield return Convert(o);

            // Post processing items
            foreach (Property feature in Features)
                feature.PostProcess(items);
            if (Label != null)
                Label.PostProcess(items);
        }

        public Tuple<Matrix, Vector> ToExamples(IEnumerable<object> examples)
        {
            return Convert(examples).ToExamples();
        }

        public Matrix ToMatrix(IEnumerable<object> examples)
        {
            return Convert(examples).ToMatrix();
        }

        public static Descriptor Create<T>()
            where T : class
        {
            return Create(typeof(T));
        }

        public static Descriptor Create(Type t)
        {
            Type = t;
            if (!t.IsClass)
                throw new InvalidOperationException("Can only work with class types");

            List<Property> features = new List<Property>();
            Property label = null;

            foreach (PropertyInfo property in t.GetProperties())
            {
                var item = property.GetCustomAttributes(typeof(NumlAttribute), false);

                if (item.Length == 1)
                {
                    var attrib = (NumlAttribute)item[0];

                    // generate appropriate property from attribute
                    Property p = attrib.GenerateProperty(property);

                    // feature
                    if (attrib.GetType().IsSubclassOf(typeof(FeatureAttribute)) ||
                        attrib is FeatureAttribute)
                        features.Add(p);
                    // label
                    else if (attrib.GetType().IsSubclassOf(typeof(LabelAttribute)) ||
                        attrib is LabelAttribute)
                    {
                        if (label != null)
                            throw new InvalidOperationException("Cannot have multiple labels in a class");
                        label = p;
                    }
                }
            }

            return new Descriptor
            {
                Features = features.ToArray(),
                Label = label
            };
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("Descriptor ({0}) {{", Type.Name));
            for (int i = 0; i < Features.Length; i++)
                sb.AppendLine(string.Format("   {0}", Features[i]));
            if (Label != null)
                sb.AppendLine(string.Format("  *{0}", Label));

            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}