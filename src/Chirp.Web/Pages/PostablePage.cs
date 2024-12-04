using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PostablePage : PageModel
{
    public ICheepRepository CheepService;
    public IAuthorRepository AuthorService;
    public List<CheepDTO> Cheeps { get; set; } = null!;

    public string? Username { get; set; }
    public string? Email { get; set; }
    
    [BindProperty]
    public CheepDTO CheepDto { get; set; } = null!;

    public int page = 0;
    public bool nextPageExits;

    public PostablePage (ICheepRepository cheepService, IAuthorRepository authorService)
    {
        CheepService = cheepService;
        AuthorService = authorService;
    }
    
    public ActionResult OnPost()
    {
        Console.WriteLine(CheepDto.Text);
        CheepDto.Author = Username;
        if (User.Identity != null) CheepDto.Email = User.Identity.Name;
        CheepDto.Timestamp = DateTime.UtcNow.AddHours(1).ToString();
        
        CheepService.CreateCheep(CheepDto);
        return RedirectToPage();
    }

    public ActionResult OnPostToggleFollow(string? self, string? follow, int page)
    {
        AuthorService.ToggleFollow(self, follow);
        return Redirect($"?page={page + 1}");
    }
    
    
    public ActionResult OnPostToggleBlocking(string? self, string? follow)
    {
        AuthorService.ToggleBlocking(self, follow);
        return RedirectToPage();
    }
    
}