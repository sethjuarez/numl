using numl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Tests.Data
{
    public class Movie
    {
        public int MovieId { get ; set; }

        public string Name { get; set; }

        [Feature]
        public double ActionScenes { get; set; }

        [Feature]
        public double ComedyScenes { get; set; }

        [Feature]
        public double LoveScenes { get; set; }

        [Feature]
        public double GoryScenes { get; set; }

        [Feature]
        public double ZombieScenes { get; set; }

        public static Movie[] GetMovies()
        {
            return new Movie[]
            {
                new Movie() { MovieId = 1, Name = "Romantic Romano", ActionScenes = 1.048686, ComedyScenes = -0.400232, LoveScenes =  1.194119, GoryScenes =  0.371128, ZombieScenes =  0.407607 },
                new Movie() { MovieId = 2, Name = "Gods", ActionScenes = 0.780851, ComedyScenes = -0.385626, LoveScenes =  0.521198, GoryScenes =  0.227355, ZombieScenes =  0.570109 },
                new Movie() { MovieId = 3, Name = "Hunt the Truth", ActionScenes = 0.641509, ComedyScenes = -0.547854, LoveScenes = -0.083796, GoryScenes = -0.598519, ZombieScenes = -0.017694 },
                new Movie() { MovieId = 4, Name = "The Trail", ActionScenes = 0.453618, ComedyScenes = -0.800218, LoveScenes =  0.680481, GoryScenes = -0.081743, ZombieScenes =  0.136601 },
                new Movie() { MovieId = 5, Name = "Waiting for Time", ActionScenes = 0.937538, ComedyScenes =  0.106090, LoveScenes =  0.361953, GoryScenes =  0.086646, ZombieScenes =  0.287505 },
                new Movie() { MovieId = 6, Name = "Aliens From Earth", ActionScenes = 0.072619, ComedyScenes = -0.508257, LoveScenes =  0.052991, GoryScenes = -0.083351, ZombieScenes =  0.328765 },
                new Movie() { MovieId = 7, Name = "Down The Drain", ActionScenes = 0.418129, ComedyScenes = -0.560677, LoveScenes =  0.481836, GoryScenes = -0.361659, ZombieScenes =  0.268461 }
            };
        }
    }
}
