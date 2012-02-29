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
using numl.Model;

namespace numl.Supervised
{
    public class DecisionTreeModel : IModel
    {
        public LabeledDescription Description { get; set; }
        public Node Tree { get; set; }
        public double Hint { get; set; }

        public DecisionTreeModel()
        {
            // no hint
            Hint = double.Epsilon;
        }

        public double Predict(Vector y)
        {
            return WalkNode(y, Tree);
        }

        private double WalkNode(Vector v, Node node)
        {
            if (node.IsLeaf)
                return node.Label;

            // Get the index of the feature for this node.
            var feature = node.Feature;
            if (feature == -1)
                throw new InvalidOperationException("Invalid Feature encountered during node walk!");

            // segmented split
            // with width set
            if (node.Segmented)
            {
                for (int i = 1; i < node.Values.Length; i++)
                    if (v[feature] >= node.Values[i - 1] && v[feature] < node.Values[i])
                        return WalkNode(v, node.Children[i - 1]);

                if (Hint != double.Epsilon)
                    return Hint;
                else
                    throw new InvalidOperationException(String.Format("Unable to match split value {0} for feature {1}", v[feature], Description.Features[feature].Name));
            }
            // non-continous split
            else
            {
                for (int i = 0; i < node.Values.Length; i++)
                    if (node.Values[i] == v[feature])
                        return WalkNode(v, node.Children[i]);

                if (Hint != double.Epsilon)
                    return Hint;
                else
                    throw new InvalidOperationException(String.Format("Unable to match split value {0} for feature {1}", v[feature], Description.Features[feature].Name));
            }
        }
    }
}
