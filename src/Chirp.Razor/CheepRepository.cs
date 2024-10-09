using Microsoft.EntityFrameworkCore;
namespace Chirp.Razor;

public interface ICheepRepository
{
    public void CreateCheep(CheepDTO newCheep);
    public List<CheepDTO> ReadCheep( int page , string userName);
    public void UpdateCheep(CheepDTO alteredCheep);
}

public class CheepRepository : ICheepRepository
{
    private ChirpDBContext service;
    public CheepRepository(ChirpDBContext service)
    {
        this.service = service;
        DbInitializer.SeedDatabase(this.service);
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

    public List<CheepDTO> ReadCheep(int page, string? userName = null)
    {
        List<CheepDTO> cheeps = new List<CheepDTO>();
        Author author = service.Authors.Find(userName);
        if (author != null)
        {
            List<Cheep> cheepList = author.Cheeps.ToList();
            for (int i = page; i < page + 32 && i < service.Cheeps.AsNoTracking().ToList().Count(); i++)
            {
                CheepDTO ch = new CheepDTO();
                ch.Author = cheepList[i].Author.Name;
                ch.Text = cheepList[i].Text;
                ch.Timestamp = cheepList[i].TimeStamp.ToString();
                ch.Email = cheepList[i].Author.Email;
                cheeps.Add(ch);
            }
        }
        else
        {
            List<Cheep> cheepList = service.Cheeps.AsNoTracking().ToList();
            for (int i = page; i < page + 32 && i < cheepList.Count(); i++)
            {
                CheepDTO ch = new CheepDTO();
                ch.Author = cheepList[i].Author.Name;
                ch.Text = cheepList[i].Text;
                ch.Timestamp = cheepList[i].TimeStamp.ToString();
                ch.Email = cheepList[i].Author.Email;
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