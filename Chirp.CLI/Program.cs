using System;
using System.IO;
using Chirp.CLI;
using DocoptNet;
using SimpleDB;

IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>();

const string usage = @"Chirp CLI version.

Usage:
  chirp read <limit>
  chirp cheep <message>
  chirp (-h | --help)
  chirp --version

Options:
  -h --help     Show this screen.
  --version     Show version.
";

var arguments = new Docopt().Apply(usage, args, version: "1.0", exit: true);
//foreach(var (key,value) in arguments){
//    Console.WriteLine($"{key}: {value}");
//}
if(arguments["read"].ToString().ToLower() == "true"){
    UserInterface.read(database.Read());
}else if (arguments["cheep"].ToString().ToLower() == "true"){
    var record = new Cheep(Environment.UserName,arguments["<message>"].ToString(),DateTimeOffset.Now.ToUnixTimeSeconds());
    database.Store(record);
}

public record Cheep(string Author, string Message, long Timestamp);