using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserProfileModel : PageModel
{
    public ICheepRepository _service;
    public Author Author { get; set; }
    public string Username { get; set; }
    public List<CheepDTO> Cheeps { get; set; }
    
    SignInManager<Author> _signInManager;
    public List<AuthorDTO> following { get; set; }
    public List<AuthorDTO> blocking { get; set; }
    public UserProfileModel(ICheepRepository service, SignInManager<Author> signInManager)
    {
        _signInManager = signInManager;
        _service = service;
    }


    public IActionResult OnGet()
    {
        if (User.Identity.IsAuthenticated)
        {
            Author = _service.GetAuthorByEmail(User.Identity.Name);
            Username = Author.Name;
            following = _service.GetFollowing(Username);
            blocking = _service.GetBlocking(Username);
        }
        else
        {
            return LocalRedirect("/");
        }
        
        List<CheepDTO> _Cheeps = _service.ReadCheep(0, Username, null);
        Cheeps = _Cheeps.TakeLast(32).ToList();
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostDeleteUser(string username)
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info != null)
        {
            await _signInManager.UserManager.RemoveLoginAsync(_service.GetAuthorByEmail(User.Identity.Name),
                info.LoginProvider, info.ProviderKey);
        }
        _service.DeleteAuthor(username); // Anon user
        await _signInManager.SignOutAsync(); // Log out user
        return RedirectToPage("Public"); // Go to main
    }
    
    public ActionResult OnPostToggleFollow(string self, string follow)
    {
        _service.ToggleFollow(self, follow);
        return RedirectToPage();
    }
    
    public ActionResult OnPostToggleBlock(string self, string follow)
    {
        _service.ToggleBlocking(self, follow);
        return RedirectToPage();
    }
}