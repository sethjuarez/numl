using System;
using System.IO;
using numl.Utils;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace numl.Serialization
{
    public class JsonWriter
    {
        private readonly TextWriter _writer;
        public JsonWriter(TextWriter writer)
        {
            _writer = writer;
        }

        public void Write(object value)
        {
            if (value == null)
                WriteNull();
            else
            {
                var type = value.GetType();
                if (type == typeof(bool))
                    WriteBool((bool)value);
                else if (Ject.CanUseSimpleType(type))
                {
                    if (value is string) WriteString((string)value);
                    else WriteSimpleType(value);
                }
                else if (JsonConstants.HasSerializer(type))
                    JsonConstants.GetSerializer(type).Write(_writer, value);
                else if (value is IEnumerable)
                    WriteArray(value as IEnumerable);
                else
                    WriteObject(value);
            }
        }

        public void WriteBool(bool value)
        {
            _writer.Write(value.ToString().ToLower());
        }

        public void WriteString(string value)
        {
            var s = value
                        .Replace("\t", "\\t")
                        .Replace("\n", "\\n")
                        .Replace("\r", "\\r");

            _writer.Write($"\"{s}\"");
        }

        private void WriteSimpleType(object value)
        {
            _writer.Write(Ject.Convert(value).ToString("r"));
        }

        public void WriteBeginArray()
        {
            _writer.Write((char)JsonConstants.BEGIN_ARRAY);
        }

        public void WriteEndArray()
        {
            _writer.Write((char)JsonConstants.END_ARRAY);
        }

        public void WriteArray(IEnumerable c)
        {
            WriteBeginArray();
            bool first = true;
            foreach (var item in c)
            {
                if (!first) _writer.Write($"{(char)JsonConstants.COMMA} ");
                Write(item);
                first = false;
            }
            WriteEndArray();
        }

        public void WriteNull()
        {
            _writer.Write(new string(JsonConstants.NULL));
        }

        public void WriteStartObject()
        {
            _writer.Write((char)JsonConstants.BEGIN_OBJECT);
        }

        public void WriteEndObject()
        {
            _writer.Write((char)JsonConstants.END_OBJECT);
        }

        public void WriteProperty(string name, object val)
        {
            Write(name);
            _writer.Write($" {(char)JsonConstants.COLON} ");
            Write(val);
        }

        public void WriteNextProperty(string name, object val)
        {
            _writer.Write($" {(char)JsonConstants.COMMA} ");
            WriteProperty(name, val);
        }

        public void WriteArrayProperty(string name, IEnumerable val)
        {
            Write(name);
            _writer.Write($" {(char)JsonConstants.COLON} ");
            Write(val);
        }

        public void WriteNextArrayProperty(string name, IEnumerable val)
        {
            _writer.Write($" {(char)JsonConstants.COMMA} ");
            WriteArrayProperty(name, val);
        }

        public void WriteObject(object o)
        {
            _writer.WriteStartObject();

            // TODO: WRITE ISERIALIZER OUT HERE
            bool first = true;
            foreach (var pi in o.GetType().GetProperties())
            {
                if (!first)
                    _writer.Write($"{(char)JsonConstants.COMMA} ");

                _writer.WriteProperty(pi.Name, pi.GetValue(o));

                first = false;
            }
            _writer.WriteEndObject();
        }
    }
}
