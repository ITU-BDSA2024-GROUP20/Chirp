using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;
namespace Chirp.Razor.Pages;

public class PublicModel : PostablePage
{
    public PublicModel(ICheepRepository service) : base(service){}
    
    public ActionResult OnGet()
    {   
        
        if (User.Identity.IsAuthenticated)
        {
            Username = _service.GetAuthorByEmail(User.Identity.Name).Name;
        }
        if (!string.IsNullOrEmpty(Request.Query["page"]) && Int32.Parse( Request.Query["page"]) > 0) 
            page =Int32.Parse( Request.Query["page"])-1;
        
        List<CheepDTO> _Cheeps = _service.ReadCheep( page*32 ,null);;
        Cheeps = _Cheeps.TakeLast(32).ToList();
        
        
        return Page();
    }
}
