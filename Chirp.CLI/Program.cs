using System;
using System.IO;
using Chirp.CLI;
using SimpleDB;

IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>();

if (args[0] == "read")
{
    UserInterface.read(database.Read());
} else if (args[0] == "cheep")
{
    var record = new Cheep(Environment.UserName,"\""+args[1]+"\"",DateTimeOffset.Now.ToUnixTimeSeconds());
    database.Store(record);
}

public record Cheep(string Author, string Message, long Timestamp);