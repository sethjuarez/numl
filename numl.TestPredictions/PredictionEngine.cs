using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using numl;
using numl.Model;
using numl.Supervised;

namespace numl.TitanicPassengersChallenge
{
    public class PredictionEngine
    {
        public void ProcessPredictionOutput(PredictionResult predictionResult)
        {
            int incorrectPredictions = 0;
            int correctPredictions = 0;

            for (int passengerIndex = 0; passengerIndex < 800; passengerIndex++)
            {
                var passenger = predictionResult.Passengers[passengerIndex];

                var survived = passenger.PassengerSurvived;

                //var passengerPredicted =
                //    predictionResult.PredictionModel.Model.Predict<Passenger>(passenger);

                var passengerPredicted =
                    predictionResult.PredictionModel.Predict<Passenger>(passenger);

                if (survived != passengerPredicted.PassengerSurvived)
                    incorrectPredictions++;
                else
                    correctPredictions++;

                //Console.WriteLine(
                //    "The actual = {0}, prediction = {1}",
                //    survived.ToString(), passengerPredicted.PassengerSurvived);
            }

            var percentageCorrect = (correctPredictions / 800.0) * 100;
            var percentageIncorrect = (incorrectPredictions / 800.0) * 100;

            Console.WriteLine("Out of 800 predictions, the engine predicted {0}% correctly and {1}% incorrectly", percentageCorrect, percentageIncorrect);

            Console.ReadKey();
        }

        public PredictionResult GeneratePredictionModel(string dataPath)
        {
            if (string.IsNullOrEmpty(dataPath))
                throw new ArgumentNullException("dataPath");

            var generator = CreateDecisionTreeGenerator();

            if (generator == null)
                throw new NullReferenceException("The decision tree generator cannot be null.");

            var data = Passenger.LoadData(dataPath, true);

            var model = generator.Generate(data);
            //var model = Learner.Learn(data, .90, 1000, generator);

            var predictionResult = new PredictionResult {Passengers = data, PredictionModel = model};

            return predictionResult;
        }

        public DecisionTreeGenerator CreateDecisionTreeGenerator()
        {
            var generator = new DecisionTreeGenerator(50)
            {
                Descriptor = Descriptor.Create<Passenger>(),
                Hint = 0
            };
            return generator;
        }
    }
}
