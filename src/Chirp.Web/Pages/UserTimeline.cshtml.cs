using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepDTO> Cheeps { get; set; }
    public int page = 0;

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet(string author)
    {
        if (!string.IsNullOrEmpty(Request.Query["page"]) && Int32.Parse( Request.Query["page"]) > 0) 
            page =Int32.Parse( Request.Query["page"])-1;
        
        List<CheepDTO> _Cheeps = _service.GetCheepsFromAuthor(author, page * 32);
        Cheeps = _Cheeps.TakeLast(32).ToList();
        
        
        return Page();
    }
}
