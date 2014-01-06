using numl.Data;
using numl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Data
{
    public enum NumberOfPlayers
    {
        Zero,
        One,
        Two,
        Three,
        Four
    }

    public class TennisWithPlayers : Tennis
    {
        [Feature]
        public NumberOfPlayers NumberOfPlayers { get; set; }


        public static TennisWithPlayers Make(Outlook outlook, Temperature temperature, Humidity humidity, bool windy, NumberOfPlayers numberOfPlayers, bool play)
        {
            return new TennisWithPlayers
            {
                Outlook = outlook,
                Temperature = temperature,
                Humidity = humidity,
                Windy = windy,
                NumberOfPlayers = numberOfPlayers,
                Play = play
            };
        }

        public static Tennis[] GetData()
        {
            return new Tennis[]  {
                TennisWithPlayers.Make(Outlook.Sunny, Temperature.Hot, Humidity.High, false,NumberOfPlayers.Two, false),
                TennisWithPlayers.Make(Outlook.Sunny, Temperature.Hot, Humidity.High, true,  NumberOfPlayers.Two,false),
                TennisWithPlayers.Make(Outlook.Overcast, Temperature.Hot, Humidity.High, false, NumberOfPlayers.Two,true),
                TennisWithPlayers.Make(Outlook.Rainy, Temperature.Mild, Humidity.High, false, NumberOfPlayers.Two,true),
                TennisWithPlayers.Make(Outlook.Rainy, Temperature.Cool, Humidity.Normal, false, NumberOfPlayers.Two,true),
                TennisWithPlayers.Make(Outlook.Rainy, Temperature.Cool, Humidity.Normal, true, NumberOfPlayers.Four,false),
                TennisWithPlayers.Make(Outlook.Overcast, Temperature.Cool, Humidity.Normal, true, NumberOfPlayers.Two,true),
                TennisWithPlayers.Make(Outlook.Sunny, Temperature.Mild, Humidity.High, false, NumberOfPlayers.Four,false),
                TennisWithPlayers.Make(Outlook.Sunny, Temperature.Cool, Humidity.Normal, false, NumberOfPlayers.Two,true),
                TennisWithPlayers.Make(Outlook.Rainy, Temperature.Mild, Humidity.Normal, false, NumberOfPlayers.Two,true),
                TennisWithPlayers.Make(Outlook.Sunny, Temperature.Hot, Humidity.High, true, NumberOfPlayers.Two,false),
                TennisWithPlayers.Make(Outlook.Overcast, Temperature.Mild, Humidity.High, true, NumberOfPlayers.Two,true),
                TennisWithPlayers.Make(Outlook.Rainy, Temperature.Mild, Humidity.Normal, false, NumberOfPlayers.Four,true),
                TennisWithPlayers.Make(Outlook.Sunny, Temperature.Mild, Humidity.Normal, true, NumberOfPlayers.Four,true),
                TennisWithPlayers.Make(Outlook.Overcast, Temperature.Mild, Humidity.High, true, NumberOfPlayers.Four,true),
                TennisWithPlayers.Make(Outlook.Overcast, Temperature.Hot, Humidity.Normal, false, NumberOfPlayers.Two,true),
                TennisWithPlayers.Make(Outlook.Rainy, Temperature.Mild, Humidity.High, true, NumberOfPlayers.Two,false),
                TennisWithPlayers.Make(Outlook.Sunny, Temperature.Cool, Humidity.Normal, false, NumberOfPlayers.Three,false),
                TennisWithPlayers.Make(Outlook.Rainy, Temperature.Mild, Humidity.Normal, false, NumberOfPlayers.One,false),
                TennisWithPlayers.Make(Outlook.Overcast, Temperature.Hot, Humidity.High, false, NumberOfPlayers.One,false),
                TennisWithPlayers.Make(Outlook.Rainy, Temperature.Mild, Humidity.High, false, NumberOfPlayers.Three,false),
                TennisWithPlayers.Make(Outlook.Sunny, Temperature.Hot, Humidity.High, false,NumberOfPlayers.One, false),
                TennisWithPlayers.Make(Outlook.Sunny, Temperature.Hot, Humidity.High, true,  NumberOfPlayers.One,false),
                TennisWithPlayers.Make(Outlook.Overcast, Temperature.Hot, Humidity.High, false, NumberOfPlayers.One,false),
                TennisWithPlayers.Make(Outlook.Rainy, Temperature.Mild, Humidity.High, false, NumberOfPlayers.One,false),
            };
        }
    }
}
