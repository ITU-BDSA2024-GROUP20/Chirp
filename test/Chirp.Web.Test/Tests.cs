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
            //this needs to be commented out because it makes it fail on github
            //Assert.Ignore("Test ignored on GitHub Actions");
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
            //this needs to be commented out because it makes it fail on github
            //Assert.Ignore("Test ignored on GitHub Actions");
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
            Console.WriteLine("TestRegister:");
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
            //logedin checks
            var myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            bool isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMyTimelineVisible);
            var logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Anonymous]"});
            bool isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isLogOutVisible);
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
            Console.WriteLine("TestLogin:");
            await Page.GotoAsync("http://localhost:5273/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
            await Page.GetByPlaceholder("email/username").ClickAsync();
            await Page.GetByPlaceholder("email/username").FillAsync("Helge");
            await Page.GetByPlaceholder("email/username").PressAsync("Tab");
            await Page.GetByPlaceholder("password").FillAsync("LetM31n!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
            //Logedin checks
            var myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            bool isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMyTimelineVisible);
            var logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Helge]"});
            bool isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isLogOutVisible);
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
            Console.WriteLine("TestLogout:");
            await Page.GotoAsync("http://localhost:5273/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
            await Page.GetByPlaceholder("email/username").ClickAsync();
            await Page.GetByPlaceholder("email/username").FillAsync("Helge");
            await Page.GetByPlaceholder("password").ClickAsync();
            await Page.GetByPlaceholder("password").FillAsync("LetM31n!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
            //Logedin Cheeks
            var myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            bool isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMyTimelineVisible);
            var logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Helge]"});
            bool isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isLogOutVisible);
            
            Console.WriteLine("logging out");
            await Page.GetByRole(AriaRole.Link, new() { Name = "logout [Helge]" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Click here to Logout" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync(); 
            //checking that it was loged out
            myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " FAIL" : " PASS"));
            Assert.IsFalse(isMyTimelineVisible);
            logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Helge]"});
            isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " FAIL" : " PASS"));
            Assert.IsFalse(isLogOutVisible);
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
            Console.WriteLine("TestCheeping:");
            await Page.GotoAsync("http://localhost:5273/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
            await Page.GetByPlaceholder("email/username").ClickAsync();
            await Page.GetByPlaceholder("email/username").FillAsync("Helge");
            await Page.GetByPlaceholder("email/username").PressAsync("Tab");
            await Page.GetByPlaceholder("password").FillAsync("LetM31n!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
            //Logedin Checks
            var myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            bool isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMyTimelineVisible);
            var logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Helge]"});
            bool isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isLogOutVisible);
            
            //finding and checking if the cheepbox exist
            var cheepBox = Page.Locator("div.cheepbox h3");
            bool isCheepBoxVisible = await cheepBox.IsVisibleAsync();
            Console.WriteLine("isCheepBoxVisible: " + isCheepBoxVisible + (isCheepBoxVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isCheepBoxVisible);
            //checking if it right
            var cheepText = await cheepBox.TextContentAsync();
            Console.WriteLine("cheepText: " + cheepText + ("What's on your mind Helge?".Equals(cheepText) ? " PASS" : " FAIL"));
            Assert.IsTrue("What's on your mind Helge?".Equals(cheepText));
            //writing the cheep
            await Page.Locator("#CheepDto_Text").ClickAsync();
            await Page.Locator("#CheepDto_Text").FillAsync("test cheep 123456789");
            Console.WriteLine("sending cheep");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();
            var cheep = Page.GetByText("test cheep 123456789").First;
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
            Console.WriteLine("TestLoginOnNewAccount:");
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
            Console.WriteLine("account created");
            //logedin checks
            var myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            bool isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMyTimelineVisible);
            var logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Anonymous1]"});
            bool isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isLogOutVisible);
            Console.WriteLine("loging out");
            //to logout of the account
            await Page.GetByRole(AriaRole.Link, new() { Name = "logout [Anonymous1]" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Click here to Logout" }).ClickAsync();
            //checking if logedout
            myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("isMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " FAIL" : " PASS"));
            Assert.IsFalse(isMyTimelineVisible);
            await Page.GetByPlaceholder("email/username").ClickAsync();
            await Page.GetByPlaceholder("email/username").FillAsync("Anonymous1");
            await Page.GetByPlaceholder("email/username").PressAsync("Tab");
            await Page.GetByPlaceholder("password").FillAsync("Password1!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
            Console.WriteLine("logged back in");
            //logedin checks
            myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMyTimelineVisible);
            logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Anonymous1]"});
            isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isLogOutVisible);
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
            Console.WriteLine("TestFollow:");
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
            //logedin checks
            var myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            bool isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMyTimelineVisible);
            var logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Anonymous2]"});
            bool isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isLogOutVisible);
            //going to public timeline
            await Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();
            //following to people
            await Page.Locator("li").Filter(new() { HasText = "Follow Jacqualine Gilcoine - 01-08-2023 13:17:39 Starbuck now is what we hear" }).GetByRole(AriaRole.Button).ClickAsync();
            await Page.Locator("li").Filter(new() { HasText = "Follow Mellie Yost - 01-08-" }).GetByRole(AriaRole.Button).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();
            Console.WriteLine("going to my timeline after following");
            //checking if Jacqualine is on Anonumous2 private timeline
            var getJacqualine = Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine" }).First.GetByRole(AriaRole.Button);
            bool isJacqualineVisible = await getJacqualine.IsVisibleAsync();
            Console.WriteLine("isJacqualineVisible: " + isJacqualineVisible + (isJacqualineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isJacqualineVisible);
            //checking if Mellie is on Anonumous2 private timeline
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
            Console.WriteLine("Unfollow:");
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
            //logedin cheecks
            var myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            bool isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMyTimelineVisible);
            var logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Anonymous3]"});
            bool isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isLogOutVisible);
            //going to public timeline
            await Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();
            //following people
            await Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine" }).First.GetByRole(AriaRole.Button).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();
            Console.WriteLine("going to private timeline after following");
            //checking if Jacqualine is on Anonymous3 private timeline
            var getJacqualine = Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine" }).First.GetByRole(AriaRole.Button);
            bool isJacqualineVisible = await getJacqualine.IsVisibleAsync();
            Console.WriteLine("isJacqualineVisible: " + isJacqualineVisible + (isJacqualineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isJacqualineVisible);
            //unfollowing Jacqualine
            await Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine" }).First.GetByRole(AriaRole.Button).ClickAsync();
            //checking if Jacqualine is on Anonymous3 private timeline
            getJacqualine = Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine" }).First.GetByRole(AriaRole.Button);
            isJacqualineVisible = await getJacqualine.IsVisibleAsync();
            Console.WriteLine("isJacqualineVisible: " + isJacqualineVisible + (isJacqualineVisible ? " FAIL" : " PASS"));
            Assert.IsFalse(isJacqualineVisible);
            
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
            Console.WriteLine("GitHubLoginTest:");
            await Page.GotoAsync("http://localhost:5273/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "GitHub" }).ClickAsync();
            await Page.GetByLabel("Username or email address").ClickAsync();
            await Page.GetByLabel("Username or email address").FillAsync("Chirp20");
            await Page.GetByLabel("Password").ClickAsync();
            await Page.GetByLabel("Password").FillAsync("Chirpdummy1");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Sign in", Exact = true }).ClickAsync();
            Console.WriteLine("Sign in in progress");
            //if first time login in with the acount uncomment this
            //await Page.GetByRole(AriaRole.Button, new() { Name = "Authorize ITU-BDSA2024-GROUP20" }).ClickAsync();
            
            await Page.WaitForURLAsync(url => url.StartsWith("http://localhost:5273/Identity/Account/ExternalLogin"));
            await Page.GetByLabel("Email").ClickAsync();
            await Page.GetByLabel("Email").FillAsync("chirp20@gmail.com");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
            Console.WriteLine("Sign in in Complete");
            //logedin checks
            var myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            bool isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMyTimelineVisible);
            var logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Chirp20]"});
            bool isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isLogOutVisible);

        }
    }
    
    [Test]
    public async Task BlocksFollowingShownOnAboutMe()
    {
        if(Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
        {
            Assert.Ignore("Test ignored on GitHub Actions");
        }
        else
        {
            Console.WriteLine("BlocksFollowingShownOnAboutMe:");
            await Page.GotoAsync("http://localhost:5273/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "REGISTER" }).ClickAsync();
            await Page.GetByPlaceholder("username").ClickAsync();
            await Page.GetByPlaceholder("username").FillAsync("Anonymous4");
            await Page.GetByPlaceholder("name@example.com").ClickAsync();
            await Page.GetByPlaceholder("name@example.com").FillAsync("Anonymous4@gmail.com");
            await Page.GetByPlaceholder("name@example.com").PressAsync("Tab");
            await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Password1!");
            await Page.GetByLabel("Password", new() { Exact = true }).PressAsync("Tab");
            await Page.GetByLabel("Confirm Password").FillAsync("Password1!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
            //logedin cheks
            var myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            bool isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMyTimelineVisible);
            var logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Anonymous4]"});
            bool isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isLogOutVisible);
            //going to public timeline
            await Page.GetByRole(AriaRole.Link, new() { Name = "PUBLIC TIMELINE" }).ClickAsync();
            //following Jacqualine
            await Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine" }).GetByRole(AriaRole.Button).First.ClickAsync();
            //blocking Mellie
            await Page.GetByRole(AriaRole.Link, new() { Name = "Mellie Yost" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Block" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "ABOUT ME" }).ClickAsync();
            Console.WriteLine("following and blocking compleate going to aboutme");
            //Checking if Anonymous4 is following Jacqualine
            var followingJacqualine = Page.GetByText("UnFollow Jacqualine Gilcoine");
            bool isfollowingJacqualine = await followingJacqualine.IsVisibleAsync();
            Console.WriteLine("isfollowingJacqualine: " + isfollowingJacqualine + (isfollowingJacqualine ? " PASS" : " FAIL"));
            Assert.IsTrue(isfollowingJacqualine);
            //Checking if Anonymous4 is blocking Mellie
            var blockingMellie = Page.GetByText("UnBlock Mellie Yost");
            bool isblockingMellie = await blockingMellie.IsVisibleAsync();
            Console.WriteLine("isblockingMellie: " + isblockingMellie + (isblockingMellie ? " PASS" : " FAIL"));
            Assert.IsTrue(isblockingMellie);
        }
    }
    
     [Test]
    public async Task TestBlock()
    {
        if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
        {
            Assert.Ignore("Test ignored on GitHub Actions");
        }
        else
        {
            Console.WriteLine("TestBlock:");
            await Page.GotoAsync("http://localhost:5273/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "register" }).ClickAsync();
            await Page.GetByPlaceholder("username").ClickAsync();
            await Page.GetByPlaceholder("username").FillAsync("Anonymous5");
            await Page.GetByPlaceholder("name@example.com").ClickAsync();
            await Page.GetByPlaceholder("name@example.com").FillAsync("Anonymous5@gmail.com");
            await Page.GetByPlaceholder("name@example.com").PressAsync("Tab");
            await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Password1!");
            await Page.GetByLabel("Password", new() { Exact = true }).PressAsync("Tab");
            await Page.GetByLabel("Confirm Password").FillAsync("Password1!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
            //Logedin Checks
            var myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            bool isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMyTimelineVisible);
            var logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Anonymous5]"});
            bool isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isLogOutVisible);
            //going to public timeline
            await Page.GetByRole(AriaRole.Link, new() { Name = "PUBLIC TIMELINE" }).ClickAsync();
            //blocking Jacqualine
            await Page.Locator("strong").Filter(new() { HasText = "Jacqualine Gilcoine - 01-08-2023 13:17:39" }).GetByRole(AriaRole.Link).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Block" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "PUBLIC TIMELINE" }).ClickAsync();
            Console.WriteLine("going to public timeline after Jacqualine has been blocked");
            //checking if Jacqualine cheeps are there
            var getJacqualine = Page.GetByText("Jacqualine Gilcoine — 01-08-2023 13:17:39 Starbuck now is what we hear the");
            bool isJacqualineVisible = await getJacqualine.IsVisibleAsync();
            Console.WriteLine("isJacqualineVisible: " + isJacqualineVisible + (isJacqualineVisible ? " FAIL" : " PASS"));
            Assert.IsFalse(isJacqualineVisible);
        }
    }

    [Test]
    public async Task TestUnBlock()
    {
        if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
        {
            Assert.Ignore("Test ignored on GitHub Actions");
        }
        else
        {
            Console.WriteLine("TestUnBlock");
            await Page.GotoAsync("http://localhost:5273/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "REGISTER" }).ClickAsync();
            await Page.GetByPlaceholder("username").ClickAsync();
            await Page.GetByPlaceholder("username").FillAsync("Anonymous6");
            await Page.GetByPlaceholder("name@example.com").ClickAsync();
            await Page.GetByPlaceholder("name@example.com").FillAsync("Anonymous6@gmail.com");
            await Page.GetByLabel("Password", new() { Exact = true }).ClickAsync();
            await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Password1!");
            await Page.GetByLabel("Password", new() { Exact = true }).PressAsync("Tab");
            await Page.GetByLabel("Confirm Password").FillAsync("Password1!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
            //Logedin checks
            var myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            bool isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMyTimelineVisible);
            var logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Anonymous6]"});
            bool isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isLogOutVisible);
            //going to public timeline
            await Page.GetByRole(AriaRole.Link, new() { Name = "PUBLIC TIMELINE" }).ClickAsync();
            //blocking Jacqualine
            await Page.Locator("strong").Filter(new() { HasText = "Jacqualine Gilcoine - 01-08-2023 13:17:39" }).GetByRole(AriaRole.Link).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Block" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "PUBLIC TIMELINE" }).ClickAsync();
            //checking if Jacqualine is blocked
            var getJacqualine = Page.GetByText("Jacqualine Gilcoine — 01-08-2023 13:17:39 Starbuck now is what we hear the");
            bool isJacqualineVisible = await getJacqualine.IsVisibleAsync();
            Console.WriteLine("isJacqualineVisible: " + isJacqualineVisible + (isJacqualineVisible ? " FAIL" : " PASS"));
            Assert.IsFalse(isJacqualineVisible);
            Console.WriteLine("unblocking");
            //going to aboutme
            await Page.GetByRole(AriaRole.Link, new() { Name = "ABOUT ME" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "Jacqualine Gilcoine" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "UnBlock" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "PUBLIC TIMELINE" }).ClickAsync();
            //checking if Jacqualine cheeps are visible again on the public timeline
            getJacqualine = Page.Locator("li").Filter(new() { HasText =  "Jacqualine Gilcoine - 01-08-2023 13:17:39 Starbuck now is what we hear" });
            isJacqualineVisible = await getJacqualine.IsVisibleAsync();
            Console.WriteLine("isJacqualineVisible: " + isJacqualineVisible + (isJacqualineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isJacqualineVisible);
        }
    }

    [Test]
    public async Task TestBlockUnfollows()
    {
        if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
        {
            Assert.Ignore("Test ignored on GitHub Actions");
        }
        else
        {
            Console.WriteLine("TestBlockUnfollows");
            await Page.GotoAsync("http://localhost:5273/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "REGISTER" }).ClickAsync();
            await Page.GetByPlaceholder("username").ClickAsync();
            await Page.GetByPlaceholder("username").FillAsync("Anonymous7");
            await Page.GetByPlaceholder("name@example.com").ClickAsync();
            await Page.GetByPlaceholder("name@example.com").FillAsync("Anonymous7@gmail.com");
            await Page.GetByPlaceholder("name@example.com").PressAsync("Tab");
            await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Password1!");
            await Page.GetByLabel("Password", new() { Exact = true }).PressAsync("Tab");
            await Page.GetByLabel("Confirm Password").FillAsync("Password1!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
            //Logeding checks
            var myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            bool isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMyTimelineVisible);
            var logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Anonymous7]"});
            bool isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isLogOutVisible);
            
            //going to public timeline to follow Jacqualine
            await Page.GetByRole(AriaRole.Link, new() { Name = "PUBLIC TIMELINE" }).ClickAsync();
            await Page.Locator("li").Filter(new() { HasText = "Follow Jacqualine Gilcoine - 01-08-2023 13:17:39 Starbuck now is what we hear" }).GetByRole(AriaRole.Button).ClickAsync();
            //going to aboutme to check if she is followed
            await Page.GetByRole(AriaRole.Link, new() { Name = "ABOUT ME" }).ClickAsync();
            var isfollowed = Page.GetByText("UnFollow Jacqualine Gilcoine");
            bool isfollowedVisible = await isfollowed.IsVisibleAsync();
            Console.WriteLine("isfollowedVisible: " + isfollowedVisible + (isfollowedVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isfollowedVisible);
            //going to Jacqualine timeline to block them
            Console.WriteLine("blocking");
            await Page.GetByRole(AriaRole.Link, new() { Name = "Jacqualine Gilcoine" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Block" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "ABOUT ME" }).ClickAsync();
            //checking if Jacqualine is still followed 
            isfollowed = Page.GetByText("UnFollow Jacqualine Gilcoine"); 
            isfollowedVisible = await isfollowed.IsVisibleAsync();
            Console.WriteLine("isfollowedVisible: " + isfollowedVisible + (isfollowedVisible ? " FAIL" : " PASS"));
            Assert.IsFalse(isfollowedVisible);
            //checking is Jacqualine is blocked
            var isblocked = Page.GetByText("UnBlock Jacqualine Gilcoine");
            bool isblockedVisible = await isblocked.IsVisibleAsync();
            Console.WriteLine("isblockedVisible: " + isblockedVisible + (isblockedVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isblockedVisible);
        }
    }
    
    [Test]
    public async Task UnfollowAndUnblockFromAboutMe()
    {
        if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
        {
            Assert.Ignore("Test ignored on GitHub Actions");
        }
        else
        {
            Console.WriteLine("UnfollowAndUnblockFromAboutMe:");
            await Page.GotoAsync("http://localhost:5273/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "REGISTER" }).ClickAsync();
            await Page.GetByPlaceholder("username").ClickAsync();
            await Page.GetByPlaceholder("username").FillAsync("Anonymous8");
            await Page.GetByPlaceholder("name@example.com").ClickAsync();
            await Page.GetByPlaceholder("name@example.com").FillAsync("Anonymous8@gmail.com");
            await Page.GetByPlaceholder("name@example.com").PressAsync("Tab");
            await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Password1!");
            await Page.GetByLabel("Password", new() { Exact = true }).PressAsync("Tab");
            await Page.GetByLabel("Confirm Password").FillAsync("Password1!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
            //login succescheck
            var myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            bool isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMyTimelineVisible);
            var logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Anonymous8]"});
            bool isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isLogOutVisible);
            //following Jacqualine and blocking Mellie
            await Page.GetByRole(AriaRole.Link, new() { Name = "PUBLIC TIMELINE" }).ClickAsync();
            await Page.Locator("li").Filter(new() { HasText = "Follow Jacqualine Gilcoine - 01-08-2023 13:17:39 Starbuck now is what we hear" }).GetByRole(AriaRole.Button).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "Mellie Yost" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Block" }).ClickAsync();
            //seeing if mellie was blocked
            await Page.GetByRole(AriaRole.Link, new() { Name = "PUBLIC TIMELINE" }).ClickAsync();
            var getMellie = Page.GetByText("Mellie Yost — 01-08-2023 13:17:33 But what was behind the barricade.");
            bool isMellieVisible = await getMellie.IsVisibleAsync();
            Console.WriteLine("isMellieVisible: " + isMellieVisible + (isMellieVisible ? " FAIL" : " PASS"));
            Assert.IsFalse(isMellieVisible);
            //seeing if jacqualine was followed
            await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();
            var getJacqualine = Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine" }).First.GetByRole(AriaRole.Button);
            bool isJacqualineVisible = await getJacqualine.IsVisibleAsync();
            Console.WriteLine("isJacqualineVisible: " + isJacqualineVisible + (isJacqualineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isJacqualineVisible);
            
            //seeing if they are on the about me page
            await Page.GetByRole(AriaRole.Link, new() { Name = "ABOUT ME" }).ClickAsync();
            await Page.GetByText("UnFollow Jacqualine Gilcoine").ClickAsync();
            await Page.GetByText("UnBlock Mellie Yost").ClickAsync();
            var followingJacqualine = Page.GetByText("UnFollow Jacqualine Gilcoine");
            bool isfollowingJacqualine = await followingJacqualine.IsVisibleAsync();
            Console.WriteLine("isfollowingJacqualine: " + isfollowingJacqualine + (isfollowingJacqualine ? " PASS" : " FAIL"));
            Assert.IsTrue(isfollowingJacqualine);
            var blockingMellie = Page.GetByText("UnBlock Mellie Yost");
            bool isblockingMellie = await blockingMellie.IsVisibleAsync();
            Console.WriteLine("isblockingMellie: " + isblockingMellie + (isblockingMellie ? " PASS" : " FAIL"));
            Assert.IsTrue(isblockingMellie);
            Console.WriteLine("unfollowing and unblocking");
            await Page.GetByRole(AriaRole.Button, new() { Name = "UnFollow" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "UnBlock" }).ClickAsync();
            
            //seeing if mellie was unblocked
            await Page.GetByRole(AriaRole.Link, new() { Name = "PUBLIC TIMELINE" }).ClickAsync();
            getMellie = Page.Locator("li").Filter(new() { HasText = "Mellie Yost" }).First;
            isMellieVisible = await getMellie.IsVisibleAsync();
            Console.WriteLine("isMellieVisible: " + isMellieVisible + (isMellieVisible ? " Pass" : " FAIL"));
            Assert.IsTrue(isMellieVisible);
            //seeing if jacqualine was unfollowed
            await Page.GetByRole(AriaRole.Link, new() { Name = "MY TIMELINE" }).ClickAsync();
            getJacqualine = Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine" }).First.GetByRole(AriaRole.Button);
            isJacqualineVisible = await getJacqualine.IsVisibleAsync();
            Console.WriteLine("isJacqualineVisible: " + isJacqualineVisible + (isJacqualineVisible ? " FAIL" : " PASS"));
            Assert.IsFalse(isJacqualineVisible);
        }
    }

    [Test]
    public async Task SeeOwnCheepsOnAboutMe()
    {
        if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
        {
            Assert.Ignore("Test ignored on GitHub Actions");
        }
        else
        {
            Console.WriteLine("SeeOwnCheepsOnAboutMe:");
            await Page.GotoAsync("http://localhost:5273/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "REGISTER" }).ClickAsync();
            await Page.GetByPlaceholder("username").ClickAsync();
            await Page.GetByPlaceholder("username").FillAsync("Anonymous9");
            await Page.GetByPlaceholder("username").PressAsync("Tab");
            await Page.GetByPlaceholder("name@example.com").FillAsync("Anonymous9@gmail.com");
            await Page.GetByPlaceholder("name@example.com").PressAsync("Tab");
            await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Password1!");
            await Page.GetByLabel("Password", new() { Exact = true }).PressAsync("Tab");
            await Page.GetByLabel("Confirm Password").FillAsync("Password1!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
            //login succescheck
            var myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            bool isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMyTimelineVisible);
            var logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Anonymous9]"});
            bool isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isLogOutVisible);
            //writing cheep1
            await Page.Locator("#CheepDto_Text").ClickAsync();
            await Page.Locator("#CheepDto_Text").FillAsync("cheep1");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();
            //checking if cheep1 was posted
            var cheep = Page.GetByText("cheep1").First;
            var isCheepVisible = await cheep.IsVisibleAsync();
            Console.WriteLine("isCheep1Visible: " + isCheepVisible + (isCheepVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isCheepVisible);
            //writing cheep2
            await Page.Locator("#CheepDto_Text").ClickAsync();
            await Page.Locator("#CheepDto_Text").FillAsync("cheep2");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();
            //checking if cheep2 was posted
            cheep = Page.GetByText("cheep2").First;
            isCheepVisible = await cheep.IsVisibleAsync();
            Console.WriteLine("isCheep2Visible: " + isCheepVisible + (isCheepVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isCheepVisible);
            Console.WriteLine("going to AboutMe");
            await Page.GetByRole(AriaRole.Link, new() { Name = "ABOUT ME" }).ClickAsync();
            //checking if cheep1 is there
            cheep = Page.GetByText("cheep1").First;
            isCheepVisible = await cheep.IsVisibleAsync();
            Console.WriteLine("isCheep1Visible: " + isCheepVisible + (isCheepVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isCheepVisible);
            //checking if cheep2 is there
            cheep = Page.GetByText("cheep1").First;
            isCheepVisible = await cheep.IsVisibleAsync();
            Console.WriteLine("isCheep2Visible: " + isCheepVisible + (isCheepVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isCheepVisible);

        }
    }

    [Test]
    public async Task Pagnation()
    {
        if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
        {
            Assert.Ignore("Test ignored on GitHub Actions");
        }
        else
        {
            Console.WriteLine("Pagnation:");
            await Page.GotoAsync("http://localhost:5273/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "REGISTER" }).ClickAsync();
            await Page.GetByPlaceholder("username").ClickAsync();
            await Page.GetByPlaceholder("username").FillAsync("Anonymous10");
            await Page.GetByPlaceholder("username").PressAsync("Tab");
            await Page.GetByPlaceholder("name@example.com").FillAsync("Anonymous10@gmail.com");
            await Page.GetByPlaceholder("name@example.com").PressAsync("Tab");
            await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Password1!");
            await Page.GetByLabel("Password", new() { Exact = true }).PressAsync("Tab");
            await Page.GetByLabel("Confirm Password").FillAsync("Password1!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
            //login succescheck
            var myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            bool isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMyTimelineVisible);
            var logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Anonymous10]"});
            bool isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isLogOutVisible);
            //going to public timeline
            await Page.GetByRole(AriaRole.Link, new() { Name = "PUBLIC TIMELINE" }).ClickAsync();
            //checking if cheep1 is there
            var cheep1 = Page.GetByText("Follow Jacqualine Gilcoine - 01-08-2023 13:17:39 Starbuck now is what we hear").First;
            var isChee1pVisible = await cheep1.IsVisibleAsync();
            Console.WriteLine("isCheep1Visible: " + isChee1pVisible + (isChee1pVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isChee1pVisible);
            //checking if cheep2 is there
            var cheep2 =  Page.GetByText("Follow Jacqualine Gilcoine - 01-08-2023 13:16:57 In the morning of the wind,").First;
            var isCheep2Visible = await cheep2.IsVisibleAsync();
            Console.WriteLine("isCheep2Visible: " + isCheep2Visible + (isCheep2Visible ? " FAIL" : " PASS"));
            Assert.IsFalse(isCheep2Visible);
            
            //checking pagenumber
            var pagenum = Page.GetByText("◀ 1 ▶");
            var isPagenumVisible = await pagenum.IsVisibleAsync();
            Console.WriteLine("isPagenum1Visible: " + isPagenumVisible + (isPagenumVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isPagenumVisible);
            Console.WriteLine("going to page 2");
            await Page.GetByRole(AriaRole.Button, new() { Name = "▶" }).ClickAsync();
            //checking pagenumber
            pagenum = Page.GetByText("◀ 2 ▶");
            isPagenumVisible = await pagenum.IsVisibleAsync();
            Console.WriteLine("isPagenum2Visible: " + isPagenumVisible + (isPagenumVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isPagenumVisible);
            
            //checking if cheep1 is there
            cheep1 = Page.GetByText("Follow Jacqualine Gilcoine - 01-08-2023 13:17:39 Starbuck now is what we hear").First; 
            isChee1pVisible = await cheep1.IsVisibleAsync();
            Console.WriteLine("isCheep1Visible: " + isChee1pVisible + (isChee1pVisible ? " FAIL" : " PASS"));
            Assert.IsFalse(isChee1pVisible);
            //checking if cheep2 is there
            cheep2 =  Page.GetByText("Follow Jacqualine Gilcoine - 01-08-2023 13:16:57 In the morning of the wind,").First;
            isCheep2Visible = await cheep2.IsVisibleAsync();
            Console.WriteLine("isCheep2Visible: " + isCheep2Visible + (isCheep2Visible ? " PASS" : " FAIL"));
            Assert.IsTrue(isCheep2Visible);
        }
    }

    [Test]
    public async Task TestForgetMe()
    {
        if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
        {
            Assert.Ignore("Test ignored on GitHub Actions");
        }
        else
        {
            await Page.GotoAsync("http://localhost:5273/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "REGISTER" }).ClickAsync();
            await Page.GetByPlaceholder("username").ClickAsync();
            await Page.GetByPlaceholder("username").FillAsync("Anonymous11");
            await Page.GetByPlaceholder("username").PressAsync("Tab");
            await Page.GetByPlaceholder("name@example.com").FillAsync("Anonymous11@gmail.com");
            await Page.GetByPlaceholder("name@example.com").PressAsync("Tab");
            await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Password1!");
            await Page.GetByLabel("Password", new() { Exact = true }).PressAsync("Tab");
            await Page.GetByLabel("Confirm Password").FillAsync("Password1!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
            //login succescheck
            var myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            bool isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMyTimelineVisible);
            var logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Anonymous11]"});
            bool isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isLogOutVisible);
            //writing cheep for test
            await Page.Locator("#CheepDto_Text").ClickAsync();
            await Page.Locator("#CheepDto_Text").FillAsync("forget me");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();
            //checking if cheep was posted by Anonymous11
            var cheep = Page.GetByText("Anonymous11").First;
            var isCheepVisible = await cheep.IsVisibleAsync();
            Console.WriteLine("isCheepVisible: " + isCheepVisible + (isCheepVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isCheepVisible);
            //going to about me to be forgotten
            await Page.GetByRole(AriaRole.Link, new() { Name = "ABOUT ME" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Forget me!" }).ClickAsync();
            Console.WriteLine("After forget me has been pressed");
            //checking that it was loged out
            myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " FAIL" : " PASS"));
            Assert.IsFalse(isMyTimelineVisible);
            logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Anonymous11]"});
            isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " FAIL" : " PASS"));
            Assert.IsFalse(isLogOutVisible);
            
            //checking if there is a cheep posted by Anonymous11
            cheep = Page.GetByText("Anonymous11").First;
            isCheepVisible = await cheep.IsVisibleAsync();
            Console.WriteLine("isCheepVisible: " + isCheepVisible + (isCheepVisible ? " FAIL" : " PASS"));
            Assert.IsFalse(isCheepVisible);
            //checking if there is a cheep posted by a deleted acount
            cheep = Page.GetByText("[DELETED").First;
            isCheepVisible = await cheep.IsVisibleAsync();
            Console.WriteLine("isCheepVisible: " + isCheepVisible + (isCheepVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isCheepVisible);
            Console.WriteLine("trying to log back in to Anonymous11");
            //trying to login to acount Anonymous11
            await Page.GetByRole(AriaRole.Link, new() { Name = "LOGIN" }).ClickAsync();
            await Page.GetByPlaceholder("email/username").ClickAsync();
            await Page.GetByPlaceholder("email/username").FillAsync("Anonymous11");
            await Page.GetByPlaceholder("email/username").PressAsync("Tab");
            await Page.GetByPlaceholder("password").FillAsync("Password1!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
            var doesUserExist = Page.GetByText("username does not exist");
            bool isdoesUserExistVisible = await doesUserExist.IsVisibleAsync();
            Console.WriteLine("isdoesUserExistVisible: " + isdoesUserExistVisible + (isdoesUserExistVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isCheepVisible);
        }
    }
    
    
}