using System;
using numl;
using numl.Model;
using numl.Supervised.DecisionTree;
using numlsample.GettingStarted.Data;

namespace numlsample.GettingStarted.ExampleCode
{
    /// <summary>
    /// This is the very simple, git-er-dun quickstart.
    /// </summary>
    public static class QuickStart
    {
        public static void Go()
        {
            // Start with our data
            Tennis[] data = SampleData.GetTennisData();

            // Create the corresponding descriptor
            var descriptor = Descriptor.Create<Tennis>();

            // Choose our generator
            var generator = new DecisionTreeGenerator(descriptor);
            generator.SetHint(false);

            Console.WriteLine($"Using the {generator.GetType().Name}\n");

            // Create the model by learning from the data using the generator
            LearningModel learningModel = Learner.Learn(data, 0.80, 1000, generator);

            Console.WriteLine(learningModel);

            // Now we could predict using the learning info's Model.
            var toPredict = new Tennis()
            {
                Outlook = Outlook.Rainy,
                Temperature = Temperature.Low,
                Windy = true,
                // Play = ? - This is what we will predict
            };

            var prediction = learningModel.Model.Predict(toPredict);

            // And we're spent...
            Console.WriteLine($"ToPredict: {toPredict}");
            Console.WriteLine($"Prediction: Play = {prediction.Play}\n");

            Console.WriteLine("Press any key...\n");
            Console.ReadKey();
        }


    }
}
