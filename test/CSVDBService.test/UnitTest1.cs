namespace CSVDBService.test;
using System.Net.Http.Json;

public class UnitTest1
{
    [Fact]
    public async void ReadIntTest()
    {
        var baseURL = "http://localhost:5241";
        using HttpClient client = new();
        client.BaseAddress = new Uri(baseURL);

        HttpResponseMessage response = await client.GetAsync("/cheeps");
        var cheeps = await response.Content.ReadFromJsonAsync<IEnumerable<Cheep>>();
        Assert.True((int)response.StatusCode == 200);

        foreach (var record in cheeps){
            Assert.Equal("ropf", record.Author);
            Assert.Equal("Hello, BDSA students!", record.Message);
            Assert.Equal(1690891760, record.Timestamp);
            break;
        }
    }


    [Fact]
    public async void StoreIntTest(){
        var baseURL = "http://localhost:5241";
        using HttpClient client = new();
        client.BaseAddress = new Uri(baseURL);
        var cheep = new Cheep("ropf", "Hello, BDSA students!", 1690891760);
        HttpResponseMessage response = await client.PostAsJsonAsync("/cheep", cheep);
        Assert.True((int)response.StatusCode == 200);
    }
}

public record Cheep(
    string Author,
    string Message,
    long Timestamp);