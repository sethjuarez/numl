using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using numl.Visualizers;
using numl.Math;

namespace numl.Tests
{
    [TestFixture]
    public class VisualizerTests
    {
        [Test]
        public void Test_Matrix_Debug_Visualizer()
        {
            int n = 10;
            var x = Matrix.VStack(
                        Vector.NormRand(n, 8, 3),
                        Vector.NormRand(n, 1, 6),
                        Vector.NormRand(n, -1, 7.5),
                        Vector.NormRand(n, .5, 2));

            MatrixVisualizer.TestShowVisualizer(x);
        }

        [Test]
        public void Test_Vector_Debug_Visualizer()
        {
            int n = 10;
            var x = Vector.NormRand(n, 8, 3);

            VectorVisualizer.TestShowVisualizer(x);
        }
    }
}
