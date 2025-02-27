// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Chirp.Web.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<Author> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        public string Username { get; set; }
        private readonly IAuthorRepository _service;

        public LogoutModel(SignInManager<Author> signInManager, ILogger<LogoutModel> logger, IAuthorRepository service)
        {
            _signInManager = signInManager;
            _logger = logger;
            _service = service;
        }

        public IActionResult OnGet()
        {
            //to get the logged in user this is need for layout to work
            if (User.Identity.IsAuthenticated)
            {
                Username = _service.GetAuthorDtoByEmail(User.Identity.Name).Name;
            }
            return Page();
        }
        
        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}
