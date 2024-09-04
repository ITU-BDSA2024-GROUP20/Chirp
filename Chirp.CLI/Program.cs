
using System;
using System.IO;
using CsvHelper;
using System.Globalization;

if (args[0] == "read")
{
    using StreamReader reader = new StreamReader(@"chirp_cli_db.csv");
    using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);{
        var records = csv.GetRecords<Cheep>();
        foreach (var record in records)
        {
            Console.WriteLine(returnCheep(record));
        }
    }
}else if (args[0] == "cheep")
{
    using StreamWriter writer = new StreamWriter(@"chirp_cli_db.csv", true);
    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)){
        var record = new Cheep(Environment.UserName,"\""+args[1]+"\"",DateTimeOffset.Now.ToUnixTimeSeconds());
        csv.WriteRecord(record);
    }
}

static string returnCheep(Cheep cheep){
    return cheep.Author+" @ "+returnTimeStamp(cheep.Timestamp)+": "+cheep.Message;
}

static string returnTimeStamp(Int64 Timestamp){
    var _timestamp = DateTimeOffset.FromUnixTimeSeconds(Timestamp).LocalDateTime.ToString("MM dd yy HH:mm:ss").Split(" ");
    return _timestamp[0]+"/"+_timestamp[1]+"/"+_timestamp[2]+" "+_timestamp[3];
   
}
public record Cheep(string Author, string Message, long Timestamp);