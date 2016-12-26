using System;
using System.Collections.Generic;
using numl;
using numl.Model;
using numl.Supervised;
using numl.Supervised.DecisionTree;
using numlsample.GettingStarted.Data;

namespace numlsample.GettingStarted.ExampleCode
{
    /// <summary>
    /// <para>
    ///   This describes a very basic workflow of using numl with various
    ///   ML algorithms (generators, models). It is essentially as follows:
    /// </para>
    /// <para>
    ///   First, you start with your data and <c>Descriptor</c>. This is how you are
    ///   declaring the features that you are analyzing and the outcome that you
    ///   want to predict.
    /// </para>
    /// <para>
    ///   Next, you choose an <c>IGenerator</c>, which is basically what ML algorithm you
    ///   want to use.
    /// </para>
    /// <para>
    ///   The <c>Learner</c> will then train a <c>IModel</c>
    /// </para>
    /// </summary>
    /// <seealso cref="SimpleNumlWorkflow.SimpleNumlWorkflowImpl{TData}(Func{IEnumerable{TData}}, Func{Descriptor}, Func{Descriptor, IGenerator}, Func{TData}, Func{TData, string}, double, int)"/>
    public static class SimpleNumlWorkflow
    {
        public static void Go()
        {
            // These all use the same simple workflow,
            // with only minor tweaks of swapped generators.

            // First try giving them a run, then tweak your default arguments.

            DoSimple_DecisionTree(simpleData: false);
            DoSimple_LinearRegression(simpleData: false);
            DoSimple_NaiveBayes(simpleData: false);
            DoSimple_LogisticRegression(simpleData: false);
            DoSimple_Perceptron(simpleData: false);
            DoSimple_NeuralNetwork(simpleData: false);
        }

        /// <summary>
        /// Implementation method that executes a simple numl workflow,
        /// easily swapping generators/models, etc. This is primarily to
        /// keep things DRY, even though this is just example code.
        /// </summary>
        /// <typeparam name="TData">The type of your data</typeparam>
        /// <param name="getData">func to retrieve data, e.g. <c>() => SampleData.GetTennisData()</c></param>
        /// <param name="getDescriptor">func to describe your type of data, e.g. <c>() => Descriptor.Create<Tennis>()</c></param>
        /// <param name="getGenerator">func to create/initialize your generator. See example methods.</param>
        /// <param name="getToPredict">func to get/create the data you want to predict. See example methods.</param>
        /// <param name="getPredictionDesc">logging func for displaying prediction output on console.</param>
        static void SimpleNumlWorkflowImpl<TData>(
            Func<IEnumerable<TData>> getData,
            Func<Descriptor> getDescriptor,
            Func<Descriptor, IGenerator> getGenerator,
            Func<TData> getToPredict,
            Func<TData, string> getPredictionDesc,
            double trainingPercentage,
            int repeat
            )
            where TData : class
        {
            // Start with descriptor
            var descriptor = getDescriptor();

            // Choose our generator
            var generator = getGenerator(descriptor);
            Console.WriteLine($"Starting {generator.GetType().Name}\n");

            // Load our data
            var data = getData();

            // Create the model by learning from the data using the generator
            var learningModel = Learner.Learn(data, trainingPercentage, repeat, generator);
            Console.WriteLine(learningModel);

            // Now we could predict using the learning info's Model.
            var toPredict = getToPredict();

            var prediction = learningModel.Model.Predict(toPredict);
            //var tennisPrediciton = learningModel.Model.Predict(areWeGonnaPlayTennis);

            Console.WriteLine($"To Predict: {toPredict})");
            Console.WriteLine($"Prediction: {getPredictionDesc(prediction)}\n");

            Console.WriteLine("Press any key...\n");
            Console.ReadKey();
        }

        /// <summary>
        /// Do the <c>SimpleNumlWorkflow</c> using a <c>DecisionTree</c> generator/model.
        /// </summary>
        /// <param name="simpleData">If true, uses a preset small sample data set; else generates a larger, pseudo-random one.</param>
        /// <param name="depth"></param>
        /// <param name="width"></param>
        static void DoSimple_DecisionTree(bool simpleData = true, int depth = 5,
            int width = 2)
        {
            SimpleNumlWorkflowImpl(
                getData: () => SampleData.GetTennisData(simpleData),

                getDescriptor: () => Descriptor.Create<Tennis>(),

                getGenerator: (descriptor) =>
                {
                    var generator = new DecisionTreeGenerator(descriptor)
                    {
                        Depth = depth,
                        Width = width
                    };
                    generator.SetHint(false);
                    return generator;
                },

                getToPredict: () =>
                {
                    return new Tennis()
                    {
                        Outlook = Outlook.Rainy,
                        Temperature = Temperature.Low,
                        Windy = true
                    };
                },

                getPredictionDesc: (t) => "Play: " + t.Play,

                trainingPercentage: 0.8d,

                repeat: 1000
                );
        }

        /// <summary>
        /// Do the <c>SimpleNumlWorkflow</c> using a <c>LinearRegression</c> generator/model.
        /// </summary>
        /// <param name="simpleData">If true, uses a preset small sample data set; else generates a larger, pseudo-random one.</param>
        /// <param name="maxIterations"></param>
        /// <param name="learningRate"></param>
        /// <param name="lambda"></param>
        static void DoSimple_LinearRegression(bool simpleData = true, int maxIterations = 500,
            double learningRate = 0.01d, double lambda = 1)
        {
            SimpleNumlWorkflowImpl(
                getData: () => SampleData.GetTennisData(simpleData),

                getDescriptor: () => Descriptor.Create<Tennis>(),

                // This is where we choose the actual algorithm
                getGenerator: (descriptor) =>
                {
                    var generator = new numl.Supervised.Regression.LinearRegressionGenerator()
                    {
                        Descriptor = descriptor,
                        MaxIterations = maxIterations,
                        LearningRate = learningRate,
                        Lambda = lambda
                    };
                    return generator;
                },

                getToPredict: () =>
                {
                    return new Tennis()
                    {
                        Outlook = Outlook.Rainy,
                        Temperature = Temperature.Low,
                        Windy = true
                    };
                },

                getPredictionDesc: (t) => "Play: " + t.Play.ToString(),

                trainingPercentage: 0.8d,

                repeat: 10
                );
        }

        static void DoSimple_NaiveBayes(bool simpleData = true, int width = 2)
        {
            SimpleNumlWorkflowImpl(
                getData: () => SampleData.GetTennisData(simpleData),

                getDescriptor: () => Descriptor.Create<Tennis>(),

                // This is where we choose the actual algorithm
                getGenerator: (descriptor) =>
                {
                    var generator = new numl.Supervised.NaiveBayes.NaiveBayesGenerator(width)
                    {
                        Descriptor = descriptor,
                    };

                    return generator;
                },

                getToPredict: () =>
                {
                    return new Tennis()
                    {
                        Outlook = Outlook.Rainy,
                        Temperature = Temperature.Low,
                        Windy = true
                    };
                },

                getPredictionDesc: (t) => "Play: " + t.Play.ToString(),

                trainingPercentage: 0.8d,

                repeat: 25
                );
        }

        /// <summary>
        /// Do the <c>SimpleNumlWorkflow</c> using a <c>LogisticRegression</c> generator/model.
        /// </summary>
        /// <param name="simpleData">If true, uses a preset small sample data set; else generates a larger, pseudo-random one.</param>
        /// <param name="polynomialFeatures"></param>
        /// <param name="maxIterations"></param>
        /// <param name="learningRate"></param>
        /// <param name="lambda"></param>
        static void DoSimple_LogisticRegression(bool simpleData = true, int polynomialFeatures = 5,
            int maxIterations = 500, double learningRate = 0.03d, double lambda = 1)
        {
            SimpleNumlWorkflowImpl(
                getData: () => SampleData.GetTennisData(simpleData),

                getDescriptor: () => Descriptor.Create<Tennis>(),

                // This is where we choose the actual algorithm
                getGenerator: (descriptor) =>
                {
                    var generator = new numl.Supervised.Regression.LogisticRegressionGenerator()
                    {
                        Descriptor = descriptor,
                        PolynomialFeatures = polynomialFeatures,
                        MaxIterations = maxIterations,
                        LearningRate = learningRate,
                        Lambda = lambda
                    };

                    return generator;
                },

                getToPredict: () =>
                {
                    return new Tennis()
                    {
                        Outlook = Outlook.Rainy,
                        Temperature = Temperature.Low,
                        Windy = true
                    };
                },

                getPredictionDesc: (t) => "Play: " + t.Play.ToString(),

                trainingPercentage: 0.8d,

                repeat: 25
                );
        }

        /// <summary>
        /// Do the <c>SimpleNumlWorkflow</c> using a <c>Perceptron</c> generator/model.
        /// </summary>
        /// <param name="simpleData">If true, uses a preset small sample data set; else generates a larger, pseudo-random one.</param>
        /// <param name="normalize"></param>
        static void DoSimple_Perceptron(bool simpleData = true, bool normalize = true)
        {
            SimpleNumlWorkflowImpl(
                getData: () => SampleData.GetTennisData(simpleData),

                getDescriptor: () => Descriptor.Create<Tennis>(),

                // This is where we choose the actual algorithm
                getGenerator: (descriptor) =>
                {
                    var generator = new numl.Supervised.Perceptron.PerceptronGenerator(normalize)
                    {
                        Descriptor = descriptor
                    };
                    return generator;
                },

                getToPredict: () =>
                {
                    return new Tennis()
                    {
                        Outlook = Outlook.Rainy,
                        Temperature = Temperature.Low,
                        Windy = true
                    };
                },

                getPredictionDesc: (t) => "Play: " + t.Play.ToString(),

                trainingPercentage: 0.8d,

                repeat: 1000
                );
        }

        /// <summary>
        /// Do the <c>SimpleNumlWorkflow</c> using a <c>Neural Network</c> generator/model.
        /// </summary>
        /// <param name="simpleData">If true, uses a preset small sample data set; else generates a larger, pseudo-random one.</param>
        /// <param name="maxIterations"></param>
        /// <param name="learningRate"></param>
        static void DoSimple_NeuralNetwork(bool simpleData = true, int maxIterations = 10000,
            double learningRate = 0.9d)
        {
            SimpleNumlWorkflowImpl(
                getData: () => SampleData.GetTennisData(simpleData),

                getDescriptor: () => Descriptor.Create<Tennis>(),

                // This is where we choose the actual algorithm
                getGenerator: (descriptor) =>
                {
                    var generator = new numl.Supervised.NeuralNetwork.NeuralNetworkGenerator()
                    {
                        Descriptor = descriptor,
                        MaxIterations = maxIterations,
                        LearningRate = learningRate
                    };
                    return generator;
                },

                getToPredict: () =>
                {
                    return new Tennis()
                    {
                        Outlook = Outlook.Rainy,
                        Temperature = Temperature.Low,
                        Windy = true
                    };
                },

                getPredictionDesc: (t) => "Play: " + t.Play.ToString(),

                trainingPercentage: 0.8d,

                repeat: 50
                );
        }
    }
}
