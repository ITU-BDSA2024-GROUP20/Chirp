using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Chirp.Razor.Test;

public class MessageRepositoryUnitTests
{
    

    [Fact]
    public async void TestCreateCheepWithAuthor()
    {
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
        
        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        
        ICheepRepository cheepRepository = new CheepRepository(context);
        var a1 = new Author()
        {
            Id = "611e3fa1-be3b-413d-b7d7-333738c17a3a",Name = "SifDJ",UserName = "esja@itu.dk", Email = "esja@itu.dk", Cheeps = new List<Cheep>()
        };
        
        context.Authors.Add(a1);
        context.SaveChanges();

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
        Assert.Equal("611e3fa1-be3b-413d-b7d7-333738c17a3a", context.Cheeps.First().AuthorId);
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

        var a1 = new Author()
        {
            Id = "611e3fa1-be3b-413d-b7d7-333738c17a3a",Name = "SifDJ",UserName = "esja@itu.dk", Email = "esja@itu.dk", Cheeps = new List<Cheep>()
        };
        
        context.Authors.Add(a1);
        context.SaveChanges();
        
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
        List<CheepDTO> result = cheepRepository.ReadCheep(0,null, null);        
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

        var a1 = new Author()
        {
            Id = "611e3fa1-be3b-413d-b7d7-333738c17a3a",Name = "SifDJ",UserName = "esja@itu.dk", Email = "esja@itu.dk", Cheeps = new List<Cheep>()
        };
        
        context.Authors.Add(a1);
        context.SaveChanges();
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
        List<CheepDTO> result = cheepRepository.ReadCheep(0,"SifDJ", null);        
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

        var a1 = new Author()
        {
            Id = "611e3fa1-be3b-413d-b7d7-333738c17a3a",Name = "SifDJ",UserName = "esja@itu.dk", Email = "esja@itu.dk", Cheeps = new List<Cheep>()
        };
        
        context.Authors.Add(a1);
        context.SaveChanges();
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

        var a1 = new Author()
        {
            Id = "611e3fa1-be3b-413d-b7d7-333738c17a3a",Name = "SifDJ",UserName = "esja@itu.dk", Email = "esja@itu.dk", Cheeps = new List<Cheep>()
        };
        
        context.Authors.Add(a1);
        context.SaveChanges();
        var author = cheepRepository.GetAuthorByEmail("esja@itu.dk");
        Assert.Equal("SifDJ", author.Name);
        Assert.Equal("esja@itu.dk", author.Email);
    }

}