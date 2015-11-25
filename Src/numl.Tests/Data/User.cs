using numl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Tests.Data
{
    public enum Gender
    {
        Male,
        Female
    }

    public class User
    {
        public string Name { get; set; }

        [Feature]
        public int Age { get; set; }

        [Feature]
        public Gender Gender { get; set; }

        [Feature]
        public int Height { get; set; }

        [Feature]
        public int IQ { get; set; }

        public MovieRating[] MovieRatings { get; set; }

        public static User[] GetUsers()
        {
            return new User[]
            {
                new User() { Name = "Ada", Age = 24, Gender = Gender.Female, Height = 167, MovieRatings = new MovieRating[] {
                    new MovieRating() {  MovieId = 1, Rating = 5.0 },
                    new MovieRating() {  MovieId = 2, Rating = 3.0 },
                    new MovieRating() {  MovieId = 3, Rating = 4.0 },
                    new MovieRating() {  MovieId = 4, Rating = 3.0 },
                    new MovieRating() {  MovieId = 5, Rating = 4.0 },
                    new MovieRating() {  MovieId = 6, Rating = 5.0 },
                    new MovieRating() {  MovieId = 7, Rating = 4.0 }
                } },
                new User() { Name = "Guy", Age = 31, Gender = Gender.Male, Height = 141, MovieRatings = new MovieRating[] {
                    new MovieRating() {  MovieId = 1, Rating = 4.0 }
                } },
                new User() { Name = "Jamie", Age = 28, Gender = Gender.Male, Height = 132, MovieRatings = new MovieRating[] {
                } },
                new User() { Name = "", Age = 29, Gender = Gender.Female, Height = 176, MovieRatings = new MovieRating[] {
                } },
                new User() { Name = "", Age = 40, Gender = Gender.Female, Height = 136, MovieRatings = new MovieRating[] {
                    new MovieRating() {  MovieId = 1, Rating = 4.0 },
                    new MovieRating() {  MovieId = 2, Rating = 3.0 },
                } },
                new User() { Name = "", Age = 38, Gender = Gender.Male, Height = 109, MovieRatings = new MovieRating[] {
                    new MovieRating() {  MovieId = 1, Rating = 4.0 },
                    new MovieRating() {  MovieId = 7, Rating = 2.0 }
                } },
            };
        }
    }

    public class MovieRating
    {
        public int MovieId { get; set; }

        [Feature]
        public double Rating { get; set; }
    }
}
