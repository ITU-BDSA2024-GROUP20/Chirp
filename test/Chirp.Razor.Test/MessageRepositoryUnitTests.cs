using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Chirp.Razor.Test;

public class MessageRepositoryUnitTests
{
    
    
    [Fact]
    public async void TestCreateAuthor()
    {
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
        
        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        
        ICheepRepository cheepRepository = new CheepRepository(context);
        
        Assert.Equal(0, context.Authors.Count());
        cheepRepository.CreateAuthor("SifDJ", "esja@itu.dk");
        Assert.Equal(1, context.Authors.Count());
        Assert.Equal("SifDJ", context.Authors.First().Name);
        Assert.Equal("esja@itu.dk", context.Authors.First().Email);
    }

    [Fact]
    public async void TestCreateCheepWithAuthor()
    {
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
        
        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        
        ICheepRepository cheepRepository = new CheepRepository(context);
        
        cheepRepository.CreateAuthor("SifDJ", "esja@itu.dk");

        Assert.Empty(context.Cheeps);
        Assert.Empty(context.Authors.First().Cheeps);
        cheepRepository.CreateCheep(new CheepDTO
        {
            Author = "SifDJ",
            Text = "Hello, World!",
            Timestamp = DateTime.Now.ToString(),
            Email = "esja@itu.dk"
            
        });
        Assert.Equal(1, context.Authors.Count());
        Assert.Equal(1, context.Cheeps.Count());
        Assert.Equal("Hello, World!", context.Cheeps.First().Text);
        Assert.Equal(1, context.Cheeps.First().AuthorId);
        Assert.Equal("Hello, World!", context.Authors.First().Cheeps.First().Text);
    }


    [Fact]
    public async void TestCreateCheepWithoutAuthor()
    {
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
        
        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        
        ICheepRepository cheepRepository = new CheepRepository(context);
        

        Assert.Empty(context.Cheeps);
        Assert.Empty(context.Authors);
        cheepRepository.CreateCheep(new CheepDTO
        {
            Author = "SifDJ",
            Text = "Hello, World!",
            Timestamp = DateTime.Now.ToString(),
            Email = "esja@itu.dk"
            
        });
        Assert.Equal(1, context.Authors.Count());
        Assert.Equal("SifDJ", context.Authors.First().Name);
        Assert.Equal("esja@itu.dk", context.Authors.First().Email);
        Assert.Equal(1, context.Cheeps.Count());
        Assert.Equal("Hello, World!", context.Cheeps.First().Text);
        Assert.Equal(1, context.Cheeps.First().AuthorId);
        Assert.Equal("Hello, World!", context.Authors.First().Cheeps.First().Text);
    }


    [Fact]
    public async void TestReadCheep()
    {
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();

        ICheepRepository cheepRepository = new CheepRepository(context);

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
        List<CheepDTO> result = cheepRepository.ReadCheep(0,null);        
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
        List<CheepDTO> result = cheepRepository.ReadCheep(0,"SifDJ");        
        Assert.Equal(cheep.Author, result.First().Author);
        Assert.Equal(cheep.Text, result.First().Text);
        Assert.Equal(cheep.Timestamp, result.First().Timestamp);
        Assert.Equal(cheep.Email, result.First().Email);
    }

    [Fact]
    public async void TestGetAuthorByName()
    {
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();

        ICheepRepository cheepRepository = new CheepRepository(context);

        cheepRepository.CreateAuthor("SifDJ", "esja@itu.dk");
        var author = cheepRepository.GetAuthorByName("SifDJ");
        Assert.Equal("SifDJ", author.Name);
        Assert.Equal("esja@itu.dk", author.Email);

    }

    [Fact]
    public async void TestGetAuthorByEmail()
    {
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();

        ICheepRepository cheepRepository = new CheepRepository(context);

        cheepRepository.CreateAuthor("SifDJ", "esja@itu.dk");
        var author = cheepRepository.GetAuthorByEmail("esja@itu.dk");
        Assert.Equal("SifDJ", author.Name);
        Assert.Equal("esja@itu.dk", author.Email);
    }

}