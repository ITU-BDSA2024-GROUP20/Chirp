using System.Net.Mime;
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
            CreateAuthor(newCheep.Author, newCheep.Email);
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
        if (userName != null)
        {
            var query = (from author in service.Authors
                    from message in author.Cheeps
                    where author.Name == userName
                    orderby message.TimeStamp descending
                    select new { author.Name, message.Text, message.TimeStamp, author.Email }
                    );
            var result =  query.Skip(page).Take(32).ToList();
            foreach (var message in result)
            {
                CheepDTO ch = new CheepDTO();
                ch.Author = message.Name;
                ch.Text = message.Text;
                ch.Timestamp = message.TimeStamp.ToString();
                ch.Email = message.Email;
                cheeps.Add(ch);
            }
        }
        else
        {
            var query = (from message in service.Cheeps
                join author in service.Authors on message.AuthorId equals author.AuthorId
                orderby message.TimeStamp descending 
                select new { author.Name, message.Text, message.TimeStamp, author.Email });
            var result =  query.Skip(page).Take(32).ToList();
            foreach (var message in result)
            {
                CheepDTO ch = new CheepDTO();
                ch.Author = message.Name;
                ch.Text = message.Text;
                ch.Timestamp = message.TimeStamp.ToString();
                ch.Email = message.Email;
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

    public void CreateAuthor(string name, string email)
    {
        Author author = new Author()
        {
            AuthorId = service.Authors.Count(),
            Name = name,
            Email = email,
            Cheeps = new List<Cheep>()
        };
        service.Authors.Add(author);
    }

    public Author GetAuthorByName(string name)
    {
        var query = (
            from author in service.Authors
            where author.Name == name
            select author
            );
        return query.FirstOrDefault();
    }

    public Author GetAuthorByEmail(string email)
    {
        var query = (
            from author in service.Authors
            where author.Email == email
            select author
        );
        return query.FirstOrDefault();
    }
}

public class CheepDTO
{
    public string Author { get; set; }
    public string Text { get; set; }
    public string Timestamp { get; set; }
    public string Email { get; set; }
}