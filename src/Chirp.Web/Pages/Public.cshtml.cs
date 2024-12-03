using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;
namespace Chirp.Razor.Pages;

public class PublicModel : PostablePage
{
    public PublicModel(ICheepRepository cheepService, IAuthorRepository authorRepository) : base(cheepService, authorRepository){}
    
    public ActionResult OnGet()
    {   
        
        if (User.Identity is { IsAuthenticated: true })
        {
            if (User.Identity.Name != null)
            {
                var temp = AuthorService.GetAuthorDtoByEmail(User.Identity.Name);
                Username = temp.Name;
                Email = temp.Email;
            }
        }

        if (!string.IsNullOrEmpty(Request.Query["page"]) && Int32.Parse( Request.Query["page"]! ) > 0)
        {
            page =Int32.Parse( Request.Query["page"]!)-1;
        }
        
        
        List<CheepDTO> cheeps;
        if (User.Identity is { IsAuthenticated: true })
        {
            cheeps = CheepService.ReadCheep( page*32 ,null, Email);
        }
        else
        {
            cheeps = CheepService.ReadCheep( page*32 ,null, null);
        }
        
        
        
        
        Cheeps = cheeps.TakeLast(32).ToList();
        
        
        return Page();
    }
}
