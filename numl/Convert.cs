using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math;
using numl.Model;
using System.Data;
using System.ComponentModel;
using numl.Exceptions;
using System.Reflection;
using System.Dynamic;

namespace numl
{
    public static class Convert
    {
        public static Vector ToVector(this object o, Description description)
        {
            Property[] features = description.Features;
            if (description == null || features == null || features.Length == 0)
                throw new InvalidDescriptionException();


            Type type = o.GetType();
            Vector vector = Vector.Zeros(features.Length);

            for (int i = 0; i < features.Length; i++)
            {
                object val = GetItem(o, features[i].Name);
                vector[i] = ConvertObject(val);
            }

            return vector;
        }

        public static Matrix ToMatrix(this IEnumerable<object> objects, Description description)
        {
            Property[] features = description.Features;
            if (description == null || features == null || features.Length == 0)
                throw new InvalidDescriptionException();

            int rows = objects.Count();
            int cols = features.Length;
            double[][] matrix = new double[rows][];

            int i = -1;
            foreach (object o in objects)
            {
                matrix[++i] = new double[cols];
                for (int j = 0; j < cols; j++)
                {
                    object val = GetItem(o, features[j].Name);
                    matrix[i][j] = ConvertObject(val);
                }
            }

            return new Matrix(matrix);

        }

        #region Internal Conversion Utility Methods
        /// <summary>
        /// Get a property or dictionary value in an object by name
        /// </summary>
        /// <param name="o">object</param>
        /// <param name="name">key</param>
        /// <returns>value of keyed item</returns>
        internal static object GetItem(object o, string name)
        {
            var type = o.GetType();
            object val = null;
            var prop = type.GetProperty(name);
            if (prop == null)
            {
                // treat as string indexable for objects with
                // o["Feature"] type lookups.
                var method = type.GetMethod("get_Item", new Type[] { typeof(string) });
                if (method == null)
                    throw new InvalidOperationException(string.Format("Could not find a property named \"{0}\" in containing object.", name));

                val = method.Invoke(o, new object[] { name });
            }
            else
                val = prop.GetValue(o, new object[] { });

            return val;
        }

        internal static double ConvertBoolPlusMinus(bool o)
        {
            return o ? 1 : -1;
        }

        internal static double ConvertBoolOneZero(bool o)
        {
            return o ? 1 : 0;
        }

        internal static double ConvertEnum(Enum o)
        {
            return (int)System.Convert.ChangeType(o, typeof(int));
        }

        internal static double ConvertString(string o)
        {
            return o.Length;
        }

        internal static double ConvertNumber(object o)
        {
            if (o.GetType() == typeof(double))
                return (double)o;
            else if (o.GetType() == typeof(char))
                return (int)Encoding.ASCII.GetBytes(new char[] { (char)o })[0];
            else
            {
                var converter = TypeDescriptor.GetConverter(o.GetType());
                if (converter.CanConvertTo(typeof(double)))
                    return (double)converter.ConvertTo(o, typeof(double));
                else
                    throw new InvalidOperationException(string.Format("Cannot convert {0} to double!", o.GetType()));
            }
        }

        internal static double ConvertObject(object o)
        {
            Type t = o.GetType();
            TypeConverter converter = TypeDescriptor.GetConverter(t);
            if (converter.CanConvertTo(typeof(double)) || t == typeof(char))
                return ConvertNumber(o);
            else
            {
                if (t == typeof(string))
                    return ConvertString((string)o);
                else if (t == typeof(bool))
                    return ConvertBoolPlusMinus((bool)o);
                else if (t.BaseType == typeof(Enum))
                    return ConvertEnum((Enum)o);
                else
                    throw new InvalidCastException(string.Format("Cannot convert {0} to double", t));
            }
        }
        #endregion
    }
}
