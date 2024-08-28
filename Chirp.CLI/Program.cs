if (args[0] == "read")
{
    try
    {
        using StreamReader reader = new StreamReader(@"chirp_cli_db.csv");
        string text = reader.ReadToEnd();
        string[] fullMessage = text.Split('\n');
        Post[] posts = new Post[fullMessage.Length];
        for (int i = 0; i < fullMessage.Length; i++)
        {
            posts[i] = new Post(fullMessage[i]);
            Console.WriteLine(posts[i].getPost());
        }
    }
    catch (IOException e)
    {
        Console.WriteLine("This file could not be read:");
        Console.WriteLine(e.Message);
    }
}

class Post
{
    public string Author { get; set; }
    public string Message { get; set; }
    public string Timestamp { get; set; }

    public Post(String fullMessage)
    {
        int index1 = fullMessage.IndexOf(",");
        Author = fullMessage.Substring(0, index1);
        Message = fullMessage.Substring(index1+1);
        int index2 = Message.LastIndexOf(",");
        Timestamp = Message.Substring(index2);
    }
    public Post(String Author, String Message, String Timestamp)
    {
        this.Author = Author;
        this.Message = Message;
        this.Timestamp = Timestamp;
    }

    public string getPost()
    {
        return Author + " @ " + Timestamp + ": " + Message;
    }
}