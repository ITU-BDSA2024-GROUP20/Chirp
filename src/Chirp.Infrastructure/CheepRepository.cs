using Chirp.Core;


namespace Chirp.Infrastructure;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext _service;
    public CheepRepository(ChirpDBContext service)
    {
        this._service = service;
    }

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
    
    public void CreateCheep(CheepDTO newCheep)
    {
        if (ValidateString(newCheep.Text))
        {
            var query = (
                from aut in _service.Authors
                where aut.Email == newCheep.Email
                select aut);
            var author = query.First();
            
           
            Cheep cheep = new Cheep
            {
                CheepId = _service.Cheeps.Count() + 1,
                AuthorId = author.Id,
                Author = author,
                Text = newCheep.Text,
                TimeStamp = DateTime.Parse(newCheep.Timestamp!)
            };

            author.Cheeps.Add(cheep);

            _service.Cheeps.Add(cheep);
            _service.Authors.Update(author);
            _service.SaveChanges();
        }
    }
    
    
    
    
    public List<CheepDTO> ReadCheep(int page, string? userName = null, string? selfEmail = null)
    {
        List<CheepDTO> cheeps = new List<CheepDTO>();
        
            var query = (from message in _service.Cheeps
                join author in _service.Authors on message.AuthorId equals author.Id
                orderby message.TimeStamp descending 
                select new { author.Name, message.Text, message.TimeStamp, author.Email });
            
            if (selfEmail != null)
            {
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

