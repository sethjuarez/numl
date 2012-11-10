using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Model;

namespace numl.Supervised
{
    public abstract class Generator : IGenerator
    {
        public Descriptor Descriptor { get; set; }

        public IModel Generate(Descriptor description, IEnumerable<object> examples)
        {
            //Description = description;
            //var data = Description.ToExamples(examples);
            //var model = Generate(data.Item1, data.Item2);
            //model.Description = Description;
            //return model;
            throw new NotImplementedException();
        }

        public IModel Generate<T>(IEnumerable<T> examples)
             where T : class
        {
            var descriptor = Descriptor.Create<T>();
            return Generate(descriptor, examples);
        }

        //public abstract IModel Generate(Matrix x, Vector y);
    }
}
