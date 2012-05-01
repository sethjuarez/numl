using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.ComponentModel;

namespace numl
{
    /// <summary>
    /// This class is used for fast reflection over types.
    /// </summary>
    public static class R
    {
        private static Dictionary<Type, Dictionary<string, Func<object, object>>> accessors =
            new Dictionary<Type, Dictionary<string, Func<object, object>>>();

        private static Dictionary<Type, Dictionary<string, Action<object, object>>> setters =
            new Dictionary<Type, Dictionary<string, Action<object, object>>>();

        internal static Func<object, object> GetAccessor(Type type, string valueName)
        {
            Func<object, object> result;
            Dictionary<string, Func<object, object>> typeAccessors;

            // Let's see if we have an accessor already for this type/valueName
            if (accessors.TryGetValue(type, out typeAccessors))
                if (typeAccessors.TryGetValue(valueName, out result))
                    return result;

            // okay, create one and store it for later
            result = CreateAccessor(type, valueName);
            if (typeAccessors == null)
            {
                typeAccessors = new Dictionary<string, Func<object, object>>();
                accessors[type] = typeAccessors;
            }

            typeAccessors[valueName] = result;
            return result;
        }

        internal static Func<object, object> CreateAccessor(Type type, string valueName)
        {
            var prop = type.GetProperty(valueName);
            if (prop != null) // std property
            {
                var param = Expression.Parameter(typeof(object), "o");
                var exp = Expression.Lambda<Func<object, object>>(
                            Expression.Convert(
                                Expression.Property(Expression.Convert(param, type), prop), typeof(object)),
                                param);

                return exp.Compile();
            }
            else // dictionary type access
            {
                var method = type.GetMethod("get_Item", new Type[] { typeof(string) });
                if (method == null)
                    throw new InvalidOperationException(string.Format("Could not find a property named \"{0}\" in containing object.", valueName));

                var param = Expression.Parameter(typeof(object), "o");
                // form method call with closure over name argument
                var exp = Expression.Lambda<Func<object, object>>(
                                Expression.Call(
                                    Expression.Convert(param, type), method,
                                    Expression.Constant(valueName, typeof(string))),
                                param);

                return exp.Compile();
            }
        }

        internal static Action<object, object> GetSetter(Type type, string valueName)
        {
            Action<object, object> result;
            Dictionary<string, Action<object, object>> typeSetters;

            // Let's see if we have an accessor already for this type/valueName
            if (setters.TryGetValue(type, out typeSetters))
                if (typeSetters.TryGetValue(valueName, out result))
                    return result;

            // okay, create one and store it for later
            result = CreateSetter(type, valueName);
            if (typeSetters == null)
            {
                typeSetters = new Dictionary<string, Action<object, object>>();
                setters[type] = typeSetters;
            }

            typeSetters[valueName] = result;
            return result;
        }

        internal static Action<object, object> CreateSetter(Type type, string valueName)
        {
            var prop = type.GetProperty(valueName);
            if (prop != null) // std property
            {
                var param = Expression.Parameter(typeof(object), "o");
                var pass = Expression.Parameter(typeof(object), "value");

                var exp = Expression.Lambda<Action<object, object>>(
                            Expression.Assign(
                                Expression.Property(Expression.Convert(param, type), prop),
                                Expression.Convert(pass, prop.PropertyType)
                            ), param, pass);

                return exp.Compile();
            }
            else // dictionary type access
            {
                var method = type.GetMethod("set_Item", new Type[] { typeof(string), typeof(object) });
                if (method == null)
                    throw new InvalidOperationException(string.Format("Could not find a property named \"{0}\" in containing object.", valueName));

                var param = Expression.Parameter(typeof(object), "o");
                var pass = Expression.Parameter(typeof(object), "value");

                // form method call with closure over name argument
                var exp = Expression.Lambda<Action<object, object>>(
                                Expression.Call(
                                    Expression.Convert(param, type),
                                    method,
                                    Expression.Constant(valueName, typeof(string)),
                                    pass),
                                param, pass);

                return exp.Compile();
            }
        }

        public static object Get(object o, string name)
        {
            var type = o.GetType();
            var accessor = GetAccessor(type, name);
            return accessor.Invoke(o);
        }

        public static void Set(object o, string name, object value)
        {
            var type = o.GetType();
            var setter = GetSetter(type, name);
            setter.Invoke(o, value);
        }

        public static bool CanUseType(Type t)
        {
            if (t == typeof(string))
                return true;
            else if (t == typeof(bool))
                return true;
            else if (t == typeof(char))
                return true;
            //else if (t == typeof(DateTime))
            //    return true;
            else if (t.BaseType == typeof(Enum))
                return true;
            else
                return TypeDescriptor.GetConverter(t).CanConvertTo(typeof(double));
        }

        public static double Convert(object o)
        {
            var t = o.GetType();
            if (t == typeof(char))
                return (double)Encoding.ASCII.GetBytes(new char[] { (char)o })[0];
            else if (t == typeof(bool))
                return (bool)o ? 1d : -1d;
            else if (t.BaseType == typeof(Enum))
                return (int)o; //System.Convert.ChangeType(o, typeof(int));
            else if (t == typeof(string))
                return (double)o.ToString().Length;
            else
            {
                TypeConverter converter = TypeDescriptor.GetConverter(t);
                if (converter.CanConvertTo(typeof(double)))
                    return (double)converter.ConvertTo(o, typeof(double));
                else
                    throw new InvalidCastException(string.Format("Cannot convert {0} to double", o));
            }
        }

        public static object Convert(double val, Type t)
        {
            if (t == typeof(char))
                return (char)((int)val);
            else if (t == typeof(bool))
                return val >= 0;
            else if (t.BaseType == typeof(Enum))
                return Enum.ToObject(t, System.Convert.ChangeType(val, System.Enum.GetUnderlyingType(t)));
            else
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(double));
                if (converter.CanConvertTo(t))
                    return converter.ConvertTo(val, t);
                else
                    throw new InvalidCastException(string.Format("Cannot convert {0} to {1}", val, t.Name));
            }
        }

        public static Type FindType(string s)
        {
            var type = Type.GetType(s);

            if (type != null) return type;

            else // need to look elsewhere
            {
                // someones notational laziness causes me to look
                // everywhere... sorry... I know it's slow...
                var q = (from p in AppDomain.CurrentDomain.GetAssemblies()
                         from t in p.GetTypes()
                         where t.FullName == s || t.Name == s
                         select t).ToArray();

                if (q.Length == 1)
                    return q[0];
                else
                    throw new TypeLoadException(string.Format("Cannot find type {0}", s));
            }

        }
    }
}
