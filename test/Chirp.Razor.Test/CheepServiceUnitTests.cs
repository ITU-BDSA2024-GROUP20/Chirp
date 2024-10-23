using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor.Test;

public class CheepServiceUnitTests
{
    [Fact]
    public async void TestReadCheepWithOutAuthor()
    {
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();

        ICheepRepository cheepRepository = new CheepRepository(context);
        ICheepService CheepService = new CheepService(cheepRepository);

        cheepRepository.CreateAuthor("SifDJ", "esja@itu.dk");
        var cheep = new CheepDTO
        {
            Author = "SifDJ",
            Text = "Hello, World!",
            Timestamp = DateTime.Now.ToString(),
            Email = "esja@itu.dk"

        };
        cheepRepository.CreateCheep(cheep);
        Assert.Equal(1, context.Cheeps.Count());
        Assert.Equal(1, context.Authors.Count());
        List<CheepDTO> result = CheepService.GetCheeps(0); 
        Assert.Equal(cheep.Author, result.First().Author);
        Assert.Equal(cheep.Text, result.First().Text);
        Assert.Equal(cheep.Timestamp, result.First().Timestamp);
        Assert.Equal(cheep.Email, result.First().Email);
    }
    
    [Fact]
    public async void TestReadCheepWithAuthor()
    {
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();

        ICheepRepository cheepRepository = new CheepRepository(context);
        ICheepService CheepService = new CheepService(cheepRepository);
        
        cheepRepository.CreateAuthor("SifDJ", "esja@itu.dk");
        var cheep = new CheepDTO
        {
            Author = "SifDJ",
            Text = "Hello, World!",
            Timestamp = DateTime.Now.ToString(),
            Email = "esja@itu.dk"

        };
        cheepRepository.CreateCheep(cheep);
        Assert.Equal(1, context.Cheeps.Count());
        Assert.Equal(1, context.Authors.Count());
        List<CheepDTO> result = CheepService.GetCheepsFromAuthor("SifDJ", 0);       
        Assert.Equal(cheep.Author, result.First().Author);
        Assert.Equal(cheep.Text, result.First().Text);
        Assert.Equal(cheep.Timestamp, result.First().Timestamp);
        Assert.Equal(cheep.Email, result.First().Email);
    }
    
    
    
}