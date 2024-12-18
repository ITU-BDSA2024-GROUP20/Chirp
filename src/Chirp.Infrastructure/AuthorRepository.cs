using Microsoft.AspNetCore.Identity;

namespace Chirp.Infrastructure;
using Core;

public class AuthorRepository : IAuthorRepository
{
    private readonly ChirpDBContext _service;

    public AuthorRepository(ChirpDBContext service)
    {
        this._service = service;
    }
    /// <summary>
    ///     gets the cheeps posted by the <paramref name="user"/> and if <paramref name="user"/>=<paramref name="selfemail"/>
    ///     it also gets the cheep of those they follow
    /// </summary>
    /// <param name="page">page to get cheeps from</param> 
    /// <param name="user">Email or username of the users cheeps you want to see</param>
    /// <param name="selfemail"> email of logged in person </param>
    /// <returns></returns>
    public List<CheepDTO> AuthorCheep(int page, string? user, string? selfemail)
    {
        //gets the email if username provided
        if (user != null && !(user.Contains("@")))
        {
            user = GetAuthorDtoByName(user).Email;
        }
        //query to get cheep from the user
        List<CheepDTO> cheeps = new List<CheepDTO>();
        var query = (from author in _service.Authors
                from message in author.Cheeps
                where author.Email == user
                orderby message.TimeStamp descending
                select new { author.Name, message.Text, message.TimeStamp, author.Email }
            );
        
        if (selfemail != null && selfemail.Equals(user))
        {
            //query to include those the user follows union with orginal to get what needs to be displayed
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
        //converting the cheeps to a list of CheepDTO's
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
    /// commits changes to the repo
    /// </summary>
    public void save()
    {
        _service.SaveChanges();
    }
    
    
    /// <summary>
    /// While this function may imply deletion of said user, due to the current implementation of the userId system, should not be fully removed from the database.
    /// However, their username, name, and email will be anonymised and who they followed.
    /// </summary>
    /// <param name="email">email of user to be anonymised</param>
    /// <param name="signInManager">a the signinManager the program uses</param>
    /// <param name="info">ExternalLoginInfo login info of user to be deleted</param>
    /// <returns>The anonymised Author for disconnection from github</returns>
    public async void DeleteAuthor(string? email, SignInManager<Author>? signInManager, ExternalLoginInfo? info)
    {
        //finds the Author
        Author author = GetAuthorByEmail(email);
        
        var temp = GenerateRandNum();
        //anonymised the user
        author.Name = "[DELETED"+temp+"]";
        author.Email = "[DELETED"+temp+"]";
        author.UserName = "[DELETED]";
        author.PasswordHash = "[DELETED]";
        author.SecurityStamp = "[DELETED]";
        author.ConcurrencyStamp = "[DELETED]";
        author.NormalizedEmail = "[DELETED " + author.Id + "]";
        author.NormalizedUserName = "[DELETED " + author.Id + "]";
        author.Following.Clear();
        author.Blocking.Clear();
        if (signInManager != null && info != null)
        {
            await signInManager.UserManager.RemoveLoginAsync(author,
                info.LoginProvider, info.ProviderKey);
        }
        _service.SaveChanges();
    }
    /// <summary>
    /// generates a random sting of 9 digits
    /// </summary>
    /// <returns>sting of 9 digits</returns>
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
    
    /// <summary>
    /// find an <c>AuthorDTO</c> from the authors Username
    /// </summary>
    /// <param name="name">Username of the Author</param>
    /// <returns>AuthorDTO</returns>
    public AuthorDTO GetAuthorDtoByName(string? name)
    {
        //finds Author
        var query = (
            from author in _service.Authors
            where author.Name == name
            select new {author.Name, author.Email}
        );
        AuthorDTO authordto = new AuthorDTO();
        var result = query.ToList();
        //makes AuthorDTO
        foreach (var author in result)
        {
            authordto.Name = author.Name;
            authordto.Email = author.Email;
        }

        return authordto;
    }
    /// <summary>
    /// find an <c>AuthorDTO</c> from the authors email
    /// </summary>
    /// <param name="email">Email of the Author</param>
    /// <returns><c>AuthorDTO</c></returns>
    public AuthorDTO GetAuthorDtoByEmail(string? email)
    {
        //finds the author
        var query = (
            from author in _service.Authors
            where author.Email == email
            select new {author.Name, author.Email}
        );
        AuthorDTO authordto = new AuthorDTO();
        var result = query.ToList();
        //makes AuthorDTO
        foreach (var author in result)
        {
            authordto.Name = author.Name;
            authordto.Email = author.Email;
        }

        return authordto;
    }

    /// <summary>
    /// Gets the <c>Author</c> by Username
    /// </summary>
    /// <param name="name">string with username of user</param>
    /// <returns><c>Author</c></returns>
    private Author GetAuthorByName(string name)
    {
        var query = (
            from author in _service.Authors
            where author.Name == name
            select author
            );
        return query.First();
    }
    /// <summary>
    /// Gets the <c>Author</c> by Email
    /// </summary>
    /// <param name="email">string with Email of user</param>
    /// <returns><c>Author</c></returns>
    private Author GetAuthorByEmail(string? email)
    {
        var query = (
            from author in _service.Authors
            where author.Email == email
            select author
        );
        return query.First();
    }
    /// <summary>
    /// Toggels <paramref name="selfEmail"/> following <paramref name="otherEmail"/> 
    /// </summary>
    /// <param name="selfEmail"> Email of user that want to toggle</param>
    /// <param name="otherEmail"> Email of user to be followed/unfollowed</param>
    public void ToggleFollow(string? selfEmail, string? otherEmail)
    {
        //checks if it is the same author
        if (isSelf(selfEmail, otherEmail))
            return;
        Author authorToFollow = GetAuthorByEmail(otherEmail);
        Author authorself = GetAuthorByEmail(selfEmail);
        
        //checks if already blocking and decides to unblock or block 
        if (isFollowing(selfEmail, otherEmail))
        {
            authorself.Following.Remove(authorToFollow);
        }
        else
        {
            authorself.Following.Add(authorToFollow);
            if (isBlocking(selfEmail, otherEmail))
            {
                ToggleBlocking(selfEmail,otherEmail);
            }
        }
        _service.Authors.Update(authorself);
        _service.SaveChanges();
    }
    
    /// <summary>
    /// checks if <paramref name="selfEmail"/> is following <paramref name="otherEmail"/>
    /// </summary>
    /// <param name="selfEmail">user to check if following</param>
    /// <param name="otherEmail">user who may be followed</param>
    /// <returns>True if following False if not</returns>
    public bool isFollowing(string? selfEmail, string? otherEmail)
    {
        //query that find otherEmail in selfEmail's Following list
        var query = (from author in _service.Authors
                where author.Email == selfEmail
                from follow in author.Following
                where follow.Email == otherEmail
                select follow
            );
        var result = query.ToList();
        return result.Count > 0;
    }
    /// <summary>
    /// checks if <paramref name="selfEmail"/> and <paramref name="otherEmail"/> are the same <c>Author</c>
    /// </summary>
    /// <param name="selfEmail">first email</param>
    /// <param name="otherEmail">second email</param>
    /// <returns>True if same <c>Author</c> False if not </returns>
    public bool isSelf(string? selfEmail, string? otherEmail)
    {
        Author authorToFollow = GetAuthorByEmail(otherEmail);
        Author authorSelf = GetAuthorByEmail(selfEmail);
        if (authorToFollow.Equals(authorSelf))
            return true;
        return false;
    }
    /// <summary>
    /// Gets <c>AuthorDTO</c> representing all the users which <paramref name="selfEmail"/> if following
    /// </summary>
    /// <param name="selfEmail"> Email of user</param>
    /// <returns>List of <c>AuthorDTO</c> <paramref name="selfEmail"/> if following</returns>
    public List<AuthorDTO> GetFollowing(string? selfEmail)
    {
        List<AuthorDTO> following = new List<AuthorDTO>();
        //query that gets all author followed by selfEmail
        var query = (from author in _service.Authors
            from follow in author.Following
            where author.Email == selfEmail
            orderby follow.Name
            select new { follow.Name, follow.Email });
        var result = query.ToList();
        //makes result into a list of AuthorDTO's
        foreach (var follow in result)
        {
            var author = new AuthorDTO();
            author.Name = follow.Name;
            author.Email = follow.Email;
            following.Add(author);
        }
        return following;
    }
    /// <summary>
    /// Toggels <paramref name="selfEmail"/> blocking <paramref name="otherEmail"/> 
    /// </summary>
    /// <param name="selfEmail"> Email of user that want to toggle</param>
    /// <param name="otherEmail"> Email of user to be Blocked/Unblocke</param>
    public void ToggleBlocking(string? selfEmail, string? otherEmail)
    {
        //checks if it is the same author
        if (isSelf(selfEmail, otherEmail))
            return;
        Author authorToFollow = GetAuthorByEmail(otherEmail);
        Author authorSelf = GetAuthorByEmail(selfEmail);
        //checks if already blocking and decides to unblock or block 
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
    /// <summary>
    /// checks if <paramref name="selfEmail"/> is Blocking <paramref name="otherEmail"/>
    /// </summary>
    /// <param name="selfEmail">user to check if Blocking</param>
    /// <param name="otherEmail">user who may be blocked</param>
    /// <returns>True if blocking False if not</returns>
    public bool isBlocking(string? selfEmail, string? otherEmail)
    {
        //query that find otherEmail in selfEmail's blocking list
        var query = (from author in _service.Authors
                where author.Email == selfEmail
                from block in author.Blocking
                where block.Email == otherEmail
                select block
            );
        var result = query.ToList();
        return result.Count > 0;
    }
    /// <summary>
    /// Gets <c>AuthorDTO</c> representing all the users which <paramref name="selfEmail"/> if Blocking
    /// </summary>
    /// <param name="selfEmail"> Email of user</param>
    /// <returns>List of <c>AuthorDTO</c> <paramref name="selfEmail"/> if Blocking</returns>
    public List<AuthorDTO> GetBlocking(string? selfEmail)
    {
        List<AuthorDTO> blocking = new List<AuthorDTO>();
        //query that gets all author blocked by selfEmail
        var query = (from author in _service.Authors
            from block in author.Blocking
            where author.Email == selfEmail
            orderby block.Name
            select new { block.Name, block.Email });
        var result = query.ToList();
        //makes result into a list of AuthorDTO's
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