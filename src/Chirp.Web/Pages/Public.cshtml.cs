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
        //to get the person who is logged in
        if (User.Identity is { IsAuthenticated: true })
        {
            if (User.Identity.Name != null)
            {
                var temp = AuthorService.GetAuthorDtoByEmail(User.Identity.Name);
                Username = temp.Name;
                Email = temp.Email;
            }
        }
        //to get the page
        if (!string.IsNullOrEmpty(Request.Query["page"]) && Int32.Parse( Request.Query["page"]! ) > 0)
        {
            page =Int32.Parse( Request.Query["page"]!)-1;
        }
        
        //to get the cheep that go on the page
        List<CheepDTO> cheeps;
        if (User.Identity is { IsAuthenticated: true })
        {
            cheeps = CheepService.ReadCheep( page*32 ,null, Email);
            nextPageExits = CheepService.ReadCheep((page+1) * 32, null, Email).Count != 0;
        }
        else
        {
            cheeps = CheepService.ReadCheep( page*32 ,null, null);
            nextPageExits = CheepService.ReadCheep((page+1) * 32, null, null).Count != 0;
        }
        

        Cheeps = cheeps;
        
        
        return Page();
    }
}
