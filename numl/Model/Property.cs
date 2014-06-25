using System;
using numl.Utils;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;

namespace numl.Model
{
    /// <summary>
    /// Concrete property. Used to convert
    /// any given data type to a number.
    /// </summary>
    [XmlRoot("Property"), Serializable]
    public class Property : IXmlSerializable
    {
        public Property()
        {
            Start = -1;
        }

        /// <summary>
        /// Property Name - Maps to object property or dictionary lookup
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of property
        /// </summary>
        public virtual Type Type { get; set; }

        /// <summary>
        /// Length of property.
        /// </summary>
        public virtual int Length { get { return 1; } }

        /// <summary>
        /// Start position in array
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Discrete or continuous value
        /// </summary>
        public bool Discrete { get; set; }

        /// <summary>
        /// Used as a preprocessing step when overridden. 
        /// Can be used to look at the entire data set
        /// as a whole before converting single elements.
        /// </summary>
        /// <param name="examples">Examples</param>
        public virtual void PreProcess(IEnumerable<object> examples)
        {
            return;
        }

        /// <summary>
        /// Used as a preprocessing step when overriden. Can be used
        /// to look at the current object in question before 
        /// converting single elements.
        /// </summary>
        /// <param name="example">Example</param>
        public virtual void PreProcess(object example)
        {
            return;
        }

        /// <summary>
        /// Used as a postprocessing step when overridden. 
        /// Can be used to look at the entire data set
        /// as a whole after converting single elements.
        /// </summary>
        /// <param name="examples">Examples</param>
        public virtual void PostProcess(IEnumerable<object> examples)
        {
            return;
        }

        /// <summary>
        /// Used as a postprocessing step when overriden. Can be used
        /// to look at the current object in question fater 
        /// converting single elements.
        /// </summary>
        /// <param name="example"></param>
        public virtual void PostProcess(object example)
        {

        }

        /// <summary>
        /// Convert the numeric representation back 
        /// to the original type.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual object Convert(double val)
        {
            return Ject.Convert(val, this.Type);
        }

        /// <summary>
        /// Convert an object to a list of numbers
        /// </summary>
        /// <param name="o">Object</param>
        /// <returns>Lazy list of doubles</returns>
        public virtual IEnumerable<double> Convert(object o)
        {
            if (Ject.CanUseSimpleType(o.GetType()))
                yield return Ject.Convert(o);
            else
                throw new InvalidOperationException(string.Format("Cannot convert {0} to a double", o.GetType()));
        }

        /// <summary>
        /// Retrieve the list of expanded columns.
        /// If there is a one-to-one correspondence
        /// between the type and its expansion it will return
        /// a single value/
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<string> GetColumns()
        {
            yield return Name;
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}, {2}]", Name, Start, Length);
        }

        // serialization

        public XmlSchema GetSchema()
        {
            return null;
        }

        public virtual void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            Name = reader.GetAttribute("Name");
            Type = Ject.FindType(reader.GetAttribute("Type"));
            Discrete = bool.Parse(reader.GetAttribute("Discrete"));
            Start = int.Parse(reader.GetAttribute("Start"));
        }

        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Name", Name);
            writer.WriteAttributeString("Type", Type.Name);
            writer.WriteAttributeString("Discrete", Discrete.ToString());
            writer.WriteAttributeString("Start", Start.ToString());
        }
    }
}
