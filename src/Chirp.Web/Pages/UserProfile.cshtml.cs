using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserProfileModel : PageModel
{
    public IAuthorRepository _service;
    public AuthorDTO AuthorDTO { get; set; }
    public string Username { get; set; }
    public List<CheepDTO> Cheeps { get; set; }
    
    SignInManager<Author> _signInManager;
    public List<AuthorDTO> following { get; set; }
    
    public UserProfileModel(IAuthorRepository service, SignInManager<Author> signInManager)
    {
        _signInManager = signInManager;
        _service = service;
    }


    public IActionResult OnGet()
    {
        if (User.Identity.IsAuthenticated)
        {
            AuthorDTO = _service.GetAuthorDtoByEmail(User.Identity.Name);
            Username = AuthorDTO.Name;
            following = _service.GetFollowing(Username);
        }
        else
        {
            return LocalRedirect("/");
        }
        
        List<CheepDTO> _Cheeps = _service.AuthorCheep(0, Username, null);
        Cheeps = _Cheeps.TakeLast(32).ToList();
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostDeleteUser(string username)
    {
        _service.DeleteAuthor(username); // Anon user
        await _signInManager.SignOutAsync(); // Log out user
        return RedirectToPage("Public"); // Go to main
    }
    
    public ActionResult OnPostToggleFollow(string self, string follow)
    {
        _service.ToggleFollow(self, follow);
        return RedirectToPage();
    }
}