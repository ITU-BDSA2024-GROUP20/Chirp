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
    
    
    
    
    public List<CheepDTO> ReadCheep(int page, string? userName = null, string? self = null)
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
            if (self != null && self == userName)
            {
                query = (from author in service.Authors
                        where author.Name == userName
                        from authors in author.Following
                        from cheep in authors.Cheeps
                        orderby cheep.TimeStamp descending
                        select new { authors.Name, cheep.Text, cheep.TimeStamp, authors.Email }
                    ).Union(query).OrderByDescending(x => x.TimeStamp);

            }
            
            var result = query.Skip(page).Take(32).ToList();
        
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

    /**
     * While this function may imply deletion of said user, due to the current implementation of the userId system,should not be fully removed from the database.
     * However, their username, name, and email will be anonymised and who they followed.
     */
    public void DeleteAuthor(string username)
    {
        Author author = GetAuthorByName(username);
        author.Name = "[DELETED]";
        author.Email = "[DELETED]";
        author.UserName = "[DELETED]";
        if (author.Following != null)
        {
            author.Following.Clear();
        }
        service.SaveChanges();
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

    public void ToggleFollow(string self, string other)
    {
        if (isSelf(self, other))
            return;
        Author authorToFollow = GetAuthorByName(other);
        Author authorSelf = GetAuthorByName(self);
        if (authorSelf.Following == null)
        {
            authorSelf.Following = new List<Author>();
        }
        if (authorSelf.Following.Contains(authorToFollow))
        {
            authorSelf.Following.Remove(authorToFollow);
        }
        else
        {
            authorSelf.Following.Add(authorToFollow);
        }
        service.SaveChanges();
    }

    public bool isFollowing(string self, string other)
    {
        Author authorSelf = GetAuthorByName(self);
        if (authorSelf.Following == null)
        {
            authorSelf.Following = new List<Author>();
        }
        Author authorToFollow = GetAuthorByName(other);
        return authorSelf.Following.Contains(authorToFollow);
    }

    public bool isSelf(string self, string other)
    {
        Author authorToFollow = GetAuthorByName(other);
        Author authorSelf = GetAuthorByName(self);
        if (authorToFollow.Equals(authorSelf))
            return true;
        return false;
    }
}

