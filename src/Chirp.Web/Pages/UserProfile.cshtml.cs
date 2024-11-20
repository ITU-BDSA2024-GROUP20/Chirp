using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserProfileModel : PageModel
{
    ICheepRepository _service;
    private Author Author { get; set; }
    public string Username { get; set; }
    
    public UserProfileModel(ICheepRepository service)
    {
        _service = service;
    }


    public IActionResult OnGet()
    {
        if (User.Identity.IsAuthenticated)
        {
            Author = _service.GetAuthorByEmail(User.Identity.Name);
            Username = Author.Name;
        }
        else
        {
            return LocalRedirect("/");
        }
        
        
        
        return Page();
    }
    
    
}