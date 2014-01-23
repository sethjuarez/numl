using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using Microsoft.VisualStudio.DebuggerVisualizers;

[
    assembly: System.Diagnostics.DebuggerVisualizer(
    typeof(numl.Visualizers.VectorVisualizer),
    typeof(VisualizerObjectSource),
    Target = typeof(Vector),
    Description = "Vector Visualizer")
]
namespace numl.Visualizers
{
    /// <summary>
    /// A Visualizer for Matrix
    /// </summary>
    public class VectorVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            if (windowService == null)
                throw new ArgumentNullException("windowService");
            if (objectProvider == null)
                throw new ArgumentNullException("objectProvider");
            if (!(objectProvider.GetObject() is Vector))
                throw new ArgumentException("Object type is not Matrix");

            var data = (Vector)objectProvider.GetObject();

            using (DataVisualizerForm f = new DataVisualizerForm())
            {
                f.Text = "Vector";
                f.SetData(data);
                windowService.ShowDialog(f);
            }
        }


        /// <summary>
        /// Tests the visualizer by hosting it outside of the debugger.
        /// </summary>
        /// <param name="objectToVisualize">The object to display in the visualizer.</param>
        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(VectorVisualizer));
            visualizerHost.ShowVisualizer();
        }
    }
}
