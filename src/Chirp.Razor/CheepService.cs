using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor;
using System.Data;
using Microsoft.Data.Sqlite;
public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps(int page);
    public List<CheepViewModel> GetCheepsFromAuthor(string author , int page);
}

public class CheepService : ICheepService
{
    private readonly DBFacade facade;
    
    public CheepService(DBFacade facade)
    {
        this.facade = facade;
    }
	
    public List<CheepViewModel> GetCheeps(int page)
    {
        string query = 
            @"SELECT u.username, m.text, m.pub_date
            FROM message m
            JOIN user u ON m.author_id = u.user_id
            ORDER by m.pub_date desc
            LIMIT 32
            OFFSET @page";
        return facade.Query(query, page);
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page)
    {
        string query = 
            @"SELECT u.username, m.text, m.pub_date
            FROM message m
            JOIN user u ON m.author_id = u.user_id
            WHERE u.username = @username
            ORDER by m.pub_date desc
            LIMIT 32
            OFFSET @page";
        // filter by the provided author name
        return facade.Query(query, page ,author);
    }

    public static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}

public class Message
{
    public int MessageId { get; set; }
    public int UserId { get; set; }
    public string Text { get; set; }
    public User User { get; set; }
}

public class User
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public ICollection<Message> Messages { get; set; }
    public string Email { get; set; }
}

public class CSDBService : DbContext
{
    public DbSet<Message> Messages { get; set; }
    public DbSet<User> Users { get; set; }
    
    public CSDBService(DbContextOptions<CSDBService> options) : base(options) { }
}

// Constructed for later usage, for when we find out what MessageDTO actually is
public interface IMessageRepository
{
    public Task CreateMessage(MessageDTO newMessage);
    public Task<List<MessageDTO>> ReadMessage(string userName);
    public Task UpdateMessage(MessageDTO alteredMessage);
}

public class MessageRepository : IMessageRepository
{
    public Task CreateMessage(MessageDTO newMessage)
    {
        throw new NotImplementedException();
    }

    public Task<List<MessageDTO>> ReadMessage(string userName)
    {
        throw new NotImplementedException();
    }

    public Task UpdateMessage(MessageDTO alteredMessage)
    {
        throw new NotImplementedException();
    }
}

public class MessageDTO
{
    
}