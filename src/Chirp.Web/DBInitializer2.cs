using Microsoft.AspNetCore.Identity;

namespace Chirp.Web;
using Chirp.Infrastructure;
using Chirp.Core;


public class DBInitializer2
{
    public static void SeedDatabase2(ChirpDBContext chirpContext, IServiceProvider serviceProvider)
    {
        Console.WriteLine("Seeding database...");
        if (!(chirpContext.Authors.Any() && chirpContext.Cheeps.Any()))
        {
            var usermanager = serviceProvider.GetRequiredService<UserManager<Author>>();
            var a1 = new Author()
            {
                Id = "611e3fa1-be3b-413d-b7d7-333738c17a3a",UserName = "ropf@itu.dk", Email = "ropf@itu.dk", Cheeps = new List<Cheep>()
            };
            var a2 = new Author()
            {
                Id = "bc14be86-0984-4ab0-8d0a-0f3b90bb5c2e", UserName = "adho@itu.dk", Email = "adho@itu.dk", Cheeps = new List<Cheep>()
            };
            usermanager.CreateAsync(a1, "LetM31n!");
            usermanager.CreateAsync(a2, "M32Want_Access");

            //var authors = new List<Author>() { a1, a2 };

            var c1 = new Cheep()
            {
                CheepId = 1, AuthorId = a1.Id, Author = a1, Text = "Hello, BDSA students!",
                TimeStamp = DateTime.Parse("2023-08-01 12:16:48")
            };
            var c2 = new Cheep()
            {
                CheepId = 2, AuthorId = a2.Id, Author = a2, Text = "Hej, velkommen til kurset.",
                TimeStamp = DateTime.Parse("2023-08-01 13:08:28")
            };

            var cheeps = new List<Cheep>() { c1, c2 };

            a1.Cheeps = new List<Cheep>() { c1 };
            a2.Cheeps = new List<Cheep>() { c2 };


            //chirpContext.Authors.AddRange(authors);
            chirpContext.Cheeps.AddRange(cheeps);
            chirpContext.SaveChanges();
        }

    }
}