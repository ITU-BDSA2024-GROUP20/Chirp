namespace SimpleDB;

using CsvHelper;
using System.Globalization;

public class CSVDatabase<T> : IDatabaseRepository<T>
{
    public IEnumerable<T> Read(int? limit = null)
    {
        IEnumerable<T> records;

        using StreamReader reader = new StreamReader(@"..\SimpleDB\chirp_cli_db.csv");
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
        using StreamWriter writer = new StreamWriter(@"..\SimpleDB\chirp_cli_db.csv",
            true);
        using (var csv = new CsvWriter(writer,
                   CultureInfo.InvariantCulture))
        {
            csv.NextRecord();
            csv.WriteRecord(record);
        }
    }
}