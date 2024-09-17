namespace SimpleDB.Test;
using SimpleDB;
public class UnitTest1
{
    IDatabaseRepository<Cheep> database = CSVDatabase<Cheep>.getInstance();
    [Fact]
    public void StoreTest()
    {
        var length = database.Read().Count();
        Console.WriteLine(length);
        var record = new Cheep("ropf", "Hello, BDSA students!", 1690891760);
        database.Store(record);
        Console.WriteLine(database.Read().Count());
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