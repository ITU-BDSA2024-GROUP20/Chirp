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
    public void CreateCheep(CheepDTO newCheep)
    {
        Author author = GetAuthorByName(newCheep.Author);
        
        if (author == null) // If no matching author was found
        {
            author = GetAuthorByName(newCheep.Author);
        }

        Cheep cheep = new Cheep
        {
            CheepId = service.Cheeps.Count(),
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

