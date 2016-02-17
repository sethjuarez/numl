using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Serialization
{
    internal static class JsonConstants
    {
        static JsonConstants()
        {
            // type magic to selfr register all available
            // ISerializers
            var serializers =
                from t in typeof(ISerializer).Assembly.GetTypes()
                where typeof(ISerializer).IsAssignableFrom(t) &&
                      t != typeof(ISerializer)
                select (ISerializer)Activator.CreateInstance(t);

            _serializers = new List<ISerializer>(serializers);
        }

        //begin-array     = ws %x5B ws  ; [ left square bracket
        internal const int BEGIN_ARRAY = '[';
        //begin-object    = ws %x7B ws; { left curly bracket
        internal const int BEGIN_OBJECT = '{';
        //end-array       = ws %x5D ws; ] right square bracket
        internal const int END_ARRAY = ']';
        //end-object      = ws %x7D ws; } right curly bracket
        internal const int END_OBJECT = '}';
        //name-separator  = ws %x3A ws; : colon
        internal const int COLON = ':';
        //value-separator = ws %x2C ws; , comma
        internal const int COMMA = ',';
        // "
        internal const int QUOTATION = '"';
        // \
        internal const int ESCAPE = '\\';

        internal readonly static char[] FALSE = new[] { 'f', 'a', 'l', 's', 'e' };
        internal readonly static char[] TRUE = new[] { 't', 'r', 'u', 'e' };
        internal readonly static char[] NULL = new[] { 'n', 'u', 'l', 'l' };
        internal readonly static char[] WHITESPACE = new[] { ' ', '\t', '\n', '\r' };
        internal readonly static char[] NUMBER = new[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.', '-', '+', 'e', 'E' };

        private static readonly List<ISerializer> _serializers;
        internal static ISerializer GetSerializer(Type type)
        {
            var q = _serializers.Where(s => s.CanConvert(type));
            if (q.Count() > 1)
            {
                var s = q.ToArray();
                if (s[0].GetType().IsSubclassOf(s[1].GetType()))
                    return s[0];
                else
                    return s[1];
            }

            return q.First();
        }

        internal static bool HasSerializer(Type type)
        {
            return _serializers.Where(s => s.CanConvert(type))
                               .Count() > 0;
        }
    }

}
