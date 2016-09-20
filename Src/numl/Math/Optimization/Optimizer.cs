using System;
using System.Linq;
using System.Threading.Tasks;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using numl.Math.Optimization.Methods;
using numl.Math.Functions.Cost;
namespace numl.Math.Optimization
{
    /// <summary>
    /// Optimizer.
    /// </summary>
    public class Optimizer
    {
        /// <summary>
        /// Gets a value indicating whether the optimizer has finished. 
        /// </summary>
        public bool Completed { get; private set; }

        /// <summary>
        /// Gets or sets the optimization method to use.
        /// </summary>
        public IOptimizationMethod OpimizationMethod { get; set; }

        /// <summary>
        /// Gets or sets the cost function to optimize.
        /// </summary>
        public ICostFunction CostFunction { get; set; }

        /// <summary>
        /// Gets or sets the optimization properties used.
        /// </summary>
        public OptimizerProperties Properties { get; set; }

        /// <summary>
        /// Initializes a new Optimizer using the default values.
        /// <param name="theta">Theta to optimize.</param>
        /// <param name="maxIterations">Maximum number of iterations.</param>
        /// <param name="learningRate">Learning Rate (alpha) (Optional).</param>
        /// <param name="momentum">Momentum parameter for use in accelerated methods (Optional).</param>
        /// <param name="optimizationMethod">Type of optimization method to use (Optional).</param>
        /// <param name="optimizer">An external typed optimization method to use (Optional).</param>
        /// </summary>
        public Optimizer(Vector theta, int maxIterations, double learningRate = 1.0, double momentum = 0.9,
            OptimizationMethods optimizationMethod = OptimizationMethods.StochasticGradientDescent, OptimizationMethod optimizer = null)
        {
            this.Completed = false;
            if (optimizationMethod != OptimizationMethods.External)
            {
                switch (optimizationMethod)
                {
                    case OptimizationMethods.FastGradientDescent: optimizer = new numl.Math.Optimization.Methods.GradientDescent.FastGradientDescent() { Momentum = momentum }; break;
                    case OptimizationMethods.StochasticGradientDescent: optimizer = new numl.Math.Optimization.Methods.GradientDescent.StochasticGradientDescent(); break;
                    case OptimizationMethods.NAGDescent: optimizer = new numl.Math.Optimization.Methods.GradientDescent.NAGDescent() { Momentum = momentum }; break;
                }
            }

            this.OpimizationMethod = optimizer;

            this.Properties = new OptimizerProperties()
            {
                Iteration = 0,
                MaxIterations = maxIterations,
                Cost = double.MaxValue,
                Gradient = Vector.Zeros(theta.Length),
                Theta = theta,
                LearningRate = learningRate,
                Momentum = momentum
            };
        }

        /// <summary>
        /// Runs the optimization routine for the set number of iterations.
        /// </summary>
        public void Run()
        {
            this.CostFunction.Initialize();

            for (int x = 0; x < this.Properties.MaxIterations; x++)
            {
                if (this.OpimizationMethod.Update(this.Properties))
                {
                    this.Step();
                }
                else
                {
                    break;
                }
            }

            this.Completed = true;
        }

        /// <summary>
        /// Runs the optimization routine for the set number of iterations.
        /// </summary>
        public Task RunAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                this.Run();
            });
        }

        /// <summary>
        /// Performs a single step of the optimization routine.
        /// </summary>
        public void Step()
        {
            this.Properties.Iteration += 1;

            double lastCost = this.Properties.Cost;

            this.Properties.Cost = this.OpimizationMethod.UpdateCost(this.CostFunction, this.Properties);
            this.Properties.CostHistory.Add(this.Properties.Cost);

            this.Properties.Gradient = this.OpimizationMethod.UpdateGradient(this.CostFunction, this.Properties);
            this.Properties.GradientHistory.Add(this.Properties.Gradient);

            this.Properties.Theta = this.OpimizationMethod.UpdateTheta(this.Properties);

            if (this.Properties.Iteration > 1 && this.Properties.Cost < lastCost) this.Properties.BestTheta = this.Properties.Theta;
            else this.Properties.BestTheta = this.Properties.Theta;
        }
    }
}
