namespace Chirp.Web;
using Chirp.Infrastructure;
using Chirp.Core;


public class DBInitializer2
{
    public static void SeedDatabase2(ChirpDBContext chirpContext)
    {
        Console.WriteLine("Seeding database...");
        if (!(chirpContext.Authors.Any() && chirpContext.Cheeps.Any()))
        {
            var a1 = new Author()
            {
                AuthorId = 1 , Name = "Helge", Email = "ropf@itu.dk", Cheeps = new List<Cheep>(), Password = "LetM31n!" 
            };
            var a2 = new Author()
            {
                AuthorId = 2, Name = "Adrian", Email = "adho@itu.dk", Cheeps = new List<Cheep>(),
                Password = "M32Want_Access"
            };

            var authors = new List<Author>() { a1, a2 };

            var c1 = new Cheep()
            {
                CheepId = 1, AuthorId = a1.AuthorId, Author = a1, Text = "Hello, BDSA students!",
                TimeStamp = DateTime.Parse("2023-08-01 12:16:48")
            };
            var c2 = new Cheep()
            {
                CheepId = 2, AuthorId = a2.AuthorId, Author = a2, Text = "Hej, velkommen til kurset.",
                TimeStamp = DateTime.Parse("2023-08-01 13:08:28")
            };

            var cheeps = new List<Cheep>() { c1, c2 };

            a1.Cheeps = new List<Cheep>() { c1 };
            a2.Cheeps = new List<Cheep>() { c2 };


            chirpContext.Authors.AddRange(authors);
            chirpContext.Cheeps.AddRange(cheeps);
            chirpContext.SaveChanges();
        }

    }
}