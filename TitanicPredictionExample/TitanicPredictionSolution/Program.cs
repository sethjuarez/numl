using System.Configuration;
using numl;
using numl.Model;
using numl.Supervised;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TitanicPredictionSolution
{
    class Program
    {
        public static void Main(string[] args)
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
