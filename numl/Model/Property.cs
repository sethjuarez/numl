using System;
using numl.Utils;
using System.Linq;
using System.Collections.Generic;

namespace numl.Model
{
    public class Property
    {
        public Property()
        {
            Start = -1;
        }

        public string Name { get; set; }
        public Type Type { get; set; }
        public virtual int Length { get { return 1; } }
        public int Start { get; set; }
        public bool Discrete { get; set; }

        public virtual void PreProcess(IEnumerable<object> examples)
        {
            return;
        }

        public virtual void PostProcess(IEnumerable<object> examples)
        {
            return;
        }

        public virtual object Convert(double val)
        {
            return FastReflection.Convert(val, this.Type);
        }

        public virtual IEnumerable<double> Convert(object o)
        {
            if (FastReflection.CanUseSimpleType(o.GetType()))
                yield return FastReflection.Convert(o);
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
    }
}
