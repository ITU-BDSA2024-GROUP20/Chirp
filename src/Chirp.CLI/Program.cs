using System;
using System.IO;
using Chirp.CLI;
using DocoptNet;
using SimpleDB;

IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>();

const string usage = @"Chirp CLI version.

Usage:
  chirp read
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
if(arguments != null){
  if(arguments["read"].IsTrue){
    if(arguments["<limit>"].IsInt){
      UserInterface.read(database.Read(int.Parse(arguments["<limit>"].ToString())));
    }else{
      UserInterface.read(database.Read());
    }
  }else if (arguments["cheep"].IsTrue){
      var record = new Cheep(Environment.UserName,arguments["<message>"].ToString(),DateTimeOffset.Now.ToUnixTimeSeconds());
      database.Store(record);
  }
}else{
  Console.WriteLine("argument is null");
}

public record Cheep(string Author, string Message, long Timestamp);