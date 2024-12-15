global using NUnit.Framework;
using System.Diagnostics;
using Microsoft.Playwright;
namespace Chirp.Live.Test;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests
{
    private IBrowser? browser;
    private IPage? Page;

    [SetUp]
    public async Task SetUp()
    {
        var playwright = await Playwright.CreateAsync();
        
        browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        { 
            Headless = false,
        });
        var context = await browser.NewContextAsync();
        Page = await context.NewPageAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        if (browser != null) 
        { 
            await browser.CloseAsync();
        }
    }
    [Test]
    public async Task LiveTest()
    {
        await Page.GotoAsync("https://bdsagroup20chirprazor-hdb4bch7ejb3abbd.northeurope-01.azurewebsites.net/");
        await Page.GetByRole(AriaRole.Link, new() { Name = "LOGIN" }).ClickAsync();
        await Page.GetByPlaceholder("email/username").ClickAsync();
        await Page.GetByPlaceholder("email/username").FillAsync("Jacqualine Gilcoine");
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("Password1!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "ABOUT ME" }).ClickAsync();
        
        var name =  Page.GetByRole(AriaRole.Heading, new() { Name = "User: Jacqualine Gilcoine" });
        bool nameVisible = await name.IsVisibleAsync();
        Console.WriteLine("nameVisible: " + nameVisible + (nameVisible ? " PASS" : " FAIL"));
        Assert.IsTrue(nameVisible);
        var email =  Page.GetByRole(AriaRole.Heading, new() { Name = "Email: [ Jacqualine.Gilcoine@" });
        bool emailVisible = await email.IsVisibleAsync();
        Console.WriteLine("isLogOutVisible: " + emailVisible + (emailVisible ? " PASS" : " FAIL"));
        Assert.IsTrue(emailVisible);
        
    }
    
    
}