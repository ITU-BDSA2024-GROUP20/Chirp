namespace Chirp.Web.Test;
using Microsoft.Playwright;
//[Parallelizable(ParallelScope.Self)]
//[TestFixture]
public class endtoend
{
    
    /* 
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
    public async Task EndtoEnd()
    {
        if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
        {
            Assert.Ignore("Test ignored on GitHub Actions");
        }
        else
        {
            Console.WriteLine("EndToEndtest:");
            await Page.GotoAsync("http://localhost:5273/");
            
            Console.WriteLine("Registering with github");
            await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "GitHub" }).ClickAsync();
            await Page.GetByLabel("Username or email address").ClickAsync();
            await Page.GetByLabel("Username or email address").FillAsync("dummyaccount20");
            await Page.GetByLabel("Password").ClickAsync();
            await Page.GetByLabel("Password").FillAsync("Chirpdummy1");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Sign in", Exact = true }).ClickAsync();
            Console.WriteLine("Sign in in progress");
            //if first time login in with the acount uncomment this
            //await Page.GetByRole(AriaRole.Button, new() { Name = "Authorize ITU-BDSA2024-GROUP20" }).ClickAsync();
            
            await Page.WaitForURLAsync(url => url.StartsWith("http://localhost:5273/Identity/Account/ExternalLogin"));
            await Page.GetByLabel("Email").ClickAsync();
            await Page.GetByLabel("Email").FillAsync("dummyaccount20@gmail.com");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
            Console.WriteLine("Sign in in Complete");
            //logedin checks
            var myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            var isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMyTimelineVisible);
            var logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Chirp20]"});
            var isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isLogOutVisible);
            
            await Page.GetByRole(AriaRole.Link, new() { Name = "LOGOUT [Chirp20]" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Click here to Logout" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync(); 
            //checking that it was loged out
            myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " FAIL" : " PASS"));
            Assert.IsFalse(isMyTimelineVisible);
            logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [Chirp20]"});
            isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " FAIL" : " PASS"));
            Assert.IsFalse(isLogOutVisible);
            
            
            
            
            
            await Page.GetByRole(AriaRole.Link, new() { Name = "REGISTER" }).ClickAsync();
            await Page.Locator("#registerForm div").Filter(new() { HasText = "Username" }).ClickAsync();
            await Page.GetByPlaceholder("username").ClickAsync();
            await Page.GetByPlaceholder("username").FillAsync("EndToEnd");
            await Page.GetByPlaceholder("username").PressAsync("Tab");
            await Page.GetByPlaceholder("name@example.com").FillAsync("EndToEnd@gmail.com");
            await Page.GetByPlaceholder("name@example.com").PressAsync("Tab");
            await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Password1!");
            await Page.GetByLabel("Password", new() { Exact = true }).PressAsync("Tab");
            await Page.GetByLabel("Confirm Password").FillAsync("Password1!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
            //logedin checks
            myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isMyTimelineVisible);
            logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [EndToEnd]"});
            isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isLogOutVisible);
            
            Console.WriteLine("writing a cheep");
            await Page.Locator("#CheepDto_Text").ClickAsync();
            await Page.Locator("#CheepDto_Text").FillAsync("test cheep");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();
            //checking if cheep was posted by Anonymous11
            var cheep = Page.GetByText("test cheep").First;
            var isCheepVisible = await cheep.IsVisibleAsync();
            Console.WriteLine("isCheepVisible: " + isCheepVisible + (isCheepVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isCheepVisible);
            
            await Page.GetByRole(AriaRole.Link, new() { Name = "PUBLIC TIMELINE" }).ClickAsync();
            await Page.Locator("li").Filter(new() { HasText = "Follow Jacqualine Gilcoine - 01-08-2023 13:17:39 Starbuck now is what we hear" }).GetByRole(AriaRole.Button).ClickAsync();
            await Page.Locator("li").Filter(new() { HasText = "Follow Mellie Yost - 01-08-" }).GetByRole(AriaRole.Button).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "MY TIMELINE" }).ClickAsync(); 
            Console.WriteLine("going to my timeline after following"); 
            //checking if Jacqualine is on EndToEnd private timeline
            var getJacqualine = Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine" }).First.GetByRole(AriaRole.Button);
            bool isJacqualineVisible = await getJacqualine.IsVisibleAsync(); 
            Console.WriteLine("isJacqualineVisible: " + isJacqualineVisible + (isJacqualineVisible ? " PASS" : " FAIL")); 
            Assert.IsTrue(isJacqualineVisible); 
            //checking if Mellie is on EndToEnd private timeline
            var getMellieYost = Page.Locator("li").Filter(new (){ HasText = "Mellie Yost" }).GetByRole(AriaRole.Link).First;
            bool isMellieYostVisible = await getMellieYost.IsVisibleAsync();
            Console.WriteLine("isMellieYostVisible: " + isMellieYostVisible + (isMellieYostVisible ? " PASS" : " FAIL")); 
            Assert.IsTrue(isMellieYostVisible);
            
            Console.WriteLine("unfollowing Mellie");
            await Page.Locator("li").Filter(new() { HasText = "UnFollow Mellie Yost — 01-08-2023 13:17:33 But what was behind the barricade." }).GetByRole(AriaRole.Button).ClickAsync();
            //checking if Jacqualine is on EndToEnd private timeline
            getJacqualine = Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine" }).First.GetByRole(AriaRole.Button);
            isJacqualineVisible = await getJacqualine.IsVisibleAsync(); 
            Console.WriteLine("isJacqualineVisible: " + isJacqualineVisible + (isJacqualineVisible ? " PASS" : " FAIL")); 
            Assert.IsTrue(isJacqualineVisible); 
            //checking if Mellie is on EndToEnd private timeline
            getMellieYost = Page.Locator("li").Filter(new (){ HasText = "Mellie Yost" }).GetByRole(AriaRole.Link).First;
            isMellieYostVisible = await getMellieYost.IsVisibleAsync();
            Console.WriteLine("isMellieYostVisible: " + isMellieYostVisible + (isMellieYostVisible ? " FAIL" : " PASS")); 
            Assert.IsFalse(isMellieYostVisible);
            
            
            Console.WriteLine("Blocking Jacqualine");
            await Page.Locator("strong").Filter(new() { HasText = "Jacqualine Gilcoine — 01-08-2023 13:17:39" }).GetByRole(AriaRole.Link).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Block" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "MY TIMELINE" }).ClickAsync();
            //checking if Jacqualine is on EndToEnd private timeline
            getJacqualine = Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine" }).First.GetByRole(AriaRole.Button);
            isJacqualineVisible = await getJacqualine.IsVisibleAsync(); 
            Console.WriteLine("isJacqualineVisible: " + isJacqualineVisible + (isJacqualineVisible ? " FAIL" : " PASS")); 
            Assert.IsFalse(isJacqualineVisible); 
            
            
            await Page.GetByRole(AriaRole.Link, new() { Name = "PUBLIC TIMELINE" }).ClickAsync();
            //checking if Jacqualine is on EndToEnd public timeline
            getJacqualine = Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine" }).First.GetByRole(AriaRole.Button);
            isJacqualineVisible = await getJacqualine.IsVisibleAsync(); 
            Console.WriteLine("isJacqualineVisible: " + isJacqualineVisible + (isJacqualineVisible ? " FAIL" : " PASS")); 
            Assert.IsFalse(isJacqualineVisible); 
            
            
            Console.WriteLine("going to AboutMe to unblock Jacqualine");
            await Page.GetByRole(AriaRole.Link, new() { Name = "ABOUT ME" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "UnBlock" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "PUBLIC TIMELINE" }).ClickAsync();
            //checking if Jacqualine is on EndToEnd public timeline
            getJacqualine = Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine" }).First.GetByRole(AriaRole.Button);
            isJacqualineVisible = await getJacqualine.IsVisibleAsync(); 
            Console.WriteLine("isJacqualineVisible: " + isJacqualineVisible + (isJacqualineVisible ? " PASS" : " FAIL")); 
            Assert.IsTrue(isJacqualineVisible); 
            
            Console.WriteLine("pagnation");
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
            
            
            Console.WriteLine("going to forget me");
            await Page.GetByRole(AriaRole.Link, new() { Name = "ABOUT ME" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Forget me!" }).ClickAsync();
            Console.WriteLine("After forget me has been pressed");
            //checking that it was loged out
            myTimeline = Page.GetByRole(AriaRole.Link, new(){Name = "my timeline"});
            isMyTimelineVisible = await myTimeline.IsVisibleAsync();
            Console.WriteLine("IsMyTimelineVisible: " + isMyTimelineVisible + (isMyTimelineVisible ? " FAIL" : " PASS"));
            Assert.IsFalse(isMyTimelineVisible);
            logout = Page.GetByRole(AriaRole.Link, new(){Name = "logout [EndToEnd]"});
            isLogOutVisible = await logout.IsVisibleAsync();
            Console.WriteLine("isLogOutVisible: " + isLogOutVisible + (isLogOutVisible ? " FAIL" : " PASS"));
            Assert.IsFalse(isLogOutVisible);
            
            //checking if there is a cheep posted by EndToEnd
            cheep = Page.GetByText("EndToEnd").First;
            isCheepVisible = await cheep.IsVisibleAsync();
            Console.WriteLine("isCheepVisible: " + isCheepVisible + (isCheepVisible ? " FAIL" : " PASS"));
            Assert.IsFalse(isCheepVisible);
            //checking if there is a cheep posted by a deleted acount
            cheep = Page.GetByText("[DELETED").First;
            isCheepVisible = await cheep.IsVisibleAsync();
            Console.WriteLine("isCheepVisible: " + isCheepVisible + (isCheepVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isCheepVisible);
            
            
            
            Console.WriteLine("trying to log back in to EndToEnd");
            //trying to login to acount EndToEnd
            await Page.GetByRole(AriaRole.Link, new() { Name = "LOGIN" }).ClickAsync();
            await Page.GetByPlaceholder("email/username").ClickAsync();
            await Page.GetByPlaceholder("email/username").FillAsync("EndToEnd");
            await Page.GetByPlaceholder("email/username").PressAsync("Tab");
            await Page.GetByPlaceholder("password").FillAsync("Password1!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
            var doesUserExist = Page.GetByText("username does not exist");
            bool isdoesUserExistVisible = await doesUserExist.IsVisibleAsync();
            Console.WriteLine("isdoesUserExistVisible: " + isdoesUserExistVisible + (isdoesUserExistVisible ? " PASS" : " FAIL"));
            Assert.IsTrue(isdoesUserExistVisible);
            
        }
    }*/
}