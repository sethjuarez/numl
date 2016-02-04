using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace numl.Serialization
{
    /// <summary>
    /// Json Helpers Class
    /// </summary>
    public static class SerializationHelpers
    {
        /// <summary>
        /// Saves the specified file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file">The file.</param>
        /// <param name="o">The o.</param>
        public static void Save<T>(string file, T o) => 
            Save(file, o, typeof(T));

        /// <summary>
        /// Serialize object to file
        /// </summary>
        /// <param name="file">file</param>
        /// <param name="o">object</param>
        /// <param name="t">type</param>
        public static void Save(string file, object o, Type t = null)
        {
            using (var stream = File.OpenWrite(file))
            using (var writer = new StreamWriter(stream))
                Save(writer, o, t ?? o.GetType());
        }

        /// <summary>Save object to file.</summary>
        /// <tparam name="T">Generic type parameter.</tparam>
        /// <param name="writer">The writer.</param>
        /// <param name="o">object.</param>
        public static void Save<T>(TextWriter writer, T o) => 
            Save(writer, o, typeof(T));

        /// <summary>Save object to file.</summary>
        /// <param name="writer">The stream.</param>
        /// <param name="o">object.</param>
        /// <param name="t">type.</param>
        public static void Save(TextWriter writer, object o, Type t)
        {
            throw new NotImplementedException();
            // TODO: Rewire here...
            //GetSerializer().Serialize(writer, o, t);
        }
        
        /// <summary>Loads.</summary>
        /// <param name="reader">The reader.</param>
        /// <param name="t">type.</param>
        /// <returns>An object.</returns>
        public static object Load(TextReader reader, Type t)
        {
            throw new NotImplementedException();
            //return GetSerializer().Deserialize(reader, t);
        }


        /// <summary>Loads the given stream.</summary>
        /// <tparam name="T">Generic type parameter.</tparam>
        /// <param name="file">file.</param>
        /// <returns>A T.</returns>
        public static T Load<T>(string file) => 
            (T)Load(file, typeof(T));

        /// <summary>Loads.</summary>
        /// <param name="file">file.</param>
        /// <param name="t">type.</param>
        /// <returns>An object.</returns>
        public static object Load(string file, Type t)
        {
            using (var stream = File.OpenRead(file))
            using (var reader = new StreamReader(stream))
                return Load(reader, t);
        }
        

    }
}
