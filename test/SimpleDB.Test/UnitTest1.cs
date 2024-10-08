namespace SimpleDB.Test;
using SimpleDB;
using System.Net.Http.Json;

public class UnitTest1
{
    CSVDatabase<Cheep> database = CSVDatabase<Cheep>.getInstance();
    [Fact]
    public void StoreTest()
    {
        var length = database.Read().Count();
        var record = new Cheep("ropf", "Hello, BDSA students!", 1690891760);
        database.Store(record);
        Assert.Equal(length + 1, database.Read().Count());

    }

    [Fact]
    public void ReadTest()
    {
        var record = new Cheep("ropf", "Hello, BDSA students!", 1690891760);
        var read = database.Read();
        Assert.Equal(record, read.First());
    }

    
}

public record Cheep(
    string Author,
    string Message,
    long Timestamp);