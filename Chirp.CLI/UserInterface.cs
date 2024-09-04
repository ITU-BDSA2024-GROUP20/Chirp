namespace Chirp.CLI;

public class UserInterface
{

    public static void read()
    {
        try
        {
            using StreamReader reader = new StreamReader(@"chirp_cli_db.csv");
            string text = reader.ReadToEnd();
            string[] fullMessage = text.Split('\n');
            Post[] posts = new Post[fullMessage.Length];
            for (int i = 1; i < fullMessage.Length; i++)
            {
                if(fullMessage[i] != ""){
                    posts[i] = new Post(fullMessage[i]);
                    Console.WriteLine(posts[i].getPost());
                }
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("This file could not be read:");
            Console.WriteLine(e.Message);
        }
    }

    public static void cheep(String[] args)
    {
        try
        {
            using StreamWriter writer = new StreamWriter(@"chirp_cli_db.csv", true);
            string userName = Environment.UserName;
            writer.WriteLine(userName + ",\"" + args[1] + "\"," + DateTimeOffset.Now.ToUnixTimeSeconds());
        
        }
        catch (IOException e)
        {
            Console.WriteLine("This file could not be written:");
            Console.WriteLine(e.Message);
        }
    }
    }

