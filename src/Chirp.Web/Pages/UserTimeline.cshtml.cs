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
    
    public UserTimelineModel(ICheepRepository cheepService, IAuthorRepository authorService) : base(cheepService, authorService)
    {
    }

    public ActionResult OnGet(string? author)
    {
        if (User.Identity is { IsAuthenticated: true })
        {
            var temp = AuthorService.GetAuthorDtoByEmail(User.Identity.Name);
            Username = temp.Name;
            Email = temp.Email;
        }
        if (!string.IsNullOrEmpty(Request.Query["page"]) && int.Parse( Request.Query["page"]!) > 0) 
            page =int.Parse( Request.Query["page"]!)-1;
        var cheeps = AuthorService.AuthorCheep(page * 32, author, User.Identity is { IsAuthenticated: true } ? Email : null);


        Cheeps = cheeps.TakeLast(32).ToList();
        
        
        return Page();
    }
}
