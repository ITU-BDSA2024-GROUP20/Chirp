using System.Diagnostics;
using Microsoft.Playwright;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests
{


    private IBrowser? browser;
    private IPage? Page;

    [SetUp]
    public async Task SetUp()
    {
        if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
        {
            Assert.Ignore("Test ignored on GitHub Actions");
        }
        else
        {
            var playwright = await Playwright.CreateAsync();

            browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
            });
            var context = await browser.NewContextAsync();
            Page = await context.NewPageAsync();
        }
    }

    [TearDown]
    public async Task TearDown()
    {
        if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
        {
            Assert.Ignore("Test ignored on GitHub Actions");
        }
        else
        {
            if (browser != null)
            {
                await browser.CloseAsync();
            }
        }
    }

    [Test]
    public async Task TestRegister()
    {
        if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
        {
            Assert.Ignore("Test ignored on GitHub Actions");
        }
        else
        {
            await Page.GotoAsync("http://localhost:5273/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "register" }).ClickAsync();
            await Page.GetByPlaceholder("username").ClickAsync();
            await Page.GetByPlaceholder("username").FillAsync("Anonymous");
            await Page.GetByPlaceholder("username").PressAsync("Tab");
            await Page.GetByPlaceholder("name@example.com").FillAsync("Anonymous@gmail.com");
            await Page.GetByPlaceholder("name@example.com").PressAsync("Tab");
            await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Password1!");
            await Page.GetByLabel("Password", new() { Exact = true }).PressAsync("Tab");
            await Page.GetByLabel("Confirm Password").FillAsync("Password1!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();
        }
    }

    [Test]
    public async Task TestLogin()
    {
        if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
        {
            Assert.Ignore("Test ignored on GitHub Actions");
        }
        else
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


    [Test]
    public async Task TestLogout()
    {
        if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
        {
            Assert.Ignore("Test ignored on GitHub Actions");
        }
        else
        {
            await Page.GotoAsync("http://localhost:5273/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
            await Page.GetByPlaceholder("email/username").ClickAsync();
            await Page.GetByPlaceholder("email/username").FillAsync("Helge");
            await Page.GetByPlaceholder("password").ClickAsync();
            await Page.GetByPlaceholder("password").FillAsync("LetM31n!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "logout [Helge]" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Click here to Logout" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();
        }
    }


    [Test]
    public async Task TestCheeping()
    {
        if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
        {
            Assert.Ignore("Test ignored on GitHub Actions");
        }
        else
        {
            await Page.GotoAsync("http://localhost:5273/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
            await Page.GetByPlaceholder("email/username").ClickAsync();
            await Page.GetByPlaceholder("email/username").FillAsync("Helge");
            await Page.GetByPlaceholder("email/username").PressAsync("Tab");
            await Page.GetByPlaceholder("password").FillAsync("LetM31n!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
            await Page.Locator("#cheepDTO_Text").ClickAsync();
            await Page.Locator("#cheepDTO_Text").FillAsync("test cheep");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();
        }

    }


    [Test]
    public async Task TestLoginOnNewAccount()
    {
        if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
        {
            Assert.Ignore("Test ignored on GitHub Actions");
        }
        else
        {
            await Page.GotoAsync("http://localhost:5273/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "register" }).ClickAsync();
            await Page.GetByPlaceholder("username").ClickAsync();
            await Page.GetByPlaceholder("username").FillAsync("Anonymous1");
            await Page.GetByPlaceholder("name@example.com").ClickAsync();
            await Page.GetByPlaceholder("name@example.com").FillAsync("Anonymous1@gmail.com");
            await Page.GetByPlaceholder("name@example.com").PressAsync("Tab");
            await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Password1!");
            await Page.GetByLabel("Password", new() { Exact = true }).PressAsync("Tab");
            await Page.GetByLabel("Confirm Password").FillAsync("Password1!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "logout [Anonymous]" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Click here to Logout" }).ClickAsync();
            await Page.GetByPlaceholder("email/username").ClickAsync();
            await Page.GetByPlaceholder("email/username").FillAsync("Anonymous1");
            await Page.GetByPlaceholder("email/username").PressAsync("Tab");
            await Page.GetByPlaceholder("password").FillAsync("Password1!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        }
    }


    
    [Test]
    public async Task TestFollow()
    {
        if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
        {
            Assert.Ignore("Test ignored on GitHub Actions");
        }
        else
        {
            await Page.GotoAsync("http://localhost:5273/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "register" }).ClickAsync();
            await Page.GetByPlaceholder("username").ClickAsync();
            await Page.GetByPlaceholder("username").FillAsync("Anonymous2");
            await Page.GetByPlaceholder("name@example.com").ClickAsync();
            await Page.GetByPlaceholder("name@example.com").FillAsync("Anonymous2@gmail.com");
            await Page.GetByPlaceholder("name@example.com").PressAsync("Tab");
            await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Password1!");
            await Page.GetByLabel("Password", new() { Exact = true }).PressAsync("Tab");
            await Page.GetByLabel("Confirm Password").FillAsync("Password1!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();
            await Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Follow Starbuck now is what we hear the worst. — 01-08-2023" }).GetByRole(AriaRole.Button).ClickAsync();
            await Page.Locator("strong").Filter(new() { HasText = "Mellie Yost Follow" }).GetByRole(AriaRole.Button).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();
        }
    }

    [Test]
    public async Task TestUnfollow()
    {
        if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
        {
            Assert.Ignore("Test ignored on GitHub Actions");
        }
        else
        {
            await Page.GotoAsync("http://localhost:5273/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "register" }).ClickAsync();
            await Page.GetByPlaceholder("username").ClickAsync();
            await Page.GetByPlaceholder("username").FillAsync("Anonymous3");
            await Page.GetByPlaceholder("username").PressAsync("Tab");
            await Page.GetByPlaceholder("name@example.com").FillAsync("Anonymous3@gmail.com");
            await Page.GetByPlaceholder("name@example.com").PressAsync("Tab");
            await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Password1!");
            await Page.GetByLabel("Password", new() { Exact = true }).PressAsync("Tab");
            await Page.GetByLabel("Confirm Password").FillAsync("Password1!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();
            await Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Follow Starbuck now is what we hear the worst. — 01-08-2023" }).GetByRole(AriaRole.Button).ClickAsync();
            await Page.Locator("strong").Filter(new() { HasText = "Mellie Yost Follow" }).GetByRole(AriaRole.Button).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();
            await Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine UnFollow Starbuck now is what we hear the worst. — 01-08-" }).GetByRole(AriaRole.Button).ClickAsync();
            await Page.Locator("li").Filter(new() { HasText = "Mellie Yost UnFollow But what" }).GetByRole(AriaRole.Button).ClickAsync();
        }
    }
}