using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepDTO> GetCheeps(int page);
    public List<CheepDTO> GetCheepsFromAuthor(string author , int page);
}

public class CheepService : ICheepService
{
    //private readonly DBFacade facade;
    private readonly ICheepRepository repository;
    
    public CheepService( ICheepRepository repository)
    {
        //this.facade = facade;
        this.repository = repository;
    }
	
    public List<CheepDTO> GetCheeps(int page)
    {
        return repository.ReadCheep(page, null);
    }

    public List<CheepDTO> GetCheepsFromAuthor(string author, int page)
    {
        // filter by the provided author name
        return repository.ReadCheep( page ,author);
    }

    public static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}