using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using Chirp.Core;

namespace Chirp.Infrastructure;

public class CheepRepository : ICheepRepository
{
    private ChirpDBContext service;
    public CheepRepository(ChirpDBContext service)
    {
        this.service = service;
    }

    public bool validdateString(string str)
    {
        if (str == null)
            return false;
        if (str.Length <= 160 && str.Length > 0)
        {
            return true;
        }
        return false;
    }
    
    public void CreateCheep(CheepDTO newCheep)
    {
        if (validdateString(newCheep.Text))
        {
            Author author = GetAuthorByEmail(newCheep.Email);
            if (author.Cheeps == null)
                author.Cheeps = new List<Cheep>();
            Console.WriteLine(author.Cheeps.Count());

            Cheep cheep = new Cheep
            {
                CheepId = service.Cheeps.Count() + 1,
                AuthorId = author.Id,
                Author = author,
                Text = newCheep.Text,
                TimeStamp = DateTime.Parse(newCheep.Timestamp)
            };

            author.Cheeps.Add(cheep);

            service.Cheeps.Add(cheep);
            service.Authors.Update(author);
            service.SaveChanges();
        }
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
                CheepDTO ch = new CheepDTO
                {
                    Author = message.Name,
                    Text = message.Text,
                    Timestamp = message.TimeStamp.ToString(),
                    Email = message.Email
                };
                cheeps.Add(ch);
            }
        }
        else
        {
            var query = (from message in service.Cheeps
                join author in service.Authors on message.AuthorId equals author.Id
                orderby message.TimeStamp descending 
                select new { author.Name, message.Text, message.TimeStamp, author.Email });
            var result =  query.Skip(page).Take(32).ToList();
            foreach (var message in result)
            {
                CheepDTO ch = new CheepDTO
                {
                    Author = message.Name,
                    Text = message.Text,
                    Timestamp = message.TimeStamp.ToString(),
                    Email = message.Email
                };
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

    public void DeleteCheep(int cheepId)
    {
        service.Cheeps.Remove(service.Cheeps.Find(cheepId));
        service.SaveChanges();
    }

    public void DeleteCheepsByAuthor(string authorId)
    {
        var query = (from author in service.Authors
                from message in author.Cheeps
                where author.Id == authorId
                orderby message.TimeStamp descending
                select new { message.CheepId }
            );
        var result =  query.ToList();
        for (int i = 0; i < result.Count(); i++)
        {
            service.Cheeps.Remove(service.Cheeps.Find(result[i].CheepId));
        }
        service.Authors.Remove(service.Authors.Find(authorId));
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

