namespace test;
using Chirp.CLI;
using System.Diagnostics;

public class UnitTest1
{
    [Fact]
    public void UnixTimeConverTest()
    {
        string timestamp = UserInterface.returnTimeStamp(1690891760);
        Assert.Equal("08/01/23 14:09:20:", timestamp);
        timestamp = UserInterface.returnTimeStamp(1690978778);
        Assert.Equal("08/02/23 14:19:38:", timestamp);
        timestamp = UserInterface.returnTimeStamp(1690979858);
        Assert.Equal("08/02/23 14:37:38:", timestamp);
        timestamp = UserInterface.returnTimeStamp(1690981487);
        Assert.Equal("08/02/23 15:04:47:", timestamp);
    }

    [Fact]
    public void CheepFromRecordTest()
    {
        var record = new Cheep("ropf", "Hello, BDSA students!", 1690891760);
        string cheep = UserInterface.returnCheep(record);
        Assert.Equal("ropf @ 08/01/23 14:09:20: Hello, BDSA students!", cheep);

        record = new Cheep("adho", "Welcome to the course!", 1690978778);
        cheep = UserInterface.returnCheep(record);
        Assert.Equal("adho @ 08/02/23 14:19:38: Welcome to the course!", cheep);

        record = new Cheep("adho", "I hope you had a good summer.", 1690979858);
        cheep = UserInterface.returnCheep(record);
        Assert.Equal("adho @ 08/02/23 14:37:38: I hope you had a good summer.", cheep);

        record = new Cheep("ropf", "Cheeping cheeps on Chirp :)", 1690981487);
        cheep = UserInterface.returnCheep(record);
        Assert.Equal("ropf @ 08/02/23 15:04:47: Cheeping cheeps on Chirp :)", cheep);
    }



    [Fact]
    public void UserInterfaceReadTest()
    {
        StringWriter writer = new StringWriter();
        Console.SetOut(writer);
        var record = new Cheep("ropf", "Hello, BDSA students!", 1690891760);
        
        UserInterface.read(new List<Cheep> { record });
      
        Assert.Equal("ropf @ 08/01/23 14:09:20: Hello, BDSA students!", writer.ToString().Trim());
    }

    [Fact]
    public void TestRead()
    {
        string path = AppDomain.CurrentDomain.BaseDirectory+@"../../../../../src/Chirp.CLI";
        // Act
        string output = "";
        using (var process = new Process())
        {
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = "run read";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = path;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            // Synchronously read the standard output of the spawned process.
            StreamReader reader = process.StandardOutput;
            output = reader.ReadToEnd();
            process.WaitForExit();
        }
        string fstCheep = output.Split("\n")[0];
        // Assert
        Assert.StartsWith("ropf", fstCheep);
        Assert.EndsWith("BDSA students!", fstCheep.Trim());
    }
    
    [Fact]
    public void TestCheep()
    {
        string path = AppDomain.CurrentDomain.BaseDirectory+@"../../../../../src/Chirp.CLI";
        // Act
        string output = "";
        using (var process = new Process())
        {
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = "run cheep \"test cheep\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = path;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            
            process.WaitForExit();
        }
        using (var process = new Process())
        {
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = "run read";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = path;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            // Synchronously read the standard output of the spawned process.
            StreamReader reader = process.StandardOutput;
            output = reader.ReadToEnd();
            process.WaitForExit();
        }
        var outputList = output.Split("\n");
        string fstCheep = outputList[outputList.Length - 2];
        // Assert
        Assert.EndsWith("test cheep", fstCheep.Trim());
    }

}