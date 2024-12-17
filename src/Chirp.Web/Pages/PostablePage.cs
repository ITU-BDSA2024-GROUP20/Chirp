using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;
/// <summary>
/// the base page model for the user and public timelines
/// </summary>
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
    /// <summary>
    /// on call this send the text and userinfo to make a new cheep
    /// </summary>
    /// <returns></returns>
    public ActionResult OnPost()
    {
        Console.WriteLine(CheepDto.Text);
        CheepDto.Author = Username;
        if (User.Identity != null) CheepDto.Email = User.Identity.Name;
        CheepDto.Timestamp = DateTime.UtcNow.AddHours(1).ToString();
        
        CheepService.CreateCheep(CheepDto);
        return RedirectToPage();
    }
    
    /// <summary>
    /// this is a function called buy a button that set the person that presses the button as following/unfollowing
    /// the person the button is associated with
    /// </summary>
    /// <param name="self"></param> the person who presses the buttons email
    /// <param name="follow"></param> email of the person to be followed/unfollowed
    /// <param name="page"></param> the current page
    /// <returns></returns>
    public ActionResult OnPostToggleFollow(string? self, string? follow, int page)
    {
        AuthorService.ToggleFollow(self, follow);
        return Redirect($"?page={page + 1}");
    }
    
    /// <summary>
    /// this is a function called buy a button that set the person that presses the button as blocking/unblocking
    /// the person the button is associated with
    /// </summary>
    /// <param name="self"></param> the person who presses the buttons email
    /// <param name="follow"></param> email of the person to be blocked/unblocked
    /// <returns></returns>
    public ActionResult OnPostToggleBlocking(string? self, string? follow)
    {
        AuthorService.ToggleBlocking(self, follow);
        return RedirectToPage();
    }
    
}