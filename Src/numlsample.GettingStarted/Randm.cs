using System;
using System.Collections.Generic;
using System.Linq;

namespace numlsample.GettingStarted
{
    public class Randm
    {
        public Randm()
        {
            NewRandomizer();
        }

        private Random Randomizer { get; set; }

        #region Singleton Pattern Members
        private static volatile Randm _Helper;
        private static object _Lock = new object();
        public static Randm Helper
        {
            get
            {
                if (_Helper == null)
                {
                    lock (_Lock)
                    {
                        if (_Helper == null)
                            _Helper = new Randm();
                    }
                }

                return _Helper;
            }
        }
        #endregion

        public void NewRandomizer()
        {
            //var generator = new RNGCryptoServiceProvider();
            ////using (RNGCryptoServiceProvider generator = new RNGCryptoServiceProvider())
            //{
            //    byte[] randomBytes = new byte[4];
            //    generator.GetBytes(randomBytes);
            //    int randomInt = BitConverter.ToInt32(randomBytes, 0);
            //    Randomizer = new Random(randomInt);
            //}

            Randomizer = new Random(DateTime.Now.Millisecond + DateTime.Now.Minute + DateTime.Now.Second * (DateTime.Now.Second + 1));
        }

        public T PickOne<T>(T aObject, T bObject, double aWeight = 0.5d, double bWeight = 0.5d)
        {
            T dummy = default(T);
            return PickOne<T>(aObject, bObject, out dummy, aWeight, bWeight);
        }

        public T PickOne<T>(T aObject, T bObject, out T loser, double aWeight = 0.5d, double bWeight = 0.5d)
        {
            if (aObject == null)
                throw new ArgumentNullException("aObject");
            if (bObject == null)
                throw new ArgumentNullException("bObject");
            if (aWeight < 0)
                throw new ArgumentException("aWeight");
            if (bWeight < 0)
                throw new ArgumentException("bWeight");

            double probabilityA = aWeight / (aWeight + bWeight);
            var randomDouble = Randomizer.NextDouble();
            if (randomDouble < probabilityA)
            {
                loser = bObject;
                return aObject;
            }
            else
            {
                loser = aObject;
                return bObject;
            }
        }
        public object PickOne(object aObject, object bObject, out object loser, double aWeight = 0.5d,
          double bWeight = 0.5d)
        {
            return PickOne<object>(aObject, bObject, out loser, aWeight, bWeight);
        }

        public T PickOne<T>(params WeightedOption<T>[] weightedOptions)
        {
            if (weightedOptions == null)
                throw new ArgumentNullException(nameof(weightedOptions));

            var result = default(T);

            var nonZeroWeightedOptions = weightedOptions.Where(wo => wo.Weight > 0);

            //We're going to add up all the weights. Each of the options' weight will set the boundary
            //of the number line that means that option is picked. 
            var weightSum = nonZeroWeightedOptions.Sum(wo => wo.Weight);
            var pickedRandomNumber = Randomizer.NextDouble() * weightSum;
            var optionsList = nonZeroWeightedOptions.ToList();
            var optionFound = false;

            for (int i = 0; i < optionsList.Count; i++)
            {
                var option = optionsList[i];
                var sumPrevOptionWeights =
                    i == 0 ?
                    0 :
                    optionsList.Take(i).Sum(wo => wo.Weight);
                var min = sumPrevOptionWeights;
                var max = sumPrevOptionWeights + option.Weight;
                //is the picked random number in our range of min/max? If so, then our option was chosen.
                if (pickedRandomNumber >= min &&
                    pickedRandomNumber <= max)
                {
                    optionFound = true;
                    result = option.Obj;
                    break;
                }
            }

            if (!optionFound)
                throw new InvalidOperationException("Option was not found in weighted random picking.");

            return result;
        }

        public object PickOne(params WeightedOption[] weightedOptions)
        {
            var castedOptions = weightedOptions.Select(wo => WeightedOption<object>._(wo.Weight, wo.Obj));
            var result = PickOne<object>(castedOptions.ToArray());
            return result;
        }

        public int NextInt(int minInclusive, int maxExclusive)
        {
            if (minInclusive >= maxExclusive)
                throw new InvalidOperationException($"{nameof(minInclusive)} must be less than {nameof(maxExclusive)}");

            return Randomizer.Next(minInclusive, maxExclusive);
        }

        public double NextDouble(double minInclusive, double maxExclusive)
        {
            if (minInclusive >= maxExclusive)
                throw new InvalidOperationException($"{nameof(minInclusive)} must be less than {nameof(maxExclusive)}");

            //thanks! http://www.experts-exchange.com/Programming/Languages/C_Sharp/Q_28243467.html 
            var d = Randomizer.NextDouble();

            var result = ((d * (maxExclusive - minInclusive)) + minInclusive);

            return result;
        }

        public T PickOne<T>(IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                throw new ArgumentNullException("enumerable");

            var count = enumerable.Count();
            if (count == 0)
                return default(T);

            var i = NextInt(0, count);
            return enumerable.ElementAt(i);
        }

        /// <summary>
        /// Picks random letters given a number of letters to pick. (uppercase and lowercase)
        /// </summary>
        /// <param name="numOfLetters"></param>
        /// <returns></returns>
        public string PickLetters(int numOfLetters)
        {
            if (numOfLetters <= 0)
                throw new ArgumentException($"{nameof(numOfLetters)} must be greater than 0.");

            var letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string randomString = "";

            for (int i = 0; i < numOfLetters; i++)
                randomString += PickRandomCharacter(letters);


            return randomString;
        }

        public string PickRandomInt_RingAround()
        {
            if (PickBool(0.5))
            {
                var result = @"2035";
                return result;
            }
            else
            {
                var result = @"2035";
                return result;
            }
        }

        public char PickRandomCharacter(string source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var randomIndex = Randomizer.Next(0, source.Length);
            var randomChar = source[randomIndex];
            return randomChar;
        }

        public bool PickBool(double probabilityTrue)
        {
            var next = numl.Math.Probability.Sampling.GetUniform();
            //var next = Randomizer.NextDouble();

            var result = (next <= probabilityTrue);

            return result;
        }
    }

    public class WeightedOption<T>
    {
        public static WeightedOption<T> _(double weight, T obj)
        {
            var result = new WeightedOption<T>()
            {
                Weight = weight,
                Obj = obj
            };
            return result;
        }
        public double Weight { get; set; }
        public T Obj { get; set; }
    }

    public class WeightedOption : WeightedOption<object>
    {
        public new static WeightedOption _(double weight, object obj)
        {
            var result = new WeightedOption()
            {
                Weight = weight,
                Obj = obj
            };
            return result;
        }
    }
}
