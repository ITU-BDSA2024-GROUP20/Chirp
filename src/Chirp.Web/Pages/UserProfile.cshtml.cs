using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Chirp.Razor.Pages;

public class UserProfileModel : PageModel
{
    public IAuthorRepository _service;
    
    public string Username { get; set; }
    public string Email { get; set; }
    public List<CheepDTO> Cheeps { get; set; }
    
    SignInManager<Author> _signInManager;
    public List<AuthorDTO> following { get; set; }
    public List<AuthorDTO> blocking { get; set; }
    
    public UserProfileModel(IAuthorRepository service, SignInManager<Author> signInManager)
    {
        _signInManager = signInManager;
        _service = service;
    }


    public IActionResult OnGet()
    {
        if (User.Identity.IsAuthenticated)
        {
            var authorDto = _service.GetAuthorDtoByEmail(User.Identity.Name);
            Username = authorDto.Name;
            Email = authorDto.Email;
            following = _service.GetFollowing(authorDto.Email);
            blocking = _service.GetBlocking(authorDto.Email);
        }
        else
        {
            return LocalRedirect("/");
        }
        Console.WriteLine();
        List<CheepDTO> _Cheeps = _service.AuthorCheep(0, Email, null);
        Cheeps = _Cheeps.TakeLast(32).ToList();
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostDeleteUser(string email)
    {
        _service.DeleteAuthor(email); // Anon user
        await _signInManager.SignOutAsync(); // Log out user
        return RedirectToPage("Public"); // Go to main
    }
    
    public ActionResult OnPostToggleFollow(string self, string follow)
    {
        _service.ToggleFollow(self, follow);
        return RedirectToPage();
    }
    public ActionResult OnPostToggleBlocking(string self, string follow)
    {
        _service.ToggleBlocking(self, follow);
        return RedirectToPage();
    }
}