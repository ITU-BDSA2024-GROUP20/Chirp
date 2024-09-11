namespace Chirp.CLI;

public class UserInterface
{
    public static void read(IEnumerable<Cheep> cheeps)
    {
        foreach (var record in cheeps)
        {
            Console.WriteLine(returnCheep(record));
        }
    }

    public static string returnCheep(Cheep cheep)
    {
        return cheep.Author + " @ " + returnTimeStamp(cheep.Timestamp) + " " + cheep.Message;
    }

    public static string returnTimeStamp(Int64 Timestamp)
    {
        var _timestamp = DateTimeOffset.FromUnixTimeSeconds(Timestamp)
            .LocalDateTime.ToString("MM dd yy HH:mm:ss")
            .Split(" ");
        return _timestamp[0] + "/" + _timestamp[1] + "/" + _timestamp[2] + " " + _timestamp[3]+":";
    }
}