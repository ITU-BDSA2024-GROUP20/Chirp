using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PostablePage
{
    public UserTimelineModel(ICheepRepository service) : base(service){}

    public ActionResult OnGet(string author)
    {
        if (User.Identity.IsAuthenticated)
        {
            Username = _service.GetAuthorByEmail(User.Identity.Name).Name;
        }
        if (!string.IsNullOrEmpty(Request.Query["page"]) && Int32.Parse( Request.Query["page"]) > 0) 
            page =Int32.Parse( Request.Query["page"])-1;
        
        List<CheepDTO> _Cheeps = _service.ReadCheep(page * 32, author);
        Cheeps = _Cheeps.TakeLast(32).ToList();
        
        
        return Page();
    }
}
