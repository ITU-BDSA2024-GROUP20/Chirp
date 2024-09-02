if (args[0] == "read")
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
}else if (args[0] == "cheep")
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

class Post
{
    public string Author { get; set; }
    public string Message { get; set; }
    public string Timestamp { get; set; }

    public Post(String fullMessage)
    {
        int index1 = fullMessage.IndexOf(",");
        int index2 = fullMessage.LastIndexOf(",");
        Author = fullMessage.Substring(0, index1);
        Message = fullMessage.Substring(index1+2, index2-index1-3);
        Timestamp = fullMessage.Substring(index2+1);
    }
    public Post(String Author, String Message, String Timestamp)
    {
        this.Author = Author;
        this.Message = Message;
        this.Timestamp = Timestamp;
    }

    public string getPost()
    {
        return Author + " @ " + returnTimeStamp() + ": " + Message;
    }

    public string returnTimeStamp(){
        try{
            //long timeLong = Convert.ToInt64(Timestamp);
            var _timestamp = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(Timestamp)).LocalDateTime.ToString("MM dd yy HH:mm:ss").Split(" ");
            return _timestamp[0]+"/"+_timestamp[1]+"/"+_timestamp[2]+" "+_timestamp[3];
        }catch(Exception e){
            Console.WriteLine("could not convert" + e);
            return Timestamp;
        }
    }

}