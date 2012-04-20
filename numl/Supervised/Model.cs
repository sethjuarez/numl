using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math;
using numl.Model;

namespace numl.Supervised
{
    public abstract class Model : IModel
    {
        public LabeledDescription Description { get; set; }
        public abstract double Predict(Vector y);

        public object Predict(object o)
        {
            var label = Description.Label;
            var pred = Predict(Description.ToVector(o));
            var val = R.Convert(pred, label.Type);
            R.Set(o, label.Name, val);
            return o;
        }

        public T Predict<T>(T o)
        {
            return (T)Predict((object)o);
        }
    }
}
