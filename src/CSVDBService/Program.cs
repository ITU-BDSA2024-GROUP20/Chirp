using CSVDBService;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

IDatabaseRepository<Cheep> database = CSVDatabase<Cheep>.getInstance();


app.MapGet("/cheeps", () => database.Read());
app.MapGet("/cheeps", (int limit) => database.Read(limit));
app.MapPost("/cheep", (Cheep cheep) => { database.Store(cheep);});

app.Run();



public record Cheep(string Author, string Message, long Timestamp);
