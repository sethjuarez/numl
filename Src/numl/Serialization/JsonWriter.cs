using System;
using System.IO;
using numl.Utils;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using numl.Math.LinearAlgebra;

namespace numl.Serialization
{
    public class JsonWriter : IDisposable
    {
        private readonly TextWriter _writer;
        public JsonWriter(TextWriter writer)
        {
            _writer = writer;
        }

        internal void WriteToken(int token)
        {
            _writer.Write((char)token);
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

        public void WriteVector(Vector v)
        {
            WriteArray(v as IEnumerable);
        }

        public void WriteMatrix(Matrix matrix)
        {
            WriteBeginArray();
            bool first = true;
            foreach (var vector in matrix.GetRows())
            {
                if (!first) WriteToken(JsonConstants.COMMA);
                WriteArray(vector as IEnumerable);
                first = false;

            }
            WriteEndArray();
        }

        public void WriteBeginArray()
        {
            WriteToken(JsonConstants.BEGIN_ARRAY);
        }

        public void WriteEndArray()
        {
            WriteToken(JsonConstants.END_ARRAY);
        }

        public void WriteArray(IEnumerable c)
        {
            WriteBeginArray();
            bool first = true;
            foreach (var item in c)
            {
                if (!first) WriteToken(JsonConstants.COMMA);
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
            WriteToken(JsonConstants.BEGIN_OBJECT);
        }

        public void WriteEndObject()
        {
            WriteToken(JsonConstants.END_OBJECT);
        }

        internal void WriteFirstProperty(string name, object val)
        {
            Write(name);
            WriteToken(JsonConstants.COLON);
            Write(val);
        }

        public void WriteProperty(string name, object val)
        {
            WriteToken(JsonConstants.COMMA);
            WriteFirstProperty(name, val);
        }

        public void WriteFirstArrayProperty(string name, IEnumerable val)
        {
            Write(name);
            WriteToken(JsonConstants.COLON);
            Write(val);
        }

        public void WriteArrayProperty(string name, IEnumerable val)
        {
            WriteToken(JsonConstants.COMMA);
            WriteFirstArrayProperty(name, val);
        }

        public void WriteObject(object o)
        {
            WriteStartObject();

            // TODO: WRITE ISERIALIZER OUT HERE
            bool first = true;
            foreach (var pi in o.GetType().GetProperties())
            {
                if (!first) WriteToken(JsonConstants.COMMA);

                WriteFirstProperty(pi.Name, pi.GetValue(o));

                first = false;
            }
            WriteEndObject();
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
                else if (Ject.CanUseSimpleType(type)) // TODO: This might take some refactoring..
                {
                    if (value is string) WriteString((string)value);
                    else WriteSimpleType(value);
                }
                else if (type == typeof(Vector))
                    WriteVector((Vector)value);
                else if (type == typeof(Matrix))
                    WriteMatrix((Matrix)value);
                else if (JsonConstants.HasSerializer(type))
                {
                    var serializer = JsonConstants.GetSerializer(type);
                    serializer.PreWrite(this);
                    serializer.Write(this, value);
                    serializer.PostWrite(this);
                }
                else if (value is IEnumerable)
                    WriteArray(value as IEnumerable);
                else
                    WriteObject(value);
            }
        }

        public void Dispose()
        {
            if (_writer != null)
                _writer.Dispose();
        }
    }
}
