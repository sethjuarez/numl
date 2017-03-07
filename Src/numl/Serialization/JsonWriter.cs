using System;
using System.IO;
using numl.Utils;
using System.Reflection;
using System.Collections;
using numl.Math.LinearAlgebra;
using System.Globalization;
using System.Text;

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
            switch(value)
            {
                case null:
                    WriteNull();
                    break;
                case bool b:
                    WriteBool(b);
                    break;
                case string s:
                    WriteString(s);
                    break;
                case Guid g:
                    WriteString(g.ToString());
                    break;
                case Vector v:
                    WriteVector(v);
                    break;
                case Matrix m:
                    WriteMatrix(m);
                    break;
                case IEnumerable e:
                    WriteArray(e);
                    break;
                default:
                    Type type = value.GetType();
                    if (Ject.CanUseSimpleType(type))
                        WriteSimpleType(value);
                    else if (type.HasSerializer())
                    {
                        var serializer = type.GetSerializer();
                        serializer.PreWrite(this);
                        serializer.Write(this, value);
                        serializer.PostWrite(this);
                    }
                    else
                        WriteObject(value);
                    break;
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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string SaveJson<T>(T o)
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
                new JsonWriter(sw).Write(o);
            return sb.ToString();
        }

        public static void Save<T>(T o, string file)
        {
            if (File.Exists(file)) File.Delete(file);

            using (var fs = new FileStream(file, FileMode.CreateNew))
            using (var f = new StreamWriter(fs))
                new JsonWriter(f).Write(o);
        }
    }
}
