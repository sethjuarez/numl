using System;
using System.IO;
using numl.Utils;
using System.Reflection;
using System.Collections;
using numl.Math.LinearAlgebra;
using System.Globalization;

namespace numl.Serialization
{
    /// <summary>
    /// JSON Writer object.
    /// </summary>
    public class JsonWriter : IDisposable
    {
        private readonly TextWriter _writer;

        /// <summary>
        /// Creates a new JsonWriter from the underlying stream.
        /// </summary>
        /// <param name="writer"></param>
        public JsonWriter(TextWriter writer)
        {
            _writer = writer;
        }

        internal void WriteToken(int token)
        {
            _writer.Write((char)token);
        }

        /// <summary>
        /// Writes a boolean value to the underlying stream.
        /// </summary>
        /// <param name="value"></param>
        public void WriteBool(bool value)
        {
            _writer.Write(value.ToString().ToLower());
        }

        /// <summary>
        /// Writes a string value to the underlying stream.
        /// </summary>
        /// <param name="value"></param>
        public void WriteString(string value)
        {
            var s = value
                        .Replace("\t", "\\t")
                        .Replace("\n", "\\n")
                        .Replace("\r", "\\r");

            _writer.Write($"\"{s}\"");
        }

        /// <summary>
        /// Writes a simple type to the underlying stream (see <seealso cref="TypeHelpers.IsSimpleType(Type, Type[])"/>)
        /// </summary>
        /// <param name="value"></param>
        private void WriteSimpleType(object value)
        {
            _writer.Write(Ject.Convert(value).ToString("r", CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Writes a Vector object to the underlying stream.
        /// </summary>
        /// <param name="v"></param>
        public void WriteVector(Vector v)
        {
            WriteArray(v as IEnumerable);
        }

        /// <summary>
        /// Writes a Matrix object to the underlying stream.
        /// </summary>
        /// <param name="matrix"></param>
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

        /// <summary>
        /// Writes the opening array tag to the underlying stream.
        /// </summary>
        public void WriteBeginArray()
        {
            WriteToken(JsonConstants.BEGIN_ARRAY);
        }

        /// <summary>
        /// Writes the closing array tag to the underlying stream.
        /// </summary>
        public void WriteEndArray()
        {
            WriteToken(JsonConstants.END_ARRAY);
        }

        /// <summary>
        /// Writes an array to the underlying stream.
        /// </summary>
        /// <param name="c"></param>
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

        /// <summary>
        /// Writes a null value tag to the underlying stream.
        /// </summary>
        public void WriteNull()
        {
            _writer.Write(new string(JsonConstants.NULL));
        }

        /// <summary>
        /// Writes an object opening tag to the underlying stream.
        /// </summary>
        public void WriteStartObject()
        {
            WriteToken(JsonConstants.BEGIN_OBJECT);
        }

        /// <summary>
        /// Writes an object closing to the underlying stream.
        /// </summary>
        public void WriteEndObject()
        {
            WriteToken(JsonConstants.END_OBJECT);
        }

        /// <summary>
        /// Writes the first property to the underlying stream.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        internal void WriteFirstProperty(string name, object val)
        {
            Write(name);
            WriteToken(JsonConstants.COLON);
            Write(val);
        }

        /// <summary>
        /// Writes a non-first property to the underlying stream.
        /// </summary>
        /// <param name="name">Property name to use when writing.</param>
        /// <param name="val">Value to write.</param>
        public void WriteProperty(string name, object val)
        {
            WriteToken(JsonConstants.COMMA);
            WriteFirstProperty(name, val);
        }

        /// <summary>
        /// Writes the first array property to the underlying stream.
        /// </summary>
        /// <param name="name">Property name to use when writing.</param>
        /// <param name="val">Value to write.</param>
        public void WriteFirstArrayProperty(string name, IEnumerable val)
        {
            Write(name);
            WriteToken(JsonConstants.COLON);
            Write(val);
        }

        /// <summary>
        /// Writes an array property to the underlying stream.
        /// </summary>
        /// <param name="name">Property name to use when writing.</param>
        /// <param name="val">Value to write.</param>
        public void WriteArrayProperty(string name, IEnumerable val)
        {
            WriteToken(JsonConstants.COMMA);
            WriteFirstArrayProperty(name, val);
        }

        /// <summary>
        /// Writes a raw complex object to the underlying stream.
        /// </summary>
        /// <param name="o"></param>
        public void WriteObject(object o)
        {
            WriteStartObject();
            var first = true;
            foreach (var pi in o.GetType().GetTypeInfo().DeclaredProperties)
            {
                if (!first) WriteToken(JsonConstants.COMMA);

                WriteFirstProperty(pi.Name, pi.GetValue(o));

                first = false;
            }
            WriteEndObject();
        }
        
        /// <summary>
        /// Writes a raw simple object to the underlying stream.
        /// </summary>
        /// <param name="value"></param>
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
					else if (value is Guid) WriteString(value.ToString());
					else WriteSimpleType(value);
                }
                else if (type == typeof(Vector))
                    WriteVector((Vector)value);
                else if (type == typeof(Matrix))
                    WriteMatrix((Matrix)value);
                else if (type.HasSerializer())
                {
                    var serializer = type.GetSerializer();
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

        /// <summary>
        /// Disposes the current JSON writer object.
        /// </summary>
        public void Dispose()
        {
            if (_writer != null)
                _writer.Dispose();
        }
    }
}
