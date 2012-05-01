using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Model;
using numl.Math;

namespace numl.Supervised
{
    public abstract class Generator : IGenerator
    {
        public Description Description { get; set; }

        public IModel Generate(Description description, IEnumerable<object> examples)
        {
            Description = description;
            var data = Description.ToExamples(examples);
            var model = Generate(data.Item1, data.Item2);
            model.Description = Description;
            return model;
        }

        public abstract IModel Generate(Matrix x, Vector y);
    }
}
