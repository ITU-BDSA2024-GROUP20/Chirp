using System;
using System.IO;
using System.Net.Http.Json;
using Chirp.CLI;
using DocoptNet;


var baseURL = "http://localhost:5241";
using HttpClient client = new();
client.BaseAddress = new Uri(baseURL);

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
var arguments = new Docopt().Apply(usage,
    args,
    version: "1.0",
    exit: true);
//foreach(var (key,value) in arguments){
//    Console.WriteLine($"{key}: {value}");
//}
if (arguments != null)
{
    if (arguments["read"].IsTrue)
    {
        if (arguments["<limit>"].IsInt)
        {
            //UserInterface.read(await client.GetFromJsonAsync<IEnumerable<Cheep>>("/cheeps", Int32.Parse(arguments["<limit>"].ToString()));
        }
        else
        {
            UserInterface.read(await client.GetFromJsonAsync<IEnumerable<Cheep>>("/cheeps"));
        }
        
    }
    else if (arguments["cheep"].IsTrue)
    {
        Cheep cheep = new Cheep(Environment.UserName,
            arguments["<message>"]
                .ToString(),
            DateTimeOffset.Now.ToUnixTimeSeconds());
        await client.PostAsJsonAsync("/cheep", cheep );
    }
}
else
{
    Console.WriteLine("argument is null");
}

public record Cheep(
    string Author,
    string Message,
    long Timestamp);