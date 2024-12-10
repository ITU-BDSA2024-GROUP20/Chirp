global using Chirp.Core;
global using Chirp.Infrastructure;
global using Microsoft.Data.Sqlite;
global using Microsoft.EntityFrameworkCore;
global using Xunit.Abstractions;

namespace Chirp.Razor.Test;

public class CheepRepotest
{
  
    

    [Theory]
    [InlineData("anon", "anon@gmail.com")]
    [InlineData("anon2", "anon2@gmail.com")]
    public async void TestCreateCheep(string name, string email)
    {
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
        
        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        
        ICheepRepository cheepRepository = new CheepRepository(context);
        IAuthorRepository authorRepository = new AuthorRepository(context);
        var a1 = new Author()
        {
            Id = "611e3fa1-be3b-413d-b7d7-333738c17a3a",Name = name,UserName = email, Email = email,
            Cheeps = new List<Cheep>(), Blocking = new List<Author>(), Following = new List<Author>()
        };
        
        context.Authors.Add(a1);
        context.SaveChanges();

        Assert.Empty(context.Cheeps);
        Assert.Empty(context.Authors.First().Cheeps);
        cheepRepository.CreateCheep(new CheepDTO
        {
            Author = a1.Name,
            Text = "Hello, World!",
            Timestamp = DateTime.Now.ToString(),
            Email = a1.Email
            
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
        IAuthorRepository authorRepository = new AuthorRepository(context);

        var a1 = new Author()
        {
            Id = "611e3fa1-be3b-413d-b7d7-333738c17a3a",Name = "anon",UserName = "anon@gmail.com", Email = "anon@gmail.com", 
            Cheeps = new List<Cheep>(), Blocking = new List<Author>(), Following = new List<Author>()
        };
        
        context.Authors.Add(a1);
        context.SaveChanges();
        
        var cheep = new CheepDTO
        {
            Author = a1.Name,
            Text = "Hello, World!",
            Timestamp = DateTime.Now.ToString(),
            Email = a1.Email

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

    [Theory]
    [InlineData("anon", "anon@gmail.com")]
    public async void TestReadCheepWithAuthor(string name , string email)
    {
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
        
        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        
        ICheepRepository cheepRepository = new CheepRepository(context);
        IAuthorRepository authorRepository = new AuthorRepository(context);

        var a1 = new Author()
        {
            Id = "611e3fa1-be3b-413d-b7d7-333738c17a3a",Name = name,UserName = email, Email = email, 
            Cheeps = new List<Cheep>(), Blocking = new List<Author>(), Following = new List<Author>()
        };
        
        context.Authors.Add(a1);
        context.SaveChanges();
        
        var cheep = new CheepDTO
        {
            Author = a1.Name,
            Text = "Hello, World!",
            Timestamp = DateTime.Now.ToString(),
            Email = a1.Email

        };
        cheepRepository.CreateCheep(cheep);
        Assert.Equal(1, context.Cheeps.Count());
        Assert.Equal(1, context.Authors.Count());
        List<CheepDTO> result = cheepRepository.ReadCheep(0,null, email);        
        Assert.Equal(cheep.Author, result.First().Author);
        Assert.Equal(cheep.Text, result.First().Text);
        Assert.Equal(cheep.Timestamp, result.First().Timestamp);
        Assert.Equal(cheep.Email, result.First().Email);
    }
    
    [Theory]
    [InlineData("anon", "anon@gmail.com", "anon2", "anon2@gmail.com")]
    public async void TestReadCheepBlocking(string name, string email, string name2, string email2)
    {
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
        
        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        
        ICheepRepository cheepRepository = new CheepRepository(context);
        IAuthorRepository authorRepository = new AuthorRepository(context);

        var a1 = new Author()
        {
            Id = "611e3fa1-be3b-413d-b7d7-333738c17a3a",Name = name,UserName = email, Email = email, 
            Cheeps = new List<Cheep>(), Blocking = new List<Author>(), Following = new List<Author>()
        };
        var a2 = new Author()
        {
            Id = "611e3fa1-be3b-413d-b7d7-333738c17a3b",Name = name2,UserName = email2, Email = email2, 
            Cheeps = new List<Cheep>(), Blocking = new List<Author>(), Following = new List<Author>()
        };
        
        context.Authors.Add(a1);
        context.Authors.Add(a2);
        context.SaveChanges();
        
        CheepDTO cheep = new CheepDTO
        {
            Author = a2.Name,
            Text = "Hello, World!",
            Timestamp = DateTime.Now.ToString(),
            Email = a2.Email
        };
        
        cheepRepository.CreateCheep(cheep);
        Assert.Equal(1, context.Cheeps.Count());
        Assert.Empty(context.Authors.First().Cheeps);
        Assert.Empty(context.Authors.First().Blocking);
        List<CheepDTO> result = cheepRepository.ReadCheep(0,null, email);
        Assert.Single(result);
        authorRepository.ToggleBlocking(context.Authors.First().Email,a2.Email);
        Assert.NotEmpty(context.Authors.First().Blocking);
        result = cheepRepository.ReadCheep(0,null, email);  
        Assert.Empty(result);
    }
    
}