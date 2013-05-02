using numl.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TitanicPredictionSolution
{

    #region "Feature details"

    //Age is in Years; Fractional if Age less than One (1)
    // If the Age is Estimated, it is in the form xx.5

    //With respect to the family relation variables (i.e. sibsp and parch)
    //some relations were ignored.  The following are the definitions used
    //for sibsp and parch.

    //Sibling:  Brother, Sister, Stepbrother, or Stepsister of Passenger Aboard Titanic
    //Spouse:   Husband or Wife of Passenger Aboard Titanic (Mistresses and Fiances Ignored)
    //Parent:   Mother or Father of Passenger Aboard Titanic
    //Child:    Son, Daughter, Stepson, or Stepdaughter of Passenger Aboard Titanic

    //Other family relatives excluded from this study include cousins,
    //nephews/nieces, aunts/uncles, and in-laws.  Some children travelled
    //only with a nanny, therefore parch=0 for them.  As well, some
    //travelled with very close friends or neighbors in a village, however,
    //the definitions do not support such relations.

    #endregion

    /// <summary>
    /// A passenger on the Titanic voyage
    /// </summary>
    public class Passenger
    {
        #region "Enums"

        /// <summary>
        /// The label used for prediction.
        /// </summary>
        public enum Survived
        {
            No = 0,
            Yes = 1
        }

        /// <summary>
        /// The economic status of the passenger
        /// </summary>
        public enum EconomicStatus
        {
            UpperClass = 1,
            MiddleClass = 2,
            LowerClass = 3
        }

        /// <summary>
        /// The port in which the passenger embarked on the boat
        /// </summary>
        public enum Ports
        {
            Cherbourg,
            Queenstown,
            Southampton
        }
        
        #endregion

        #region "Features"

        /// <summary>
        /// The sex of the passenger
        /// </summary>
        [Feature]
        public string Sex { get; set; }

        /// <summary>
        /// The age of the passenger.  If the passenger is
        /// less than a year old, then the age is an estimation
        /// and is a string in this format xx.5
        /// </summary>
        [Feature]
        public decimal Age { get; set; }

        /// <summary>
        /// If the passenger is an adult, this is field represents the number
        /// of Spouses (should be one)..
        /// If the passenger isn't an adult, this field represents the number
        /// of siblings that the passenger has onboard
        /// </summary>
        [Feature]
        public int SiblingOrSpouseCount { get; set; }

        /// <summary>
        /// If the passenger is a child, this count represents 
        /// </summary>
        [Feature]
        public int ParentOrChildCount { get; set; }


        /// <summary>
        /// Represents the port from which the passenger
        /// began their trip on the boat
        /// </summary>
        [Feature]
        public Ports PortEmbarkedFrom { get; set; }

        [Feature]
        public int EStatus { get; set; }

        #endregion

        #region "Label"

        /// <summary>
        /// This is the label in which we are predicting.
        /// This specifies if the passenger survived or died
        /// </summary>
        [Label]
        public Survived PassengerSurvived { get; set; }

        #endregion

        #region "Load data methods"

        private static List<Passenger> _passengers;

        /// <summary>
        /// Processes a CSV file and creates an array of Passenger instances
        /// based on the data.
        /// </summary>
        /// <param name="dataPath">The path to the training and testing data</param>
        /// <param name="train">
        ///     if training, the first column contains the
        ///     predictioin label.  If not, the first column doesn't contain
        ///     the prediction label.
        /// </param>
        /// <returns>
        ///     A collection of Passenger objects that is built from
        ///     the specified data file.
        /// </returns>
        public static Passenger[] LoadData(string dataPath, bool train)
        {
            _validateDataFilePath(dataPath);

            _passengers = new List<Passenger>();

            var firstLine = true;

            using (var reader = new StreamReader(dataPath))
            {
                string currentLine;
                while ((currentLine = reader.ReadLine()) != null)
                {
                    if (!firstLine)
                    {
                        _extractPassengerInstance(train, currentLine);
                    }
                    else
                    {
                        firstLine = false;
                    }
                }
            }

            return _passengers.ToArray();
        }

        private static void _extractPassengerInstance(bool train, string currentLine)
        {
            var tokens = currentLine.Split(',');

            decimal ageParsed = 0.0m;
            decimal.TryParse(tokens[3], out ageParsed);

            int siblingOrSpouseCountParsed = 0;
            int.TryParse(tokens[4], out siblingOrSpouseCountParsed);

            int parentOrChildCountParsed = 0;
            int.TryParse(tokens[5], out parentOrChildCountParsed);

            int eStatus;
            int.TryParse(tokens[1], out eStatus);

            Ports port;
            port = _parsePort(tokens);
            
            var currentPassenger = new Passenger
                {
                    EStatus = eStatus,
                    Sex = tokens[2],
                    Age = ageParsed,
                    SiblingOrSpouseCount = siblingOrSpouseCountParsed,
                    ParentOrChildCount = parentOrChildCountParsed,
                    PortEmbarkedFrom = port
                };

            if (train)
            {
                currentPassenger.PassengerSurvived = (tokens[0] == "0") ? Survived.No : Survived.Yes;
            }

            if (_passengers == null)
                _passengers = new List<Passenger>();

            _passengers.Add(currentPassenger);
        }

        private static Ports _parsePort(string[] tokens)
        {
            Ports port = Ports.Southampton;
            switch (tokens[6].ToUpper())
            {
                case "S":
                    port = Ports.Southampton;
                    break;
                case "Q":
                    port = Ports.Queenstown;
                    break;
                case "C":
                    port = Ports.Cherbourg;
                    break;
            }
            return port;
        }

        private static void _validateDataFilePath(string dataPath)
        {
            if (string.IsNullOrEmpty(dataPath))
                throw new ArgumentNullException("dataPath");

            if (!File.Exists(dataPath))
                throw new FileNotFoundException("unable to find data file {0}", dataPath);
        }

        public override string ToString()
        {
            return
                string.Format(
                    "\nSex {0}, \nAge {1}, \nClass {2}, \nSibling/Spouse {3}, \nParent/Child {4}, \nPort {5}, \nSurvived {6}",
                    this.Sex, this.Age, this.EStatus, SiblingOrSpouseCount, this.ParentOrChildCount,
                    this.PortEmbarkedFrom, this.PassengerSurvived);
        }

        #endregion

    }
}
