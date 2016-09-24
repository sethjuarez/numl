using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using numl.Math.LinearAlgebra;
using numl.Utils;

namespace numl.Math.Discretization
{
    /// <summary>
    /// Feature binning discretizer.
    /// </summary>
    public class BinningDiscretizer : Discretizer
    {
        /// <summary>
        /// Gets or sets a Vector of the number of bins for each feature property.
        /// <para>Limiting a state value will discretize a single feature to be between zero and the specified value.</para>
        /// </summary>
        public Vector States { get; set; }

        /// <summary>
        /// Gets or sets the ranges for each feature bin.
        /// </summary>
        public Range[][] Ranges { get; set; }

        /// <summary>
        /// Initializes a new BinningDiscretizer with a specified number of bins for each feature.
        /// </summary>
        /// <param name="states">Vector of bin counts for each feature property.</param>
        public BinningDiscretizer(Vector states)
        {
            this.States = states;
        }

        /// <summary>
        /// Initializes the discretizer.
        /// </summary>
        /// <param name="rows">Matrix.</param>
        /// <param name="summary">Summary.</param>
        public override void Initialize(Matrix rows, Summary summary)
        {
            base.Initialize(rows, summary);

            this.Ranges = new Range[rows.Cols][];

            for (int idx = 0; idx < rows.Cols; idx++)
            {
                if (States[idx] >= 2.0)
                    this.Ranges[idx] = rows[idx, VectorType.Col].Segment((int) States[idx]);
                else
                    this.Ranges[idx] = new Range[] { Range.Make(summary.Minimum[idx], summary.Maximum[idx]) };
            }
        }

        /// <summary>
        /// Returns a discretized value given the source vector.
        /// </summary>
        /// <param name="row">Row vector to discretize.</param>
        /// <param name="summary">Summary.</param>
        /// <returns>Double.</returns>
        public override double Discretize(Vector row, Summary summary)
        {
            string s = "0";

            for (int c = 0; c < row.Length; c++)
            {
                s += this.Ranges[c].IndexOf(f => f.Test(row[c])) + 1;
            }

            return double.Parse(s);
        }
    }
}