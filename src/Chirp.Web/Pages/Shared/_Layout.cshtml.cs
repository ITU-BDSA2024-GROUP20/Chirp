using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SQLitePCL;

namespace Chirp.Razor.Pages;

public class _Layout_cshtml : PageModel
{
    private readonly IAuthorRepository _service;
    
    public string UsernameLayout { get; set; }
    
    public _Layout_cshtml(IAuthorRepository service)
    {
        _service = service;
    }

    public ActionResult OnGet()
    {
        if (User.Identity.IsAuthenticated)
        {
            UsernameLayout = _service.GetAuthorDtoByEmail(User.Identity.Name).Name;
        }

        return Page();
    }

}