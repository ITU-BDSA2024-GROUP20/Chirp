using System.Diagnostics;
using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{
    
   
    
    
    [Test]
    public async Task TestRegister()
    {
        await Page.GotoAsync("http://localhost:5273/");
        await Page.GetByRole(AriaRole.Link, new() { Name = "register" }).ClickAsync();
        await Page.GetByPlaceholder("username").ClickAsync();
        await Page.GetByPlaceholder("username").FillAsync("SifDJ");
        await Page.GetByPlaceholder("username").PressAsync("Tab");
        await Page.GetByPlaceholder("name@example.com").FillAsync("sif@gmail.com");
        await Page.GetByPlaceholder("name@example.com").PressAsync("Tab");
        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("HellaC00!");
        await Page.GetByLabel("Password", new() { Exact = true }).PressAsync("Tab");
        await Page.GetByLabel("Confirm Password").FillAsync("HellaC00!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
    }
    
    [Test]
    public async Task TestLogin()
    {
        await Page.GotoAsync("http://localhost:5273/");
        await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("email/username").ClickAsync();
        await Page.GetByPlaceholder("email/username").FillAsync("Helge");
        await Page.GetByPlaceholder("email/username").PressAsync("Tab");
        await Page.GetByPlaceholder("password").FillAsync("LetM31n!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
    }
}