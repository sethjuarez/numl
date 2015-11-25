using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using numl.Supervised;


namespace numl
{
    /// <summary>Structure to hold generator, model, and accuracy information.</summary>
    public class LearningModel
    {
        /// <summary>Generator used to create model.</summary>
        /// <value>The generator.</value>
        public IGenerator Generator { get; set; }

        /// <summary>Model created by generator.</summary>
        /// <value>The model.</value>
        public IModel Model { get; set; }

        /// <summary>
        /// Gets the Score of the model.
        /// </summary>
        public Score Score { get; set; }

        /// <summary>Gets the overall Accuracy of the model.</summary>
        /// <value>The accuracy.</value>
        public double Accuracy
        {
            get
            {
                return this.Score.Accuracy;
            }
        }

        /// <summary>Textual representation of structure.</summary>
        /// <returns>string.</returns>
        public override string ToString()
        {
            return string.Format("Learning Model:\n  Generator {0}\n  Model:\n{1}\n  Accuracy: {2:p}\n  Precision: {3:p}\n  Recall: {4:p}\n  F-Score: {5:p}",
                Generator,
                Model,
                Accuracy,
                Score.Precision,
                Score.Recall,
                Score.FScore);
        }
    }
}
