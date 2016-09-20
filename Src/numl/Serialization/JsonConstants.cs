using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace numl.Serialization
{
    internal static class JsonConstants
    {
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
    }

}
