using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor;

public class ChirpDBContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }
    
    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options) { }
}

public class Cheep
{
    public int CheepId { get; set; }
    
    [Required]
    [StringLength(160)]
    public string Text { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; }
    public DateTime TimeStamp { get; set; }
}

public class Author
{
    public int AuthorId { get; set; }
    public string Name { get; set; }
    public ICollection<Cheep> Cheeps { get; set; }
    public string Email { get; set; }
}

public interface ICheepRepository
{
    public void CreateCheep(CheepDTO newCheep);
    public List<CheepDTO> ReadCheep(string userName);
    public void UpdateCheep(CheepDTO alteredCheep);
}

public class CheepRepository : ICheepRepository
{
    private ChirpDBContext service;
    public CheepRepository()
    {
        service = new ChirpDBContext(new DbContextOptions<ChirpDBContext>());
    }
    public void CreateCheep(CheepDTO newCheep)
    {
        Author author = service.Authors.Find(newCheep.Author);
        if (author == null) // If no matching author was found
        {
            int nr = service.Authors.Count();
            author.AuthorId = nr + 1;
            author.Name = newCheep.Author;
            author.Email = newCheep.Email;
            author.Cheeps = new List<Cheep>();
            service.Authors.Add(author);
        }

        Cheep cheep = new Cheep();
        cheep.CheepId = service.Cheeps.Count();
        cheep.AuthorId = author.AuthorId;
        cheep.Author = author;
        cheep.Text = newCheep.Text;
        cheep.TimeStamp = DateTime.Parse(newCheep.Timestamp);
        
        author.Cheeps.Add(cheep);
        
        service.Cheeps.Add(cheep);
        service.Authors.Update(author);
        service.SaveChanges();
    }

    public List<CheepDTO> ReadCheep(string? userName)
    {
        List<CheepDTO> cheeps = new List<CheepDTO>();
        Author author = service.Authors.Find(userName);
        if (author != null)
        {
            foreach (Cheep cheep in author.Cheeps)
            {
                CheepDTO ch = new CheepDTO();
                ch.Author = cheep.Author.Name;
                ch.Text = cheep.Text;
                ch.Timestamp = cheep.TimeStamp.ToString();
                ch.Email = cheep.Author.Email;
                cheeps.Add(ch);
            }
        }
        else
        {
            foreach (Cheep cheep in service.Cheeps.AsNoTracking().ToList())
            {
                CheepDTO ch = new CheepDTO();
                ch.Author = cheep.Author.Name;
                ch.Text = cheep.Text;
                ch.Timestamp = cheep.TimeStamp.ToString();
                ch.Email = cheep.Author.Email;
                cheeps.Add(ch);
            }
        }

        return cheeps;
    }

    public void UpdateCheep(CheepDTO alteredCheep)
    {
        // This does not currently make sense within the bounds of the database. Maybe return here later.
        throw new NotImplementedException();
    }
}

public class CheepDTO
{
    public string Author { get; set; }
    public string Text { get; set; }
    public string Timestamp { get; set; }
    public string Email { get; set; }
}