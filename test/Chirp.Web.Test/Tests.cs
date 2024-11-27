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
            var myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            bool isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMyTimelineVisible);
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
            
            var helgatimeline = Page.GetByRole(AriaRole.Heading, new(){Name = "Helge's Timeline"});
            bool isHelgatimelineVisible = await helgatimeline.IsVisibleAsync();
            Assert.IsTrue(isHelgatimelineVisible);
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
            var logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Helge]"});
            bool isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isLogOutVisible);
            await Page.GetByRole(AriaRole.Link, new() { Name = "logout [Helge]" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Click here to Logout" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();
            var myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            bool isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("isMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " FAIL" : " PASS"));
            Assert.IsFalse(isMyTimelineVisible);
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
            var cheepBox = Page.Locator("div.cheepbox h3");
            bool isCheepBoxVisible = await cheepBox.IsVisibleAsync();
            Console.WriteLine("isCheepBoxVisible: " + isCheepBoxVisible + (isCheepBoxVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isCheepBoxVisible);
            var cheepText = await cheepBox.TextContentAsync();
            Console.WriteLine("cheepText: " + cheepText + ("What's on your mind Helge?".Equals(cheepText) ? " PASS" : " FAIL"));
            Assert.IsTrue("What's on your mind Helge?".Equals(cheepText));
            await Page.Locator("#cheepDTO_Text").ClickAsync();
            await Page.Locator("#cheepDTO_Text").FillAsync("test cheep");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();
            var cheep = Page.GetByText("Helge test cheep").First;
            var isCheepVisible = await cheep.IsVisibleAsync();
            Console.WriteLine("isCheepVisible: " + isCheepVisible + (isCheepVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isCheepVisible);
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
            var myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            bool isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMyTimelineVisible);
            var logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Anonymous1]"});
            bool isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isLogOutVisible);
            await Page.GetByRole(AriaRole.Link, new() { Name = "logout [Anonymous1]" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Click here to Logout" }).ClickAsync();
            myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("isMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " FAIL" : " PASS"));
            Assert.IsFalse(isMyTimelineVisible);
            await Page.GetByPlaceholder("email/username").ClickAsync();
            await Page.GetByPlaceholder("email/username").FillAsync("Anonymous1");
            await Page.GetByPlaceholder("email/username").PressAsync("Tab");
            await Page.GetByPlaceholder("password").FillAsync("Password1!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
            myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMyTimelineVisible);
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
            var myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            bool isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMyTimelineVisible);
            var logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Anonymous2]"});
            bool isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isLogOutVisible);
            await Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();
            await Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Follow" }).First.GetByRole(AriaRole.Button).ClickAsync();
            await Page.Locator("strong").Filter(new() { HasText = "Mellie Yost Follow" }).GetByRole(AriaRole.Button).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();
            var getJacqualine = Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine" }).First.GetByRole(AriaRole.Button);
            bool isJacqualineVisible = await getJacqualine.IsVisibleAsync();
            Console.WriteLine("isJacqualineVisible: " + isJacqualineVisible + (isJacqualineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isJacqualineVisible);
            var getMellieYost = Page.Locator("li").Filter(new (){ HasText = "Mellie Yost" }).GetByRole(AriaRole.Link).First;
            bool isMellieYostVisible = await getMellieYost.IsVisibleAsync();
            Console.WriteLine("isMellieYostVisible: " + isMellieYostVisible + (isMellieYostVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMellieYostVisible);
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
            await Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Follow" }).First.GetByRole(AriaRole.Button).ClickAsync();
            await Page.Locator("strong").Filter(new() { HasText = "Mellie Yost Follow" }).First.GetByRole(AriaRole.Button).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();
            var getJacqualine = Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine" }).First.GetByRole(AriaRole.Button);
            bool isJacqualineVisible = await getJacqualine.IsVisibleAsync();
            Console.WriteLine("isJacqualineVisible: " + isJacqualineVisible + (isJacqualineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isJacqualineVisible);
            await Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine UnFollow" }).First.GetByRole(AriaRole.Button).ClickAsync();
            getJacqualine = Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine" }).First.GetByRole(AriaRole.Button);
            isJacqualineVisible = await getJacqualine.IsVisibleAsync();
            Console.WriteLine("isJacqualineVisible: " + isJacqualineVisible + (isJacqualineVisible ? " FAIL" : " PASS"));
            Assert.IsFalse(isJacqualineVisible);
            await Page.Locator("li").Filter(new() { HasText = "Mellie Yost UnFollow" }).First.GetByRole(AriaRole.Button).ClickAsync();
        }
    }

    [Test]
    public async Task GitHubLoginTest()
    {
        if(Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
        {
            Assert.Ignore("Test ignored on GitHub Actions");
        }
        else{
            await Page.GotoAsync("http://localhost:5273/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "GitHub" }).ClickAsync();
            await Page.GetByLabel("Username or email address").ClickAsync();
            await Page.GetByLabel("Username or email address").FillAsync("Chirp20");
            await Page.GetByLabel("Password").ClickAsync();
            await Page.GetByLabel("Password").FillAsync("Chirpdummy1");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Sign in", Exact = true }).ClickAsync();
            
            //if first time login in with the acount uncomment this
            //await Page.GetByRole(AriaRole.Button, new() { Name = "Authorize ITU-BDSA2024-GROUP20" }).ClickAsync();
            
            await Page.WaitForURLAsync(url => url.StartsWith("http://localhost:5273/Identity/Account/ExternalLogin"));
            await Page.GetByPlaceholder("Please enter your email.").ClickAsync();
            await Page.GetByPlaceholder("Please enter your email.").FillAsync("chirp20@gmail.com");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
            
            var chirp20timeline = Page.GetByRole(AriaRole.Heading, new(){Name = "Chirp20's Timeline"});
            bool ischirp20timelineVisible = await chirp20timeline.IsVisibleAsync();
            Assert.IsTrue(ischirp20timelineVisible);

        }
    }


}