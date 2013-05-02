using System;
using numl.Model;
using System.Linq;
using System.Collections.Generic;
using numl.Math;
using System.Net;
using numl.Math.LinearAlgebra;


namespace numl.Supervised
{
    public abstract class Generator : IGenerator
    {
        public Descriptor Descriptor { get; set; }

        public IModel Generate(IEnumerable<object> examples)
        {
            if (Descriptor == null)
                throw new InvalidOperationException("Cannot generate model with empty Descriptor");
            return Generate(Descriptor, examples);
        }

        public IModel Generate(Descriptor description, IEnumerable<object> examples)
        {
            Descriptor = description;
            if (Descriptor.Features == null || Descriptor.Features.Length == 0)
                throw new InvalidOperationException("Invalid descriptor: Empty feature set!");
            if (Descriptor.Label == null)
                throw new InvalidOperationException("Invalid descriptor: Empty label!");

            var doubles = Descriptor.Convert(examples);
            var tuple = doubles.ToExamples();

            return Generate(tuple.Item1, tuple.Item2);
        }

        public IModel Generate<T>(IEnumerable<T> examples)
             where T : class
        {
            var descriptor = Descriptor.Create<T>();
            return Generate(descriptor, examples);
        }

        public abstract IModel Generate(Matrix x, Vector y);
    }
}
