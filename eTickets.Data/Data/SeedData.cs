using Bogus;
using eTickets.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTickets.Data.Data
{
    public class SeedData
    {
        private static Faker faker = null!;
        public static async Task InitAsync(ApplicationDbContext db)
        {
            if (await db.Actors.AnyAsync()) return;

            faker = new Faker("sv");

            var actors = GenerateActors(30);
            await db.AddRangeAsync(actors);

            var producers = GenerateProducers(20);
            await db.AddRangeAsync(producers);

            var cinamas = GenerateCinemas(20);
            await db.AddRangeAsync(cinamas);

            var movies = GenerateMovies(6);
            await db.AddRangeAsync(movies);

            //var enrollments = GenerateEnrollments(courses, students);
            //await db.AddRangeAsync(enrollments);

            await db.SaveChangesAsync();
        }

        private static IEnumerable<Actor> GenerateActors(int numberOfActors)
        {
            var Actors = new List<Actor>();

            for (int i = 0; i < numberOfActors; i++)
            {
                var actor = new Actor {

                 FullName = faker.Name.FullName(),
                 ProfilePictureURL = faker.Internet.Avatar(),
                 Bio = "This is the Bio of the second actor",};
                Actors.Add(actor);
            
            }

            return Actors;
        }
        private static IEnumerable<Producer> GenerateProducers(int numberOfActors)
        {
            var Producers = new List<Producer>();

            for (int i = 0; i < numberOfActors; i++)
            {
                var producer = new Producer
                {
                    FullName = faker.Name.FullName(),
                    ProfilePictureURL = faker.Internet.Avatar(),
                    Bio = "This is the Bio of the second producer",
                };
                Producers.Add(producer);

            }

            return Producers;
        }

        private static IEnumerable<Cinema> GenerateCinemas(int numberOfActors)
        {
            var Cinemas = new List<Cinema>();
            string[] CinemaName =
{
                "Cinema 1",
                "Cinema 2",
                "Cinema 3",
                "Cinema 4",
                "Cinema 5",
        
            };
            string[] CinemaLogo =
{
                "http://dotnethow.net/images/cinemas/cinema-1.jpeg",
                "http://dotnethow.net/images/cinemas/cinema-2.jpeg",
                "http://dotnethow.net/images/cinemas/cinema-3.jpeg",
                "http://dotnethow.net/images/cinemas/cinema-4.jpeg",
                "http://dotnethow.net/images/cinemas/cinema-5.jpeg",

            };
            for (int i = 0; i < numberOfActors; i++)
            {
                foreach (var name in CinemaName)
                {
                    foreach (var logo in CinemaLogo)
                    {

                        var cinema = new Cinema
                        {

                            //  var Courses = await GetCoursesAsync(AT, courselist, modulelist, documentlist, activitylist);

                            Name = name,
                            Logo = logo,
                            Description = "This is the Description of the cinema",
                        };
                        Cinemas.Add(cinema);
                    }
                }
            }

            return Cinemas;
        }

        private static IEnumerable<Movie> GenerateMovies(int numberOfActors)
        {
            var movies = new List<Movie>();
        
            string[] MovieLogo =
{
                "http://dotnethow.net/images/movies/movie-3.jpeg",
                "http://dotnethow.net/images/movies/movie-1.jpeg",
                "http://dotnethow.net/images/movies/movie-4.jpeg",
                "http://dotnethow.net/images/movies/movie-6.jpeg",
                "http://dotnethow.net/images/movies/movie-7.jpeg",
                "http://dotnethow.net/images/movies/movie-8.jpeg",

            };

            for (int i = 0; i < numberOfActors; i++)
            {

                foreach (var logo in MovieLogo)
                {
                   
                        var movie = new Movie
                        {
                            Name = faker.Lorem.Sentence(),
                            ImageURL = logo,
                            Description = faker.Lorem.Lines(),
                            Price = faker.Random.Double(100, 200),
                            StartDate = faker.Date.Past(30),
                            EndDate = faker.Date.Future(20),
                            CinemaId = faker.Random.Int(1, 5),
                            ProducerId = faker.Random.Int(1, 20),
                            MovieCategory = faker.PickRandom<MovieCategory>(),
                        };
                        movies.Add(movie);
                    }
                
            }

            return movies;
        }
    }
}
