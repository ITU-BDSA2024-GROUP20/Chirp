using Microsoft.AspNetCore.Identity;

namespace Chirp.Infrastructure;
using Core;

public class AuthorRepository : IAuthorRepository
{
    private ChirpDBContext service;
    private SignInManager<Author> signInManager;

    public AuthorRepository(ChirpDBContext service, SignInManager<Author> signInManager)
    {
        this.service = service;
        this.signInManager = signInManager;
    }

    public List<CheepDTO> AuthorCheep(int page, string userName, string self)
    {
        List<CheepDTO> cheeps = new List<CheepDTO>();
        var query = (from author in service.Authors
                from message in author.Cheeps
                where author.Name == userName
                orderby message.TimeStamp descending
                select new { author.Name, message.Text, message.TimeStamp, author.Email }
            );
        if (self != null && self.Equals(userName))
        {
            query = (from author in service.Authors
                where author.Name == userName
                from authors in author.Following
                from cheep in authors.Cheeps
                where  (from aut in service.Authors
                        where aut.Name == userName
                        from blocks in aut.Following
                        select  blocks.Id 
                    ).Contains(cheep.AuthorId)
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
        return cheeps;
    }
    
    
    /// <summary>
    /// While this function may imply deletion of said user, due to the current implementation of the userId system, should not be fully removed from the database.
    /// However, their username, name, and email will be anonymised and who they followed.
    /// </summary>
    public async void DeleteAuthor(string username)
    {
        var info = await signInManager.GetExternalLoginInfoAsync();
        Author author = GetAuthorByName(username);
        if (info != null)
        {
             signInManager.UserManager.RemoveLoginAsync(author,
                info.LoginProvider, info.ProviderKey);
        }
        author.Name = "[DELETED]";
        author.Email = "[DELETED]";
        author.UserName = "[DELETED]";
        author.PasswordHash = "[DELETED]";
        author.SecurityStamp = "[DELETED]";
        author.ConcurrencyStamp = "[DELETED]";
        author.NormalizedEmail = "[DELETED " + author.Id + "]";
        author.NormalizedUserName = "[DELETED " + author.Id + "]";
        if (author.Following != null)
        {
            author.Following.Clear();
        }
        service.SaveChanges();
    }

    public AuthorDTO GetAuthorDtoByName(string name)
    {
        var query = (
            from author in service.Authors
            where author.Name == name
            select new {author.Name, author.UserName}
        );
        AuthorDTO authordto = new AuthorDTO();
        var result = query.ToList();
        foreach (var author in result)
        {
            authordto.Name = author.Name;
            authordto.Email = author.UserName;
        }

        return authordto;
    }

    public AuthorDTO GetAuthorDtoByEmail(string email)
    {
        var query = (
            from author in service.Authors
            where author.Email == email
            select author
        );
        AuthorDTO authordto = new AuthorDTO();
        var result = query.ToList();
        foreach (var author in result)
        {
            authordto.Name = author.Name;
            authordto.Email = author.UserName;
        }

        return authordto;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name">string with username of user</param>
    /// <returns><c>Author</c> of the first name that matches the name</returns>
    private Author GetAuthorByName(string name)
    {
        var query = (
            from author in service.Authors
            where author.Name == name
            select author
            );
        return query.First();
    }

    private Author GetAuthorByEmail(string email)
    {
        var query = (
            from author in service.Authors
            where author.Email == email
            select author
        );
        return query.First();
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

    public List<AuthorDTO> GetFollowing(string self)
    {
        List<AuthorDTO> following = new List<AuthorDTO>();
        var query = (from author in service.Authors
            from follow in author.Following
            where author.Name == self
            orderby follow.Name
            select new { follow.Name });
        var result = query.ToList();
        foreach (var follow in result)
        {
            var author = new AuthorDTO();
            author.Name = follow.Name;
            following.Add(author);
        }
        return following;
    }
    
    
    
}