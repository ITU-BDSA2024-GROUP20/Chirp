using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Chirp.Razor.Test;

public class MessageRepositoryUnitTests
{
    [Fact]
    public async void TestAddAuthor()
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
    }
}