using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Scripting.Hosting;

namespace Chirp.Razor.Pages;

public class PostablePage : PageModel
{
    public ICheepRepository _cheepService;
    public IAuthorRepository _authorService;
    public List<CheepDTO> Cheeps { get; set; }
    
    public String Username { get; set; }
    
    [BindProperty]
    public CheepDTO cheepDTO { get; set; }
    public int page = 0;

    public PostablePage(ICheepRepository cheepService, IAuthorRepository authorService)
    {
        _cheepService = cheepService;
        _authorService = authorService;
    }
    
    public ActionResult OnPost()
    {
        cheepDTO.Author = Username;
        cheepDTO.Email = User.Identity.Name;
        cheepDTO.Timestamp = DateTime.UtcNow.AddHours(1).ToString();
        
        _cheepService.CreateCheep(cheepDTO);
        return RedirectToPage();
    }

    public ActionResult OnPostToggleFollow(string self, string follow)
    {
        _authorService.ToggleFollow(self, follow);
        return RedirectToPage();
    }
    
    
    public ActionResult OnPostToggleBlocking(string self, string follow)
    {
        _authorService.ToggleBlocking(self, follow);
        return RedirectToPage();
    }
    
}