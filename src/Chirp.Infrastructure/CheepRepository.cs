using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using Chirp.Core;
using Microsoft.AspNetCore.Components.Authorization;

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
            var query = (
                from aut in service.Authors
                where aut.Email == newCheep.Email
                select aut);
            var author = query.First();
            
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
    
    
    
    
    public List<CheepDTO> ReadCheep(int page, string? userName = null, string? selfEmail = null)
    {
        List<CheepDTO> cheeps = new List<CheepDTO>();
        
            var query = (from message in service.Cheeps
                join author in service.Authors on message.AuthorId equals author.Id
                orderby message.TimeStamp descending 
                select new { author.Name, message.Text, message.TimeStamp, author.Email });
            
            if (selfEmail != null)
            {
                query = (from message in service.Cheeps
                    join author in service.Authors on message.AuthorId equals author.Id
                    where  !(from aut in service.Authors
                            where aut.Email == selfEmail
                            from blocks in aut.Blocking
                            select  blocks.Id 
                        ).Contains(message.AuthorId)
                    orderby message.TimeStamp descending 
                    select new { author.Name, message.Text, message.TimeStamp, author.Email });
            }
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

        return cheeps;
    }
    
}

