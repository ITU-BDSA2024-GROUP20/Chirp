namespace SimpleDB;

using CsvHelper;
using System.Globalization;
using System.IO;
public class CSVDatabase<T> : IDatabaseRepository<T>
{

    private static CSVDatabase<T> instance;

    private CSVDatabase(){}

    public static CSVDatabase<T> getInstance(){
        if (instance == null)
        {
            instance = new CSVDatabase<T>();
        }
        return instance;
    }

    string path = AppDomain.CurrentDomain.BaseDirectory; 
    
    public IEnumerable<T> Read(int? limit = null)
    {
        IEnumerable<T> records;
        //Console.WriteLine(path);
        using StreamReader reader = new StreamReader(path+@"../../../../SimpleDB/chirp_cli_db.csv");
        using var csv = new CsvReader(reader,
            CultureInfo.InvariantCulture);
        {
            records = csv.GetRecords<T>()
                .ToList<T>();
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
        using StreamWriter writer = new StreamWriter(path +@"../../../../SimpleDB/chirp_cli_db.csv",
            true);
        using (var csv = new CsvWriter(writer,
                   CultureInfo.InvariantCulture))
        {
            csv.NextRecord();
            csv.WriteRecord(record);
        }
    }
}