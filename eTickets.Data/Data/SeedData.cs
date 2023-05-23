using Bogus;
using eTickets.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        private static ApplicationDbContext db = default!;
        private static RoleManager<IdentityRole> roleManager = default!;
        private static UserManager<ApplicationUser> userManager = default!;

   
        private static async Task AddToRolesAsync(ApplicationUser admin, string[] roleNames)
        {
            foreach (var role in roleNames)
            {
                if (await userManager.IsInRoleAsync(admin, role)) continue;
                var result = await userManager.AddToRoleAsync(admin, role);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        private static async Task<ApplicationUser> AddAdminAsync(string adminEmail, string adminPW)
        {
            var found = await userManager.FindByEmailAsync(adminEmail);

            if (found != null) return null!;

            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Admin",
            };

            var result = await userManager.CreateAsync(admin, adminPW);
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

            return admin;
        }
        private static async Task AddRolesAsync(string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (await roleManager.RoleExistsAsync(roleName)) continue;
                var role = new IdentityRole { Name = roleName };
                var result = await roleManager.CreateAsync(role);

                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        public static async Task InitAsync(ApplicationDbContext db, IServiceProvider services, string adminPW)
        {
            if (await db.Actors.AnyAsync()) return;

            faker = new Faker("sv");
            if (db is null) throw new ArgumentNullException(nameof(db));

            ArgumentNullException.ThrowIfNull(nameof(services));
             if (services is null) throw new ArgumentNullException(nameof(services));

              // if (db.app.Any()) return;

            roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            ArgumentNullException.ThrowIfNull(roleManager);

            userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            ArgumentNullException.ThrowIfNull(userManager);

            var roleNames = new[] { "Member", "Admin" };
            var adminEmail = "admin@etickets.se";


            await AddRolesAsync(roleNames);

            var admin = await AddAdminAsync(adminEmail, adminPW);

            await AddToRolesAsync(admin, roleNames);

            var actors = GenerateActors(6);
            await db.AddRangeAsync(actors);

            var producers = GenerateProducers(6);
            await db.AddRangeAsync(producers);

            var cinamas = GenerateCinemas(5);
            await db.AddRangeAsync(cinamas);

            var movies = GenerateMovies(1 ,cinamas, producers);
           await db.AddRangeAsync(movies);

            var enrollments = GenerateEnrollments(actors, movies);
            await db.AddRangeAsync(enrollments);

            await db.SaveChangesAsync();
        }
        private static IEnumerable<Actor_Movie> GenerateEnrollments(IEnumerable<Actor> actors, IEnumerable<Movie> movies)
        {

            var enrollments = new List<Actor_Movie>();


            foreach (var actor in actors)
            {
                foreach (var movie in movies)
                {
                   
                        var enrollment = new Actor_Movie
                        {
                            Movie = movie,
                            Actor = actor,
                        };

                        enrollments.Add(enrollment);
                    


                }
            }




            return enrollments;

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

        public static IEnumerable<Cinema> GenerateCinemas(int numberOfActors)
        {
            var Cinemas = new List<Cinema>();
          
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
              
                        var cinema = new Cinema
                        {
                            Name = faker.Company.CatchPhrase() +"Cinema",
                            Logo = faker.PickRandom(CinemaLogo),
                            Description = "This is the Description of the cinema",
                        };
                        Cinemas.Add(cinema);
                    
                
            }

            return Cinemas;
        }

        private static IEnumerable<Movie> GenerateMovies(int numberOfActors, IEnumerable<Cinema> cinemas, IEnumerable<Producer> producers)
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



           
                    foreach (var cinema in cinemas)
                    {
                        foreach (var producer in producers)
                        { 
                        for (int i = 0; i < numberOfActors; i++)
                         {
                            var movie = new Movie
                            {
                                Name = faker.Company.CatchPhrase(),
                                ImageURL = faker.PickRandom(MovieLogo),
                                Description = faker.Hacker.Verb(),
                                Price = faker.Random.Int(100, 200),
                                StartDate = DateTime.Now.AddDays(faker.Random.Int(-10, 10)),
                                EndDate = DateTime.Now.AddDays(faker.Random.Int(-5, 5)),
                                Cinema = cinema,
                                Producer =producer,
                                MovieCategory = faker.PickRandom<MovieCategory>(),
                            };
                            movies.Add(movie);
                        }
                    //} 
                }
            }

            return movies;
        }
    }
}
