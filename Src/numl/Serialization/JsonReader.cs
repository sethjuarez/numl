using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using numl.Math.LinearAlgebra;
using numl.Utils;

namespace numl.Serialization
{
    public class JsonReader : IDisposable
    {
        private readonly TextReader _reader;
        public JsonReader(TextReader reader)
        {
            _reader = reader;
        }

        public void EatWhitespace()
        {
            while (JsonConstants.WHITESPACE.Contains((char)_reader.Peek()))
                _reader.Read();
        }

        public bool IsNull()
        {
            EatWhitespace();
            return _reader.Peek() == JsonConstants.NULL[0] &&
                   ReadLiteral() == null;
        }

        internal void ReadToken(int token)
        {
            EatWhitespace();
            if (_reader.Read() != token)
                throw new InvalidOperationException($"Invalid token (expected {(char)token})!");
        }

        internal void PeekToken(int token)
        {
            EatWhitespace();
            if (_reader.Peek() == token)
                _reader.Read();
        }

        public string ReadString()
        {
            EatWhitespace();
            ReadToken(JsonConstants.QUOTATION);
            StringBuilder sb = new StringBuilder();
            int cur = _reader.Read();
            while (cur != JsonConstants.QUOTATION)
            {
                if (cur == JsonConstants.ESCAPE)
                {
                    cur = _reader.Read();
                    switch (cur)
                    {
                        case '\\':
                            sb.Append('\\');
                            break;
                        case '/':
                            sb.Append('/');
                            break;
                        case 'b':
                            sb.Append('\b');
                            break;
                        case 'f':
                            sb.Append('\f');
                            break;
                        case 'n':
                            sb.Append('\n');
                            break;
                        case 'r':
                            sb.Append('\r');
                            break;
                        case 't':
                            sb.Append('\t');
                            break;
                        case '"':
                            sb.Append('"');
                            break;
                        case 'u':
                            var hex = new string(new[] { (char)_reader.Read(), (char)_reader.Read(), (char)_reader.Read(), (char)_reader.Read() });
                            sb.Append((char)ushort.Parse(hex, NumberStyles.HexNumber));
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected escape token encountered!");

                    }
                }
                else
                    sb.Append((char)cur);
                cur = _reader.Read();
            }

            return sb.ToString();

        }
        public double ReadNumber()
        {
            EatWhitespace();
            StringBuilder sb = new StringBuilder();
            while (_reader.Peek() > -1 && JsonConstants.NUMBER.Contains((char)_reader.Peek()))
                sb.Append((char)_reader.Read());
            return double.Parse(sb.ToString());
        }
        public object ReadLiteral()
        {
            EatWhitespace();
            var next = _reader.Peek();
            switch (next)
            {
                case 'f':
                    for (int i = 0; i < JsonConstants.FALSE.Length; i++)
                        if (_reader.Read() != JsonConstants.FALSE[i])
                            throw new InvalidOperationException($"Unexpected token parsing \"false\", exected {(char)JsonConstants.FALSE[i]}!");
                    return false;
                case 'n':
                    for (int i = 0; i < JsonConstants.NULL.Length; i++)
                        if (_reader.Read() != JsonConstants.NULL[i])
                            throw new InvalidOperationException($"Unexpected token parsing \"null\", exected {(char)JsonConstants.NULL[i]}!");
                    return null;
                case 't':
                    for (int i = 0; i < JsonConstants.TRUE.Length; i++)
                        if (_reader.Read() != JsonConstants.TRUE[i])
                            throw new InvalidOperationException($"Unexpected token parsing \"true\", exected {(char)JsonConstants.TRUE[i]}!");
                    return true;
                default:
                    throw new InvalidOperationException("Unexpected token when parsing literal!");

            }
        }

        public Vector ReadVector()
        {
            return new Vector(ReadArray().Select(i => (double)i).ToArray());
        }

        public Matrix ReadMatrix()
        {
            return new Matrix(
                ReadArray().Select(i => ((object[])i).Select(j => (double)j).ToArray()).ToArray()
            );
        }

        public object[] ReadArray()
        {
            EatWhitespace();
            ReadToken(JsonConstants.BEGIN_ARRAY);
            List<object> array = new List<object>();
            int token = 0;
            do
            {
                EatWhitespace();

                // empty array situation
                if (_reader.Peek() == JsonConstants.END_ARRAY)
                {
                    _reader.Read();
                    break;
                }

                array.Add(Read());

                EatWhitespace();

                token = _reader.Read();
                if (token != JsonConstants.COMMA && token != JsonConstants.END_ARRAY)
                    throw new InvalidOperationException("Unexpected token while parsing array!");
            }
            while (token != JsonConstants.END_ARRAY);

            return array.ToArray();
        }
        public void ReadStartObject()
        {
            EatWhitespace();
            ReadToken(JsonConstants.BEGIN_OBJECT);
        }

        public void ReadEndObject()
        {
            EatWhitespace();
            ReadToken(JsonConstants.END_OBJECT);
        }

        public JsonProperty ReadProperty()
        {
            var name = ReadString();
            ReadToken(JsonConstants.COLON);
            var value = Read();
            PeekToken(JsonConstants.COMMA);

            return new JsonProperty { Name = name, Value = value };
        }

        public JsonProperty ReadArrayProperty()
        {

            var name = ReadString();
            ReadToken(JsonConstants.COLON);
            var value = ReadArray();
            PeekToken(JsonConstants.COMMA);
            return new JsonProperty { Name = name, Value = value };
        }

        public object ReadObject()
        {
            ReadToken(JsonConstants.BEGIN_OBJECT);
            var obj = new Dictionary<string, object>();
            while (_reader.Peek() != JsonConstants.END_OBJECT)
            {
                var p = ReadProperty();

                if (p.Name == JsonSerializer.SERIALIZER_ATTRIBUTE)
                {
                    // TODO: Maybe keep instances of these things for later reuse
                    var s = (ISerializer)Activator.CreateInstance(Ject.FindType(p.Value.ToString()));
                    var o = s.Read(this);
                    s.PostRead(this);
                    return o;
                }

                if (obj.ContainsKey(p.Name))
                    throw new InvalidOperationException("Key already exists");

                obj[p.Name] = p.Value;
                
            }

            ReadToken(JsonConstants.END_OBJECT);
            return obj;
        }

        public object Read()
        {
            // A JSON value MUST be an object, array, number, or string, 
            // or one of the following three literal names
            //    false null true

            EatWhitespace();

            if (_reader.Peek() == -1)
                return null;

            EatWhitespace();

            var next = _reader.Peek();

            if (next == JsonConstants.BEGIN_OBJECT)
                return ReadObject();
            else if (next == JsonConstants.BEGIN_ARRAY)
                return ReadArray();
            else if (next == JsonConstants.QUOTATION)
                return ReadString();
            else if (char.IsNumber((char)next) || next == '-')
                return ReadNumber();
            else if (next == 'f' || next == 'n' || next == 't')
                return ReadLiteral();
            else
                throw new InvalidOperationException("Unexpected token encountered while parsing json");
        }

        public void Dispose()
        {
            if (_reader != null)
                _reader.Dispose();
        }
    }
}
