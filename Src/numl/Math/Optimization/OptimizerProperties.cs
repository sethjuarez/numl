using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Math.Optimization
{
    /// <summary>
    /// Optimisation Properties for use in Optimization.
    /// </summary>
    public class OptimizerProperties
    {
        private const int MinimizationConstantValue = 1 * 10 ^ -4;

        /// <summary>
        /// Gets or sets the early stopping minimization constant value (default is 1*10^(-4)).
        /// <para>This is the relative cost difference at each timestep to have atleast decreased by from the last timestep.</para>
        /// </summary>
        public double MinimizationConstant { get; set; }

        /// <summary>
        /// Gets or sets the current iteration counter.
        /// </summary>
        public int Iteration { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of iterations.
        /// </summary>
        public int MaxIterations { get; set; }
        /// <summary>
        /// Gets or Sets the current Gradient.
        /// </summary>
        public Vector Gradient { get; set; }

        /// <summary>
        /// Gets the Gradient history for each timestep.
        /// </summary>
        public List<Vector> GradientHistory { get; private set; }

        /// <summary>
        /// Gets or sets the current Cost.
        /// </summary>
        public double Cost { get; set; }

        /// <summary>
        /// Gets the Cost history for each timestep.
        /// </summary>
        public List<double> CostHistory { get; private set; }

        /// <summary>
        /// Gets or Sets the learning rate (alpha).
        /// </summary>
        public double LearningRate { get; set; }

        /// <summary>
        /// Gets or sets the Momentum used.
        /// </summary>
        public double Momentum { get; set; }

        /// <summary>
        /// Gets or sets the Theta value that is being optimized.
        /// </summary>
        public Vector Theta { get; set; }

        /// <summary>
        /// Gets or sets the Theta value that is being optimized.
        /// </summary>
        public Vector BestTheta { get; set; }

        /// <summary>
        /// Initializes a new OptimizerProperties object with a default max iteration value of 400.
        /// </summary>
        public OptimizerProperties()
        {
            this.MinimizationConstant = MinimizationConstantValue;
            this.MaxIterations = 400;
            this.GradientHistory = new List<Vector>(this.MaxIterations);
            this.CostHistory = new List<double>(this.MaxIterations);
        }

        /// <summary>
        /// Initializes a new OptimizerProperties object.
        /// </summary>
        /// <param name="maxIterations">Maximum number of iterations.</param>
        public OptimizerProperties(int maxIterations)
        {
            this.MinimizationConstant = MinimizationConstantValue;
            this.MaxIterations = maxIterations;
            this.GradientHistory = new List<Vector>(this.MaxIterations);
            this.CostHistory = new List<double>(this.MaxIterations);
        }
    }
}
