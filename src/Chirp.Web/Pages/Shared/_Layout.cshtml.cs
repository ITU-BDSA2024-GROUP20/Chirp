using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SQLitePCL;

namespace Chirp.Razor.Pages;

public class _Layout_cshtml : PageModel
{
    private readonly ICheepRepository _service;
    
    public string UsernameLayout { get; set; }
    
    public _Layout_cshtml(ICheepRepository service)
    {
        _service = service;
    }

    public ActionResult OnGet()
    {
        if (User.Identity.IsAuthenticated)
        {
            UsernameLayout = _service.GetAuthorByEmail(User.Identity.Name).Name;
        }

        return Page();
    }

}