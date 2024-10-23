namespace Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using System.Data;

public record CheepViewModel(string Author, string Message, string Timestamp);
public class DBFacade
{
    private string path ;
    
    public DBFacade(string path)
    {
        this.path = path;
    }

    public SqliteConnection EstablishConnection()
    {
        return new SqliteConnection($"Data Source={path}");
    }
    
    public List<CheepViewModel> Query(string query, int page, string? author = null) {
        List<CheepViewModel> cheeps = new List<CheepViewModel>();
        using (var connection = EstablishConnection()){
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@page", page);
            if (!string.IsNullOrEmpty(author))
            {
                command.Parameters.AddWithValue("@username", author);
            }
            using var reader = command.ExecuteReader();
            while (reader.Read()){
                var dataRecord = (IDataRecord)reader;
                cheeps.Add(
                    new CheepViewModel(
                        dataRecord[0].ToString(),
                        dataRecord[1].ToString(),
                        UnixTimeStampToDateTimeString(Double.Parse(dataRecord[2].ToString()))
                        )
                    );
            }
        }
        return cheeps;
    }
    public static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}