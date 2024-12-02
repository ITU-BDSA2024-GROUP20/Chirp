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
        
        if (User.Identity.IsAuthenticated)
        {
            Username = _authorService.GetAuthorDtoByEmail(User.Identity.Name).Name;
        }

        List<CheepDTO> _Cheeps;
        if (User.Identity.IsAuthenticated)
        {
            _Cheeps = _cheepService.ReadCheep( page*32 ,null, Username);
        }
        else
        {
            _Cheeps = _cheepService.ReadCheep( page*32 ,null, null);
        }
        
        if (!string.IsNullOrEmpty(Request.Query["page"]) && Int32.Parse( Request.Query["page"]) > 0) 
            page =Int32.Parse( Request.Query["page"])-1;
        
        
        Cheeps = _Cheeps.TakeLast(32).ToList();
        
        
        return Page();
    }
}
