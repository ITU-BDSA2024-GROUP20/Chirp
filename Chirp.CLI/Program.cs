using Chirp.CLI;

if (args[0] == "read")
{
    UserInterface.read();
} else if (args[0] == "cheep")
{
    UserInterface.cheep(args);
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