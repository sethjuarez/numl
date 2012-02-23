using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using numl.Math;

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
            // TODO: Add the following to your testing code to test the visualizer:
            // 
            //    MatrixVisualizer.TestShowVisualizer(new SomeType());
            // 

            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(VectorVisualizer));
            visualizerHost.ShowVisualizer();
        }
    }
}
