namespace Chirp.Razor;
using Microsoft.Data.Sqlite;

public class DBFacade
{
    string path = "chirp.db";
    
    public DBFacade(string path)
    {
        this.path = path;
    }

    public SqliteConnection establishConnection()
    {
        return new SqliteConnection($"Data Source={path}");
    }
    
    public List<CheepViewModel> query(string query){
        List<CheepViewModel> cheeps = new List<CheepViewModel>();
        using (var connection = new SqliteConnection(establishConnection()){
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
                        UnixTimeStampToDateTimeString(Double.Parse(dataRecord[2].ToString()))));
            }
        }
    }
}