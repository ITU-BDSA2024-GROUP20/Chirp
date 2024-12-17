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
        //getting the person who is logged in
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
        //return the cheep of the logged in person   
        List<CheepDTO> _Cheeps = Service.AuthorCheep(0, Email, null);
        Cheeps = _Cheeps.TakeLast(32).ToList();
        
        return Page();
    }
    /// <summary>
    /// this anonymises the user and makes one unable to log in to the account
    /// </summary>
    /// <param name="email"></param> email of the person to be deleted
    /// <returns></returns>
    public async Task<IActionResult> OnPostDeleteUser(string email)
    {
        var info = await _signInManager.GetExternalLoginInfoAsync(); 
        Author author = Service.DeleteAuthor(email); // Anon user
        if (info != null)
        {
            await _signInManager.UserManager.RemoveLoginAsync(author,
                info.LoginProvider, info.ProviderKey);
        }
        Service.save();
        
        await _signInManager.SignOutAsync(); // Log out user
        return RedirectToPage("Public"); // Go to main
    }
    /// <summary>
    /// this is a function called buy a button that set the person that presses the button as unfollowing
    /// the person the button is associated with
    /// </summary>
    /// <param name="self"></param> the person who presses the buttons email
    /// <param name="follow"></param> email of the person to be unfollowed
    /// <returns></returns>
    public ActionResult OnPostToggleFollow(string? self, string? follow)
    {
        Service.ToggleFollow(self, follow);
        return RedirectToPage();
    }
    /// <summary>
    /// this is a function called buy a button that set the person that presses the button as unblocking
    /// the person the button is associated with
    /// </summary>
    /// <param name="self"></param> the person who presses the buttons email
    /// <param name="follow"></param> email of the person to be unblocked
    /// <returns></returns>
    public ActionResult OnPostToggleBlocking(string? self, string? follow)
    {
        Service.ToggleBlocking(self, follow);
        return RedirectToPage();
    }
}