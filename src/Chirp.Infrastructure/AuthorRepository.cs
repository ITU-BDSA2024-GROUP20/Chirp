using Microsoft.AspNetCore.Identity;

namespace Chirp.Infrastructure;
using Core;

public class AuthorRepository : IAuthorRepository
{
    private readonly ChirpDBContext _service;
    private readonly SignInManager<Author> _signInManager;

    public AuthorRepository(ChirpDBContext service, SignInManager<Author> signInManager)
    {
        this._service = service;
        this._signInManager = signInManager;
    }

    public List<CheepDTO> AuthorCheep(int page, string? user, string? selfemail)
    {
        if (user != null && !(user.Contains("@")))
        {
            user = GetAuthorDtoByName(user).Email;
        }

        List<CheepDTO> cheeps = new List<CheepDTO>();
        var query = (from author in _service.Authors
                from message in author.Cheeps
                where author.Email == user
                orderby message.TimeStamp descending
                select new { author.Name, message.Text, message.TimeStamp, author.Email }
            );
        if (selfemail != null && selfemail.Equals(user))
        {
            query = (from author in _service.Authors
                where author.Email == selfemail
                from authors in author.Following
                from cheep in authors.Cheeps
                where  !(from aut in _service.Authors
                        where aut.Email == selfemail
                        from blocks in aut.Blocking
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
    public async void DeleteAuthor(string? email)
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        Author author = GetAuthorByEmail(email);
        if (info != null)
        {
             await _signInManager.UserManager.RemoveLoginAsync(author,
                 info.LoginProvider, info.ProviderKey);
        }

        var temp = GenerateRandNum();
        author.Name = "[DELETED"+temp+"]";
        author.Email = "[DELETED"+temp+"]";
        author.UserName = "[DELETED]";
        author.PasswordHash = "[DELETED]";
        author.SecurityStamp = "[DELETED]";
        author.ConcurrencyStamp = "[DELETED]";
        author.NormalizedEmail = "[DELETED " + author.Id + "]";
        author.NormalizedUserName = "[DELETED " + author.Id + "]";
        author.Following.Clear();
        _service.SaveChanges();
    }
    
    private string GenerateRandNum()
    {
        Random random = new Random();
        string r = "";
        for (int i = 0; i < 9; i++)
        {
            r += random.Next(0, 9).ToString();
        }
        return r;
    }
    

    public AuthorDTO GetAuthorDtoByName(string? name)
    {
        var query = (
            from author in _service.Authors
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

    public AuthorDTO GetAuthorDtoByEmail(string? email)
    {
        var query = (
            from author in _service.Authors
            where author.Email == email
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name">string with username of user</param>
    /// <returns><c>Author</c> of the first name that matches the name</returns>
    private Author GetAuthorByName(string name)
    {
        var query = (
            from author in _service.Authors
            where author.Name == name
            select author
            );
        return query.First();
    }

    private Author GetAuthorByEmail(string? email)
    {
        var query = (
            from author in _service.Authors
            where author.Email == email
            select author
        );
        return query.First();
    }

    public void ToggleFollow(string? selfEmail, string? otherEmail)
    {
        if (isSelf(selfEmail, otherEmail))
            return;
        Author authorToFollow = GetAuthorByEmail(otherEmail);
        Author authorself = GetAuthorByEmail(selfEmail);
        

        if (isFollowing(selfEmail, otherEmail))
        {
            authorself.Following.Remove(authorToFollow);
        }
        else
        {
            authorself.Following.Add(authorToFollow);
        }
        _service.Authors.Update(authorself);
        _service.SaveChanges();
    }

    public bool isFollowing(string? selfEmail, string? otherEmail)
    {
        var query = (from author in _service.Authors
                where author.Email == selfEmail
                from follow in author.Following
                where follow.Email == otherEmail
                select follow
            );
        var result = query.ToList();
        return result.Count > 0;
    }

    public bool isSelf(string? selfEmail, string? otherEmail)
    {
        Author authorToFollow = GetAuthorByEmail(otherEmail);
        Author authorSelf = GetAuthorByEmail(selfEmail);
        if (authorToFollow.Equals(authorSelf))
            return true;
        return false;
    }

    public List<AuthorDTO> GetFollowing(string? selfEmail)
    {
        List<AuthorDTO> following = new List<AuthorDTO>();
        var query = (from author in _service.Authors
            from follow in author.Following
            where author.Email == selfEmail
            orderby follow.Name
            select new { follow.Name, follow.Email });
        var result = query.ToList();
        foreach (var follow in result)
        {
            var author = new AuthorDTO();
            author.Name = follow.Name;
            author.Email = follow.Email;
            following.Add(author);
        }
        return following;
    }

    public void ToggleBlocking(string? selfEmail, string? otherEmail)
    {
        if (isSelf(selfEmail, otherEmail))
            return;
        Author authorToFollow = GetAuthorByEmail(otherEmail);
        Author authorSelf = GetAuthorByEmail(selfEmail);
        if (isBlocking(selfEmail , otherEmail))
        {
            authorSelf.Blocking.Remove(authorToFollow);
        }
        else
        {
            authorSelf.Blocking.Add(authorToFollow);
            if (isFollowing(selfEmail, otherEmail))
            {
                ToggleFollow(selfEmail,otherEmail);
            }
        }
        _service.SaveChanges();
    }

    public bool isBlocking(string? selfEmail, string? otherEmail)
    {
        var query = (from author in _service.Authors
                where author.Email == selfEmail
                from block in author.Blocking
                where block.Email == otherEmail
                select block
            );
        var result = query.ToList();
        return result.Count > 0;
    }
    public List<AuthorDTO> GetBlocking(string? selfEmail)
    {
        List<AuthorDTO> blocking = new List<AuthorDTO>();
        var query = (from author in _service.Authors
            from block in author.Blocking
            where author.Email == selfEmail
            orderby block.Name
            select new { block.Name, block.Email });
        var result = query.ToList();
        foreach (var block in result)
        {
            var author = new AuthorDTO();
            author.Name = block.Name;
            author.Email = block.Email;
            blocking.Add(author);
        }
        return blocking;
    }
    
    
}