using Chirp.Core;


namespace Chirp.Infrastructure;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext _service;
    public CheepRepository(ChirpDBContext service)
    {
        this._service = service;
    }
    /// <summary>
    /// check if the string is too long
    /// </summary>
    /// <param name="str">the string to be checked</param>
    /// <returns>True if valid False if not</returns>
    public static bool ValidateString(string? str)
    {
        if (str == null)
            return false;
        if (str.Length <= 160 && str.Length > 0)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// Creates a new cheep form the given information
    /// </summary>
    /// <param name="newCheep"><c>CheepDTO</c> with the cheep's text and email of author</param>
    public void CreateCheep(CheepDTO newCheep)
    {
        //first checking if the text is valid
        if (ValidateString(newCheep.Text))
        {
            //finding the author of the cheep
            var query = (
                from aut in _service.Authors
                where aut.Email == newCheep.Email
                select aut);
            var author = query.First();
            
           //makes the cheep
            Cheep cheep = new Cheep
            {
                CheepId = _service.Cheeps.Count() + 1,
                AuthorId = author.Id,
                Author = author,
                Text = newCheep.Text,
                TimeStamp = DateTime.Parse(newCheep.Timestamp!)
            };
            //add cheep to the Author and list of cheeps
            author.Cheeps.Add(cheep);
            _service.Cheeps.Add(cheep);
            //saves the changes
            _service.Authors.Update(author);
            _service.SaveChanges();
        }
    }
    
    
    
    /// <summary>
    /// Gets the cheeps of all authors if user is not logged in.
    /// If logged in get all cheeps exept those writen by blocked users 
    /// </summary>
    /// <param name="page">current page user is on</param>
    /// <param name="userName">must be null</param>
    /// <param name="selfEmail">email of logged in user</param>
    /// <returns></returns>
    public List<CheepDTO> ReadCheep(int page, string? userName = null, string? selfEmail = null)
    {
        List<CheepDTO> cheeps = new List<CheepDTO>(); 
        //query that finds all cheeps and pust them in a decending order
        var query = (from message in _service.Cheeps 
            join author in _service.Authors on message.AuthorId equals author.Id 
            orderby message.TimeStamp descending 
            select new { author.Name, message.Text, message.TimeStamp, author.Email });
        
        if (selfEmail != null)
        {
            //query that finds all cheeps and pust them in a decending order exept those blocked buy the user
            query = (from message in _service.Cheeps 
                join author in _service.Authors on message.AuthorId equals author.Id 
                where  !(from aut in _service.Authors 
                        where aut.Email == selfEmail 
                        from blocks in aut.Blocking 
                        select  blocks.Id
                        ).Contains(message.AuthorId)
                orderby message.TimeStamp descending 
                select new { author.Name, message.Text, message.TimeStamp, author.Email });
        } 
        var result =  query.Skip(page).Take(32).ToList(); 
        //all cheeps are made into CheepDTO and put into a list to be returned
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

