/*
 Copyright (c) 2012 Seth Juarez

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 THE SOFTWARE.
*/

using System;
using numl.Math;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;
using numl.Math.Information;
using numl.Model;


namespace numl.Supervised
{
    public class DecisionTreeGenerator : IGenerator
    {
        public int Depth { get; set; }
        public int Width { get; set; }
        public double Hint { get; set; }
        public ImpurityType Type { get; set; }
        public LabeledDescription Description { get; set; }

        public DecisionTreeGenerator(
            int depth = 5, 
            int width = 2, 
            LabeledDescription description = null,
            ImpurityType type = ImpurityType.Entropy, 
            double hint = double.Epsilon)
        {
            if (width < 2)
                throw new InvalidOperationException("Cannot set dt tree width to less than 2!");

            Description = description;
            Depth = depth;
            Width = width;
            Type = type;
            Hint = hint;
        }

        public IModel Generate(LabeledDescription description, IEnumerable<object> examples)
        {
            Description = description;
            var data = examples.ToExamples(Description);
            return Generate(data.Item1, data.Item2);
        }

        public IModel Generate(Matrix x, Vector y)
        {
            if (Description == null)
                throw new InvalidOperationException("Cannot build decision tree without type knowledge!");

            var n = BuildTree(x, y, Depth, new List<int>(x.Cols));

            return new DecisionTreeModel
            {
                Description = Description,
                Tree = n,
                Hint = Hint
            };
        }

        private Node BuildTree(Matrix x, Vector y, int depth, List<int> used)
        {
            // reached depth limit or all labels are the same
            if (depth < 0 || y.Distinct().Count() == 1)
                return new Node { IsLeaf = true, Label = y.Mode() };

            double bestGain = -1;
            int bestFeature = -1;
            double[] splitValues = new double[] { };
            Impurity measure = null;

            for (int i = 0; i < x.Cols; i++)
            {
                var feature = x[i, VectorType.Column];
                var fd = Description.Features[i];

                // is feature discrete? ie enum or bool or string?
                var discrete = fd.Type == ItemType.Enumeration 
                                            || fd.Type == ItemType.Boolean
                                            || fd.Type == ItemType.String;

                switch (Type)
                {
                    case ImpurityType.Error:
                        if (!discrete)
                            measure = Error.Of(y)
                                        .Given(feature)
                                        .WithWidth(Width);
                        else
                            measure = Error.Of(y)
                                        .Given(feature);
                        break;
                    case ImpurityType.Entropy:
                        if (!discrete)
                            measure = Entropy.Of(y)
                                        .Given(feature)
                                        .WithWidth(Width);
                        else
                            measure = Entropy.Of(y)
                                        .Given(feature);
                        break;
                    case ImpurityType.Gini:
                        if (!discrete)
                            measure = Gini.Of(y)
                                         .Given(feature)
                                         .WithWidth(Width);
                        else
                            measure = Gini.Of(y)
                                         .Given(feature);
                        break;
                }

                double gain = measure.RelativeGain();

                if (gain > bestGain && !used.Contains(i))
                {
                    bestGain = gain;
                    bestFeature = i;
                    splitValues = measure.SplitValues;
                }
            }

            // uh oh, need to return something?
            // a weird node of some sort...
            // but just in case...
            if (bestFeature == -1)
                return new Node { IsLeaf = true, Label = y.Mode() };

            used.Add(bestFeature);
            Node n = new Node();
            n.Gain = bestGain;
            // measure has a width property set
            // meaning its a continuous var
            // (second conditional indicates
            //  a width that has range values)

            var bestFD = Description.Features[bestFeature];

            // multiway split - constant fan-out width (non-continuous)
            if (bestFD.Type == ItemType.Enumeration || 
                bestFD.Type == ItemType.Boolean ||
                bestFD.Type == ItemType.String)
            {
                n.Children = new Node[splitValues.Length];
                for (int i = 0; i < n.Children.Length; i++)
                {
                    var slice = x.Indices(v => v[bestFeature] == splitValues[i], VectorType.Row);
                    n.Children[i] = BuildTree(x.Slice(slice, VectorType.Row), y.Slice(slice), depth - 1, used);
                }
                n.Segmented = false;
            }
            // continuous split with built in ranges
            else
            {
                // since this is in ranges, need each slot
                // represents two boundaries
                n.Children = new Node[measure.Width];
                for (int i = 0; i < n.Children.Length; i++)
                {
                    var slice = x.Indices(
                        v => v[bestFeature] >= splitValues[i] && v[bestFeature] < splitValues[i + 1],
                        VectorType.Row);

                    n.Children[i] = BuildTree(x.Slice(slice, VectorType.Row), y.Slice(slice), depth - 1, used);
                }
                n.Segmented = true;
            }

            n.IsLeaf = false;
            n.Feature = bestFeature;
            n.Values = splitValues;
            return n;
        }
    }
}
