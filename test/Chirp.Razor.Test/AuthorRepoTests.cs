namespace Chirp.Razor.Test;

public class AuthorRepoTests
{
    /*
    [Theory]
    [InlineData("anon", "anon@gmail.com")]
    public async void Name(string name, string email)
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
    }
    */
    
    [Theory]
    [InlineData("anon", "anon@gmail.com")]
    public async void TestGetAuthorDtoByName(string name , string email)
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
        var author = authorRepository.GetAuthorDtoByName(name);
        Assert.Equal(name, author.Name);
        Assert.Equal(email, author.Email);

    }

    [Theory]
    [InlineData("anon", "anon@gmail.com")]
    public async void TestGetAuthorDtoByEmail(string name, string email)
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
        var author = authorRepository.GetAuthorDtoByEmail(email);
        Assert.Equal(name, author.Name);
        Assert.Equal(email, author.Email);
    }
    
    [Theory]
    [InlineData("anon", "anon@gmail.com")]
    public async void DeleteAuthorTest(string name, string email)
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
        Assert.Equal(name,context.Authors.First().Name);
        Assert.Equal(email,context.Authors.First().Email);
        //deleting a author is anonymizing them so we check if there name and email Starts with [DELETED
        authorRepository.DeleteAuthor(email,null,null);
        Assert.StartsWith("[DELETED",context.Authors.First().Name);
        Assert.StartsWith("[DELETED",context.Authors.First().Email);
        
    }
    
    [Theory]
    [InlineData("anon", "anon@gmail.com", "anon2", "anon2@gmail.com")]
    public async void TestFollow(string name, string email, string name2, string email2)
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
        
        Assert.Empty(context.Authors.First().Following);
        authorRepository.ToggleFollow(context.Authors.First().Email,a2.Email);
        Assert.NotEmpty(context.Authors.First().Following);
        Assert.Equal(context.Authors.First().Following.First(), a2);
    }
    
    [Theory]
    [InlineData("anon", "anon@gmail.com", "anon2", "anon2@gmail.com")]
    public async void TestIsFollowing(string name, string email, string name2, string email2)
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
        
        Assert.False( authorRepository.isFollowing(a1.Email, a2.Email));
        authorRepository.ToggleFollow(context.Authors.First().Email,a2.Email);
        Assert.True( authorRepository.isFollowing(a1.Email, a2.Email));
    }
    
    [Theory]
    [InlineData("anon", "anon@gmail.com", "anon2", "anon2@gmail.com")]
    public async void TestGetFollowing(string name, string email, string name2, string email2)
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
        
        Assert.Empty(authorRepository.GetFollowing(a1.Email));
        authorRepository.ToggleFollow(context.Authors.First().Email,a2.Email);
        var list = authorRepository.GetFollowing(a1.Email);
        Assert.NotEmpty(list);
        Assert.Equal(list.First().Name, a2.Name);
        Assert.Equal(list.First().Email, a2.Email);
    }

    [Theory]
    [InlineData("anon", "anon@gmail.com", "anon2", "anon2@gmail.com")]
    public async void TestIsself(string name, string email, string name2, string email2)
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
            Id = "611e3fa1-be3b-413d-b7d7-333738c17a3a", Name = name, UserName = email, Email = email,
            Cheeps = new List<Cheep>(), Blocking = new List<Author>(), Following = new List<Author>()
        };
        var a2 = new Author()
        {
            Id = "611e3fa1-be3b-413d-b7d7-333738c17a3b", Name = name2, UserName = email2, Email = email2,
            Cheeps = new List<Cheep>(), Blocking = new List<Author>(), Following = new List<Author>()
        };

        context.Authors.Add(a1);
        context.Authors.Add(a2);
        context.SaveChanges();
        
        Assert.True(authorRepository.isSelf(a1.Email,a1.Email));
        Assert.False(authorRepository.isSelf(a1.Email,a2.Email));
    }

    [Theory]
    [InlineData("anon", "anon@gmail.com", "anon2", "anon2@gmail.com")]
    public async void TestUnfollow(string name, string email, string name2, string email2)
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
        
        Assert.Empty(context.Authors.First().Following);
        authorRepository.ToggleFollow(context.Authors.First().Email,a2.Email);
        Assert.NotEmpty(context.Authors.First().Following);
        Assert.Equal(context.Authors.First().Following.First(), a2);
        authorRepository.ToggleFollow(context.Authors.First().Email,a2.Email);
        Assert.Empty(context.Authors.First().Following);
    }
    
    [Theory]
    [InlineData("anon", "anon@gmail.com", "anon2", "anon2@gmail.com")]
    public async void Testblocking(string name, string email, string name2, string email2)
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
        
        Assert.Empty(context.Authors.First().Blocking);
        authorRepository.ToggleBlocking(context.Authors.First().Email,a2.Email);
        Assert.NotEmpty(context.Authors.First().Blocking);
        Assert.Equal(context.Authors.First().Blocking.First(), a2);
    }
    
    [Theory]
    [InlineData("anon", "anon@gmail.com", "anon2", "anon2@gmail.com")]
    public async void TestUnblocking(string name, string email, string name2, string email2)
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
        
        Assert.Empty(context.Authors.First().Blocking);
        authorRepository.ToggleBlocking(context.Authors.First().Email,a2.Email);
        Assert.NotEmpty(context.Authors.First().Blocking);
        Assert.Equal(context.Authors.First().Blocking.First(), a2);
        authorRepository.ToggleBlocking(context.Authors.First().Email,a2.Email);
        Assert.Empty(context.Authors.First().Blocking);
    }
    
    [Theory]
    [InlineData("anon", "anon@gmail.com", "anon2", "anon2@gmail.com")]
    public async void TestIsBlocking(string name, string email, string name2, string email2)
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
        
        Assert.False( authorRepository.isBlocking(a1.Email, a2.Email));
        authorRepository.ToggleBlocking(context.Authors.First().Email,a2.Email);
        Assert.True( authorRepository.isBlocking(a1.Email, a2.Email));
    }
    
    [Theory]
    [InlineData("anon", "anon@gmail.com", "anon2", "anon2@gmail.com")]
    public async void TestGetBlocking(string name, string email, string name2, string email2)
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
        
        Assert.Empty(authorRepository.GetBlocking(a1.Email));
        authorRepository.ToggleBlocking(context.Authors.First().Email,a2.Email);
        var list = authorRepository.GetBlocking(a1.Email);
        Assert.NotEmpty(list);
        Assert.Equal(list.First().Name, a2.Name);
        Assert.Equal(list.First().Email, a2.Email);
    }
    
    [Theory]
    [InlineData("anon", "anon@gmail.com")]
    public async void TestSave(string name, string email)
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
        Assert.Empty(context.Authors);
        authorRepository.save();
        Assert.NotEmpty(context.Authors);
    }
    
    [Theory]
    [InlineData("anon", "anon@gmail.com")]
    public async void TestOtherPersonAuthorCheep(string name, string email)
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

        CheepDTO cheep = new CheepDTO
        {
            Author = a1.Name,
            Text = "Hello, World!",
            Timestamp = DateTime.Now.ToString(),
            Email = a1.Email
        };
        
        cheepRepository.CreateCheep(cheep);
        Assert.Equal(1, context.Cheeps.Count());
        Assert.Equal(1, context.Authors.First().Cheeps.Count());
        List<CheepDTO> result = authorRepository.AuthorCheep(0,email, null);        
        Assert.Equal(cheep.Author, result.First().Author);
        Assert.Equal(cheep.Text, result.First().Text);
        Assert.Equal(cheep.Timestamp, result.First().Timestamp);
        Assert.Equal(cheep.Email, result.First().Email);
    }
    
    [Theory]
    [InlineData("anon", "anon@gmail.com")]
    public async void TestOwnTimelineAuthorCheep(string name, string email)
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

        CheepDTO cheep = new CheepDTO
        {
            Author = a1.Name,
            Text = "Hello, World!",
            Timestamp = DateTime.Now.ToString(),
            Email = a1.Email
        };
        
        cheepRepository.CreateCheep(cheep);
        Assert.Equal(1, context.Cheeps.Count());
        Assert.Equal(1, context.Authors.First().Cheeps.Count());
        List<CheepDTO> result = authorRepository.AuthorCheep(0,email, email);        
        Assert.Equal(cheep.Author, result.First().Author);
        Assert.Equal(cheep.Text, result.First().Text);
        Assert.Equal(cheep.Timestamp, result.First().Timestamp);
        Assert.Equal(cheep.Email, result.First().Email);
    }
    
    [Theory]
    [InlineData("anon", "anon@gmail.com", "anon2", "anon2@gmail.com")]
    public async void TestOwnTimelineFollowingAuthorCheep(string name, string email, string name2 , string email2)
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
        Assert.Equal(1, authorRepository.AuthorCheep(0,email2, null).Count);
        authorRepository.ToggleFollow(a1.Email,a2.Email);
        List<CheepDTO> result = authorRepository.AuthorCheep(0,email, email);        
        Assert.Equal(cheep.Author, result.First().Author);
        Assert.Equal(cheep.Text, result.First().Text);
        Assert.Equal(cheep.Timestamp, result.First().Timestamp);
        Assert.Equal(cheep.Email, result.First().Email);
    }
    
    [Theory]
    [InlineData("anon", "anon@gmail.com", "anon2", "anon2@gmail.com")]
    public async void TestblockFollow(string name, string email, string name2, string email2)
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
        
        Assert.Empty(context.Authors.First().Blocking);
        authorRepository.ToggleBlocking(context.Authors.First().Email,a2.Email);
        Assert.NotEmpty(context.Authors.First().Blocking);
        Assert.Equal(context.Authors.First().Blocking.First(), a2);
        Assert.Empty(context.Authors.First().Following);
        authorRepository.ToggleFollow(context.Authors.First().Email,a2.Email);
        Assert.NotEmpty(context.Authors.First().Following);
        Assert.Empty(context.Authors.First().Blocking);
        Assert.Equal(context.Authors.First().Following.First(), a2);
        authorRepository.ToggleBlocking(context.Authors.First().Email,a2.Email);
        Assert.NotEmpty(context.Authors.First().Blocking);
        Assert.Empty(context.Authors.First().Following);
        Assert.Equal(context.Authors.First().Blocking.First(), a2);
    }
    
}