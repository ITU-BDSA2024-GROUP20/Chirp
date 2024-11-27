using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Scripting.Hosting;

namespace Chirp.Razor.Pages;

public class PostablePage : PageModel
{
    public ICheepRepository _service;
    public List<CheepDTO> Cheeps { get; set; }
    
    public String Username { get; set; }
    
    [BindProperty]
    public CheepDTO cheepDTO { get; set; }
    public int page = 0;

    public PostablePage(ICheepRepository service)
    {
        _service = service;
    }
    
    public ActionResult OnPost()
    {
        cheepDTO.Author = Username;
        cheepDTO.Email = User.Identity.Name;
        cheepDTO.Timestamp = DateTime.UtcNow.AddHours(1).ToString();
        
        _service.CreateCheep(cheepDTO);
        return RedirectToPage();
    }

    public ActionResult OnPostToggleFollow(string self, string follow)
    {
        _service.ToggleFollow(self, follow);
        return RedirectToPage();
    }
    
    public ActionResult OnPostToggleBlock(string self, string follow)
    {
        _service.ToggleBlocking(self, follow);
        return RedirectToPage();
    }
}