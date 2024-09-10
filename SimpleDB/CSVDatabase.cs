﻿namespace SimpleDB;
using CsvHelper;
using System.Globalization;

public class CSVDatabase<T> : IDatabaseRepository<T>
{
    public IEnumerable<T> Read(int? limit = null)
    {
        using StreamReader reader = new StreamReader(@"..\SimpleDB\chirp_cli_db.csv");
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);{
            return csv.GetRecords<T>().ToList<T>();
        }
    }

    public void Store(T record)
    {
        using StreamWriter writer = new StreamWriter(@"..\SimpleDB\chirp_cli_db.csv", true);
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)){
            writer.WriteLine();
            csv.WriteRecord(record);
        }
    }
}