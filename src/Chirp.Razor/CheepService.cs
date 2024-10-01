namespace Chirp.Razor;
using System.Data;
using Microsoft.Data.Sqlite;
public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    private readonly DBFacade facade;
    
    public CheepService(DBFacade facade){
        this.facade = facade
    }
	
    public List<CheepViewModel> GetCheeps()
    {
        string query = 
            @"SELECT u.username, m.text, m.pub_date
            FROM message m
            JOIN user u ON m.author_id = u.user_id
            ORDER by m.pub_date desc";
        return facade.query(query);
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        List<CheepViewModel> cheeps = new List<CheepViewModel>();
        var sqlDBFilePath = "chirp.db";
        var sqlQuery = @"SELECT u.username, m.text, m.pub_date FROM message m JOIN user u ON m.author_id = u.user_id ORDER by m.pub_date desc";
        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var dataRecord = (IDataRecord)reader;
                cheeps.Add(
                    new CheepViewModel(
                        dataRecord[0].ToString(),
                        dataRecord[1].ToString(), 
                        UnixTimeStampToDateTimeString(Double.Parse(dataRecord[2].ToString()))));
                       
            }
        }
        // filter by the provided author name
        return cheeps.Where(x => x.Author == author).ToList();
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
