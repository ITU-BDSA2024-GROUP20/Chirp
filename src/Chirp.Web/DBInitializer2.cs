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
            var a11 = new Author()
            {
                AuthorId = 11, Name = "Helge", Email = "ropf@itu.dk", Cheeps = new List<Cheep>(), Password = "LetM31n!"
            };
            var a12 = new Author()
            {
                AuthorId = 12, Name = "Adrian", Email = "adho@itu.dk", Cheeps = new List<Cheep>(),
                Password = "M32Want_Access"
            };

            var authors = new List<Author>() { a11, a12 };

            var c656 = new Cheep()
            {
                CheepId = 656, AuthorId = a11.AuthorId, Author = a11, Text = "Hello, BDSA students!",
                TimeStamp = DateTime.Parse("2023-08-01 12:16:48")
            };
            var c657 = new Cheep()
            {
                CheepId = 657, AuthorId = a12.AuthorId, Author = a12, Text = "Hej, velkommen til kurset.",
                TimeStamp = DateTime.Parse("2023-08-01 13:08:28")
            };

            var cheeps = new List<Cheep>() { c656, c657 };

            a11.Cheeps = new List<Cheep>() { c656 };
            a12.Cheeps = new List<Cheep>() { c657 };


            chirpContext.Authors.AddRange(authors);
            chirpContext.Cheeps.AddRange(cheeps);
            chirpContext.SaveChanges();
        }

    }
}