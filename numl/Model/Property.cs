using System;
using numl.Utils;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;

namespace numl.Model
{
    [XmlRoot("Property"), Serializable]
    public class Property : IXmlSerializable
    {
        public Property()
        {
            Start = -1;
        }

        public string Name { get; set; }
        public virtual Type Type { get; set; }
        public virtual int Length { get { return 1; } }
        public int Start { get; set; }
        public bool Discrete { get; set; }

        public virtual void PreProcess(IEnumerable<object> examples)
        {
            return;
        }

        public virtual void PreProcess(object example)
        {
            return;
        }

        public virtual void PostProcess(IEnumerable<object> examples)
        {
            return;
        }

        public virtual void PostProcess(object example)
        {

        }

        public virtual object Convert(double val)
        {
            return Ject.Convert(val, this.Type);
        }

        public virtual IEnumerable<double> Convert(object o)
        {
            if (Ject.CanUseSimpleType(o.GetType()))
                yield return Ject.Convert(o);
            else
                throw new InvalidOperationException(string.Format("Cannot convert {0} to a double", o.GetType()));
        }

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
