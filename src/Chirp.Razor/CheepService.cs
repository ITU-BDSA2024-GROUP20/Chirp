using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps(int page);
    public List<CheepViewModel> GetCheepsFromAuthor(string author , int page);
}

public class CheepService : ICheepService
{
    private readonly DBFacade facade;
    
    public CheepService(DBFacade facade)
    {
        this.facade = facade;
    }
	
    public List<CheepViewModel> GetCheeps(int page)
    {
        string query = 
            @"SELECT u.username, m.text, m.pub_date
            FROM message m
            JOIN user u ON m.author_id = u.user_id
            ORDER by m.pub_date desc
            LIMIT 32
            OFFSET @page";
        return facade.Query(query, page);
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page)
    {
        string query = 
            @"SELECT u.username, m.text, m.pub_date
            FROM message m
            JOIN user u ON m.author_id = u.user_id
            WHERE u.username = @username
            ORDER by m.pub_date desc
            LIMIT 32
            OFFSET @page";
        // filter by the provided author name
        return facade.Query(query, page ,author);
    }

    public static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}

public class Cheep
{
    public int CheepId { get; set; }
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

public class CSDBService : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }
    
    public CSDBService(DbContextOptions<CSDBService> options) : base(options) { }
}

// Constructed for later usage, for when we find out what MessageDTO actually is
public interface ICheepRepository
{
    public Task CreateCheep(CheepDTO newCheep);
    public Task<List<CheepDTO>> ReadCheep(string userName);
    public Task UpdateCheep(CheepDTO alteredCheep);
}

public class CheepRepository : ICheepRepository
{
    CSDBService service;
    CheepRepository()
    {
        service = new CSDBService(new DbContextOptions<CSDBService>());
    }
    public Task CreateCheep(CheepDTO newCheep)
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
        
        throw new NotImplementedException(); // Don't know how to return a task. What is a task? :shrug:
    }

    public Task<List<CheepDTO>> ReadCheep(string? userName)
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

        service.SaveChanges();
        throw new NotImplementedException(); // I still don't know how to return a task, especially if it is of a list of cheeps, this is torture.
    }

    public Task UpdateCheep(CheepDTO alteredCheep)
    {
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