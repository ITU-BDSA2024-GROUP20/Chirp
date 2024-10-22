namespace test;
using Chirp.CLI;                   // Your ChirpContext and models
using Chirp.Repositories;          // Your MessageRepository or equivalent
using Microsoft.Data.Sqlite;       // For in-memory SQLite
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO;

public class UnitTest1
{
    [Fact]
    public async Task QueryMessagesForUser_InMemoryDatabaseTest()
    {
        // Arrange: Set up an in-memory SQLite database
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<ChirpContext>()
            .UseSqlite(connection)
            .Options;

        using var context = new ChirpContext(options);
        await context.Database.EnsureCreatedAsync(); // Apply schema

        await SeedTestData(context);

        IMessageRepository repository = new MessageRepository(context);

        var result = repository.QueryMessages("Jacqualine Gilcoine");

        Assert.NotNull(result);
        Assert.Equal(3, result.Count); 
        Assert.Equal("They were married in Chicago, with old Smith, and was expected aboard every day; meantime, the two went past me.", result[0].Text);
    }

    private static async Task SeedTestData(ChirpContext context)
    {
        // Insert test users
        await context.Database.ExecuteSqlRawAsync(@"
            INSERT INTO user (Id, Author, Email) VALUES
                (10, 'Jacqualine Gilcoine', 'Jacqualine.Gilcoine@gmail.com'),
                (11, 'Helge', 'ropf@itu.dk'),
                (12, 'Adrian', 'adho@itu.dk');
        ");

        // Insert test messages
        await context.Database.ExecuteSqlRawAsync(@"
            INSERT INTO message (Id, UserId, Text, Timestamp) VALUES
                (0, 10, 'They were married in Chicago, with old Smith, and was expected aboard every day; meantime, the two went past me.', 1690895677),
                (1, 10, 'And then, as he listened to all that''s left o'' twenty-one people.', 1690895721),
                (2, 10, 'In various enchanted attitudes, like the Sperm Whale.', 1690895698),
                (3, 5, 'Unless we succeed in establishing ourselves in some monomaniac way whatever significance might lurk in them.', 1690895674);
        ");
    }
}
