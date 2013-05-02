using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.TitanicPassengersChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begining prediction of Titanic data set.");

            var predictionEngine = new PredictionEngine();
            var generator = predictionEngine.CreateDecisionTreeGenerator();
            var trainingDataPath = ConfigurationManager.AppSettings["trainingDataPath"];
            var predictionResult = predictionEngine.GeneratePredictionModel(trainingDataPath);

            predictionEngine.ProcessPredictionOutput(predictionResult);

            

            _displaySuccess();
        }

        private static void _displaySuccess()
        {
            Console.WriteLine("The prediction is complete.");
            Console.ReadKey();
        }

    }
}
