using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace numl.Utils
{
    /// <summary>
    /// Xml serialization helper
    /// </summary>
    public static class Xml
    {
        /// <summary>
        /// Save object to file
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="file">file</param>
        /// <param name="o">object</param>
        public static void Save<T>(string file, T o)
        {
            using (var stream = File.OpenWrite(file))
                Save(stream, o, typeof(T));
        }

        /// <summary>
        /// Save object to file
        /// </summary>
        /// <param name="file">file</param>
        /// <param name="o">object</param>
        /// <param name="t">type</param>
        public static void Save(string file, object o, Type t)
        {
            using (var stream = File.OpenWrite(file))
                Save(stream, o, t);
        }

        public static void Save<T>(Stream stream, T o)
        {
            Save(stream, o, typeof(T));
        }

        public static void Save(Stream stream, object o, Type t)
        {
            XmlSerializer serializer = new XmlSerializer(t);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            serializer.Serialize(stream, o, ns);
        }

        public static string ToXmlString<T>(T o)
        {
            return ToXmlString(o, typeof(T));
        }

        public static string ToXmlString(object o, Type t)
        {
            XmlSerializer serializer = new XmlSerializer(t);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            StringWriter textWriter = new StringWriter();
            ns.Add("", "");

            serializer.Serialize(textWriter, o, ns);
            return textWriter.ToString();
        }

        public static T Load<T>(string file)
        {
            using (var stream = File.OpenRead(file))
                return Load<T>(stream);
        }

        public static object Load(string file, Type t)
        {
            using (var stream = File.OpenRead(file))
                return Load(stream, t);
        }

        public static T Load<T>(Stream stream)
        {
            var o = Load(stream, typeof(T));
            return (T)o;
        }

        public static object Load(Stream stream, Type t)
        {
            XmlSerializer serializer = new XmlSerializer(t);
            var o = serializer.Deserialize(stream);
            return o;
        }

        public static T LoadXmlString<T>(string xml)
        {
            var o = LoadXmlString(xml, typeof(T));
            return (T)o;
        }

        public static object LoadXmlString(string xml, Type t)
        {
            TextReader reader = new StringReader(xml);
            XmlSerializer serializer = new XmlSerializer(t);
            var o = serializer.Deserialize(reader);
            return o;
        }

        public static void Write<T>(XmlWriter writer, T thing)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            serializer.Serialize(writer, thing, ns);
        }

        public static T Read<T>(XmlReader reader)
        {
            XmlSerializer dserializer = new XmlSerializer(typeof(T));
            T item = (T)dserializer.Deserialize(reader);
            // move to next thing
            reader.Read();
            return item;
        }
    }
}
