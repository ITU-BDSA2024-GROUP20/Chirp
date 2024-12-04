using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Chirp.Razor.Pages;

public class UserProfileModel : PageModel
{

    public IAuthorRepository Service;
    public string? Username { get; set; }
    public string? Email { get; set; }
    public List<CheepDTO> Cheeps { get; set; } = null!;

    SignInManager<Author> _signInManager;
    public List<AuthorDTO> Following { get; set; } = null!;
    public List<AuthorDTO> Blocking { get; set; } = null!;

    public UserProfileModel(IAuthorRepository service, SignInManager<Author> signInManager)
    {
        _signInManager = signInManager;
        Service = service;
    }


    public IActionResult OnGet()
    {
        if (User.Identity is { IsAuthenticated: true })
        {
            var authorDto = Service.GetAuthorDtoByEmail(User.Identity.Name);
            Username = authorDto.Name;
            Email = authorDto.Email;
            Following = Service.GetFollowing(authorDto.Email);
            Blocking = Service.GetBlocking(authorDto.Email);

        }
        else
        {
            return LocalRedirect("/");
        }
        Console.WriteLine();
        List<CheepDTO> _Cheeps = Service.AuthorCheep(0, Email, null);
        Cheeps = _Cheeps.TakeLast(32).ToList();
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostDeleteUser(string email)
    {
        Service.DeleteAuthor(email); // Anon user
        await _signInManager.SignOutAsync(); // Log out user
        return RedirectToPage("Public"); // Go to main
    }
    
    public ActionResult OnPostToggleFollow(string? self, string? follow)
    {
        Service.ToggleFollow(self, follow);
        return RedirectToPage();
    }
    public ActionResult OnPostToggleBlocking(string? self, string? follow)
    {
        Service.ToggleBlocking(self, follow);
        return RedirectToPage();
    }
}