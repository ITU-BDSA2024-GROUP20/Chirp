namespace Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using System.Data;

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
                        CheepService.UnixTimeStampToDateTimeString(Double.Parse(dataRecord[2].ToString()))
                        )
                    );
            }
        }
        return cheeps;
    }
}