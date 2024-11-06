using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _service;
    public List<CheepDTO> Cheeps { get; set; }
    public int page = 0;
    
    public string Username { get; set; }

    public UserTimelineModel(ICheepRepository service)
    {
        _service = service;
    }

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
