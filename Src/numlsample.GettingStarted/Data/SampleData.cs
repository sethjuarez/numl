using System.Collections.Generic;

namespace numlsample.GettingStarted.Data
{
    public static class SampleData
    {
        public static Tennis[] GetTennisData(bool predetermined = true)
        {
            if (predetermined)
            {
                return new Tennis[]  {
                    new Tennis { Play = true, Outlook=Outlook.Sunny, Temperature = Temperature.Low, Windy=true},
                    new Tennis { Play = false, Outlook=Outlook.Sunny, Temperature = Temperature.High, Windy=true},
                    new Tennis { Play = false, Outlook=Outlook.Sunny, Temperature = Temperature.High, Windy=false},
                    new Tennis { Play = true, Outlook=Outlook.Overcast, Temperature = Temperature.Low, Windy=true},
                    new Tennis { Play = true, Outlook=Outlook.Overcast, Temperature = Temperature.High, Windy= false},
                    new Tennis { Play = true, Outlook=Outlook.Overcast, Temperature = Temperature.Low, Windy=false},
                    new Tennis { Play = false, Outlook=Outlook.Rainy, Temperature = Temperature.Low, Windy=true},
                    new Tennis { Play = true, Outlook=Outlook.Rainy, Temperature = Temperature.Low, Windy=false}
                };
            }
            else
            {
                var result = new List<Tennis>();
                var outlookOptions = new List<Outlook>()
                {
                    Outlook.Sunny,
                    Outlook.Overcast,
                    Outlook.Rainy
                };

                for (int i = 0; i < 1000; i++)
                {
                    // Completely random (should generate around 50% accuracy, closer to 50% with more iterations)
                    //Tennis tennis = GetTennis_CompletelyRandom(outlookOptions);

                    // Calm wind means play (generates 100.0% accuracy)
                    //Tennis tennis = GetTennis_CalmWindAlwaysPlay(outlookOptions);

                    // Some kind of meaning behind whether to play or not,
                    // so this should be a higher accuracy than completely random,
                    // but not close to 100%
                    Tennis tennis = GetTennis_UsuallyPlayFairConditions(outlookOptions);

                    result.Add(tennis);
                }

                return result.ToArray();
            }
        }

        private static Tennis GetTennis_CompletelyRandom(List<Outlook> outlookOptions)
        {
            var tennis = new Tennis()
            {
                Play = Randm.Helper.PickBool(0.5),
                Outlook = Randm.Helper.PickOne(outlookOptions),
                Temperature = Randm.Helper.PickBool(0.5) ? Temperature.Low : Temperature.High,
                Windy = Randm.Helper.PickBool(0.5),
            };

            return tennis;
        }

        private static Tennis GetTennis_CalmWindAlwaysPlay(List<Outlook> outlookOptions)
        {
            var tennis = new Tennis()
            {
                Outlook = Randm.Helper.PickOne(outlookOptions),
                Temperature = Randm.Helper.PickBool(0.5) ? Temperature.Low : Temperature.High,
                Windy = Randm.Helper.PickBool(0.5),
            };
            tennis.Play = !tennis.Windy;

            return tennis;
        }

        private static Tennis GetTennis_UsuallyPlayFairConditions(List<Outlook> outlookOptions)
        {
            var tennis = new Tennis()
            {
                Outlook = Randm.Helper.PickOne(outlookOptions),
                Temperature = Randm.Helper.PickBool(0.7) ? Temperature.Low : Temperature.High,
                Windy = Randm.Helper.PickBool(0.7),
            };

            if (tennis.Windy && tennis.Outlook == Outlook.Rainy)
                tennis.Play = Randm.Helper.PickBool(0.1);
            else if (tennis.Outlook == Outlook.Overcast)
                tennis.Play = Randm.Helper.PickBool(0.8);
            else
                tennis.Play = Randm.Helper.PickBool(0.9);

            return tennis;
        }
    }
}
