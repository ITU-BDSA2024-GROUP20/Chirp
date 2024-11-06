using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;
namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _service;
    public List<CheepDTO> Cheeps { get; set; }
    
    public String Username { get; set; }
    
    [BindProperty]
    public CheepDTO cheepDTO { get; set; }
    int page = 0;

    public PublicModel(ICheepRepository service)
    {
        _service = service;
    }
    
    public ActionResult OnGet()
    {   
        if (User.Identity.IsAuthenticated)
        {
            Username = _service.GetAuthorByEmail(User.Identity.Name).Name;
        }
        if (!string.IsNullOrEmpty(Request.Query["page"]) && Int32.Parse( Request.Query["page"]) > 0) 
            page =Int32.Parse( Request.Query["page"])-1;
        
        List<CheepDTO> _Cheeps = _service.ReadCheep( page*32 ,null);;
        Cheeps = _Cheeps.TakeLast(32).ToList();
        
        
        return Page();
    }
    
    public ActionResult OnPost()
    {
        cheepDTO.Author = Username;
        cheepDTO.Email = User.Identity.Name;
        cheepDTO.Timestamp = DateTime.UtcNow.AddHours(1).ToString();
        
        _service.CreateCheep(cheepDTO);
        return RedirectToPage("Public");
    }
}
