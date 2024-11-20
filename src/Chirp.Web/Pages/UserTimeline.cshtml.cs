using AspNet.Security.OAuth.GitHub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V5.Pages.Account.Internal;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PostablePage
{
    SignInManager<Author> _signInManager;
    UserManager<Author> _userManager;
    public UserTimelineModel(ICheepRepository service, SignInManager<Author> signInManager, UserManager<Author> userManager) : base(service)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public ActionResult OnGet(string author)
    {
        if (User.Identity.IsAuthenticated)
        {
            Username = _service.GetAuthorByEmail(User.Identity.Name).Name;
        }
        if (!string.IsNullOrEmpty(Request.Query["page"]) && Int32.Parse( Request.Query["page"]) > 0) 
            page =Int32.Parse( Request.Query["page"])-1;
        
        List<CheepDTO> _Cheeps = _service.ReadCheep(page * 32, author, Username);
        Cheeps = _Cheeps.TakeLast(32).ToList();
        
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostDeleteUser(string username)
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        await _signInManager.UserManager.RemoveLoginAsync(_service.GetAuthorByEmail(User.Identity.Name),
            info.LoginProvider, info.ProviderKey);
        _service.DeleteAuthor(username); // Anon user
        await _signInManager.SignOutAsync(); // Log out user
        return RedirectToPage("Public"); // Go to main
    }
}
