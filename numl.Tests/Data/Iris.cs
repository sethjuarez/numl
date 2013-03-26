using numl.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Tests.Data
{
    public class Iris
    {
        [Feature]
        public decimal SepalLength { get; set; }
        [Feature]
        public decimal SepalWidth { get; set; }
        [Feature]
        public decimal PetalLength { get; set; }
        [Feature]
        public decimal PetalWidth { get; set; }
        [StringLabel]
        public string Class { get; set; }

        public static Iris[] Load()
        {
            if (File.Exists("Data\\iris.data"))
            {
                List<Iris> data = new List<Iris>();
                using (var reader = File.OpenText("Data\\iris.data"))
                {
                    while (!reader.EndOfStream)
                    {
                        var tokens = reader.ReadLine().Split(',');
                        if (tokens.Length == 5)
                        {
                            Iris f = new Iris
                            {
                                SepalLength = decimal.Parse(tokens[0]),
                                SepalWidth = decimal.Parse(tokens[1]),
                                PetalLength = decimal.Parse(tokens[2]),
                                PetalWidth = decimal.Parse(tokens[3]),
                                Class = tokens[4],
                            };

                            data.Add(f);
                        }
                    }
                }

                return data.ToArray();
            }
            else
                return new Iris[] { };
        }
    }
}
