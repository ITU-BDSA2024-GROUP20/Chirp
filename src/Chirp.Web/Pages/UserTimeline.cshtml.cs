﻿using AspNet.Security.OAuth.GitHub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V5.Pages.Account.Internal;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PostablePage
{
    
    public UserTimelineModel(ICheepRepository cheepService, IAuthorRepository authorService) : base(cheepService, authorService)
    {
    }

    public ActionResult OnGet(string author)
    {
        if (User.Identity.IsAuthenticated)
        {
            var temp = _authorService.GetAuthorDtoByEmail(User.Identity.Name);
            Username = temp.Name;
            email = temp.Email;
        }
        if (!string.IsNullOrEmpty(Request.Query["page"]) && Int32.Parse( Request.Query["page"]) > 0) 
            page =Int32.Parse( Request.Query["page"])-1;
        List<CheepDTO> _Cheeps = _authorService.AuthorCheep(page * 32, author, null);
        if (User.Identity.IsAuthenticated)
        { 
            _Cheeps = _authorService.AuthorCheep(page * 32, author, email);
        }
        else
        {
            _Cheeps = _authorService.AuthorCheep(page * 32, author, null);
        }
        
        
        Cheeps = _Cheeps.TakeLast(32).ToList();
        
        
        return Page();
    }
}
