using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SQLitePCL;

namespace Chirp.Razor.Pages;

public class _Layout_cshtml : PageModel
{
    //this page is an artifact of some earlier test we have not deleted
    private readonly IAuthorRepository _service;
    
    public string? UsernameLayout { get; set; } = null!;

    public _Layout_cshtml(IAuthorRepository service)
    {
        _service = service;
    }

    public ActionResult OnGet()
    {
        if (User.Identity is { IsAuthenticated: true })
        {
            UsernameLayout = _service.GetAuthorDtoByEmail(User.Identity.Name).Name;
        }

        return Page();
    }

}