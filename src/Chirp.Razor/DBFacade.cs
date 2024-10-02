namespace Chirp.Razor;
using Microsoft.Data.Sqlite;
using System.Data;

public class DBFacade
{
    string path = "chirp.db";
    
    public DBFacade(string path)
    {
        this.path = path;
    }

    public SqliteConnection EstablishConnection()
    {
        return new SqliteConnection($"Data Source={path}");
    }
    
    public List<CheepViewModel> Query(string query, string? author = null){
        List<CheepViewModel> cheeps = new List<CheepViewModel>();
        using (var connection = EstablishConnection()){
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = query;
            using var reader = command.ExecuteReader();
            while (reader.Read()){
                var dataRecord = (IDataRecord)reader;
                cheeps.Add(
                    new CheepViewModel(
                        dataRecord[0].ToString(),
                        dataRecord[1].ToString(),
                        dataRecord[2].ToString()
                        )
                    );
            }
        }
        return cheeps;
    }
}