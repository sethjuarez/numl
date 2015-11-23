using System;
using numl.Model;
using System.Linq;
using System.Collections.Generic;

namespace numl.Tests.Data
{
    public enum Outlook
    {
        Sunny,
        Overcast,
        Rainy
    }

    public enum Temperature
    {
        Hot,
        Mild,
        Cool
    }

    public enum Humidity
    {
        High,
        Normal
    }

    public class Tennis
    {
        [Feature]
        public Outlook Outlook { get; set; }
        [Feature]
        public Temperature Temperature { get; set; }
        [Feature]
        public Humidity Humidity { get; set; }
        [Feature]
        public bool Windy { get; set; }
        [Label]
        public bool Play { get; set; }

        public static Tennis Make(Outlook outlook, Temperature temperature, Humidity humidity, bool windy, bool play)
        {
            return new Tennis
            {
                Outlook = outlook,
                Temperature = temperature,
                Humidity = humidity,
                Windy = windy,
                Play = play
            };
        }

        public static Tennis[] GetData()
        {
            return new Tennis[]
            {
                Tennis.Make(Outlook.Sunny, Temperature.Hot, Humidity.High, false, false),
                Tennis.Make(Outlook.Sunny, Temperature.Hot, Humidity.High, true,  false),
                Tennis.Make(Outlook.Overcast, Temperature.Hot, Humidity.High, false, true),
                Tennis.Make(Outlook.Rainy, Temperature.Mild, Humidity.High, false, true),
                Tennis.Make(Outlook.Rainy, Temperature.Cool, Humidity.Normal, false, true),
                Tennis.Make(Outlook.Rainy, Temperature.Cool, Humidity.Normal, true, false),
                Tennis.Make(Outlook.Overcast, Temperature.Cool, Humidity.Normal, true, true),
                Tennis.Make(Outlook.Sunny, Temperature.Mild, Humidity.High, false, false),
                Tennis.Make(Outlook.Sunny, Temperature.Cool, Humidity.Normal, false, true),
                Tennis.Make(Outlook.Rainy, Temperature.Mild, Humidity.Normal, false, true),
                Tennis.Make(Outlook.Sunny, Temperature.Mild, Humidity.Normal, true, true),
                Tennis.Make(Outlook.Overcast, Temperature.Mild, Humidity.High, true, true),
                Tennis.Make(Outlook.Overcast, Temperature.Hot, Humidity.Normal, false, true),
                Tennis.Make(Outlook.Rainy, Temperature.Mild, Humidity.High, true, false)
            };
        }
    }
}
