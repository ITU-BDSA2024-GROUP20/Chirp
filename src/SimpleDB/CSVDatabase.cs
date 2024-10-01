namespace SimpleDB;

using CsvHelper;
using System.Globalization;
using System.IO;
public class CSVDatabase<T> : IDatabaseRepository<T>
{

    private static CSVDatabase<T> instance;
    string path = AppDomain.CurrentDomain.BaseDirectory; 
    string filename;
    private CSVDatabase(){
        if(File.Exists(path+@"../../../../../src/CSVDBService/chirp_cli_db.csv")){
            filename = path+@"../../../../../src/CSVDBService/chirp_cli_db.csv";
        }else if(File.Exists("chirp_cli_db.csv")){
            filename = "chirp_cli_db.csv";
        }else{
            File.Create("chirp_cli_db.csv");
            filename = "chirp_cli_db.csv";
        }
    }

    public static CSVDatabase<T> getInstance(){
        if (instance == null)
        {
            instance = new CSVDatabase<T>();
        }
        return instance;
    }

    public IEnumerable<T> Read(int? limit = null)
    {
        
        IEnumerable<T> records;
        using StreamReader reader = new StreamReader(filename);
        using var csv = new CsvReader(reader,
            CultureInfo.InvariantCulture);
        {
            records = csv.GetRecords<T>()
            .ToList();
        }
        if (limit.HasValue)
        {
            return records.TakeLast(limit.Value);
        }
        else
        {
            return records;
        }
    }

    public void Store(T record)
    {
        using StreamWriter writer = new StreamWriter(filename,
            true);
        using (var csv = new CsvWriter(writer,
                   CultureInfo.InvariantCulture))
        {
            csv.NextRecord();
            csv.WriteRecord(record);
        }
    }
}