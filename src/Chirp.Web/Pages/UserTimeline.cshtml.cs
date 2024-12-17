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
        //gets the logged in user
        if (User.Identity is { IsAuthenticated: true })
        {
            var temp = AuthorService.GetAuthorDtoByEmail(User.Identity.Name);
            Username = temp.Name;
            Email = temp.Email;
        }
        //gets the current page
        if (!string.IsNullOrEmpty(Request.Query["page"]) && int.Parse( Request.Query["page"]!) > 0) 
            page =int.Parse( Request.Query["page"]!)-1;
        //finds the cheeps to be displayed
        List<CheepDTO> cheeps;
        if (User.Identity is { IsAuthenticated: true })
        {
            cheeps = AuthorService.AuthorCheep(page * 32, author, Email);
            nextPageExits = AuthorService.AuthorCheep((page+1) * 32, author, Email).Count > 0;
        }
        else
        {
            cheeps = AuthorService.AuthorCheep(page * 32, author, null);
            nextPageExits = AuthorService.AuthorCheep((page+1) * 32, author, null).Count > 0;
        }
        
        Cheeps = cheeps;
        
        
        return Page();
    }
}
