// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Chirp.Infrastructure;

namespace Chirp.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<Author> _signInManager;
        private readonly UserManager<Author> _userManager;
        private readonly IUserStore<Author> _userStore;
        private readonly IUserEmailStore<Author> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;
        
        private readonly IAuthorRepository _service;
        public string Username { get; set; }
        public bool UsernameTaken { get; set; }
        public bool EmailTaken { get; set; }
        

        public ExternalLoginModel(
            SignInManager<Author> signInManager,
            UserManager<Author> userManager,
            IUserStore<Author> userStore,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender,
            IAuthorRepository service)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
            _emailSender = emailSender;
            _service = service;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ProviderDisplayName { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [DataType(DataType.Text)]
            [StringLength(25, ErrorMessage = "must be at least 2 characters long and can't be longer than 25.", MinimumLength = 2)]
            [RegularExpression(@"^[^@]*$", ErrorMessage = "The username cannot contain the '@' symbol.")]
            [Display(Name = "Username")]
            public string Username { get; set; }
            
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [EmailAddress]
            public string Email { get; set; }
            
            public bool UsernameTakenIn { get; set; }
            public bool EmailTakenIn { get; set; }
        }
        

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                Username = _service.GetAuthorDtoByEmail(User.Identity.Name).Name;
            }
            return RedirectToPage("./Login");
            
        }
        
        
        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else if (info.Principal.Identity.Name != null &&   info.Principal.FindFirstValue(ClaimTypes.Email) != null)
            {
                var user = CreateUser();
                bool reload = false;
                var existingUserByName = _service.GetAuthorDtoByName(info.Principal.Identity.Name);
                if (existingUserByName.Name != null && existingUserByName.Email != null)
                {
                    ModelState.AddModelError(string.Empty, "The username is already taken.");
                    UsernameTaken = true;
                    reload = true;
                }
                var existingUserByEmail = _service.GetAuthorDtoByEmail(info.Principal.FindFirstValue(ClaimTypes.Email));
                if (existingUserByEmail.Name != null && existingUserByEmail.Email != null)
                {
                    ModelState.AddModelError(string.Empty, "The Email is already taken.");
                    EmailTaken = true;
                    reload = true;
                }
                if (reload)
                {
                    return Page();
                }
                
                user.Name = info.Principal.Identity.Name;
                user.Cheeps = new List<Cheep>();
                user.Following = new List<Author>();
                user.Blocking = new List<Author>();
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                //Console.WriteLine(info.Principal.Identity.Name + " " + info.Principal.FindFirstValue(ClaimTypes.Email));
                await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, email, CancellationToken.None);

                var resultLogin = await _userManager.CreateAsync(user);
                if (resultLogin.Succeeded)
                {
                    resultLogin = await _userManager.AddLoginAsync(user, info);
                    if (resultLogin.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("./RegisterConfirmation", new { Email = email });
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                        if (returnUrl == "/")
                        {
                            returnUrl = "/"+user.Name;
                        }
                        //Console.WriteLine(returnUrl+"    shfisanfpajfoisaiofjsafjpsajfpoaojsajfjsjofajpofjsafjpfoajfoajfopsjafsaposajfjapofsjpafjjaoajofsjoajfpajkfpojasfjposajfajfjsoapfjajfpojsaofjajfsajf");

                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in resultLogin.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                ProviderDisplayName = info.ProviderDisplayName;
                if (returnUrl == "/")
                {
                    returnUrl = "/"+user.Name;
                }
                //Console.WriteLine(returnUrl+"    shfisanfpajfoisaiofjsafjpsajfpoaojsajfjsjofajpofjsafjpfoajfoajfopsjafsaposajfjapofsjpafjjaoajofsjoajfpajkfpojasfjposajfajfjsoapfjajfpojsaofjajfsajf");
                ReturnUrl = returnUrl;
                return Page();
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
                return Page();
            }
            
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var usernameinput = info.Principal.Identity.Name;
            var emailinput = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (ModelState.IsValid)
            {
                UsernameTaken = Input.UsernameTakenIn;
                EmailTaken = Input.EmailTakenIn;
                var user = CreateUser();
                bool reload = false;
                if (Input.Username != null)
                {
                    var existingUserByName = _service.GetAuthorDtoByName(Input.Username);
                    if (existingUserByName.Name != null && existingUserByName.Email != null)
                    {
                        ModelState.AddModelError(string.Empty, "The username is already taken.");
                        reload = true;
                    }
                    else
                    {
                        usernameinput = Input.Username;
                    }
                    
                }
                else if (UsernameTaken)
                {
                    ModelState.AddModelError(string.Empty, "fill out username");
                    reload = true;
                }
                if (Input.Email != null)
                {
                    var existingUserByEmail = _service.GetAuthorDtoByEmail(Input.Email);
                    if (existingUserByEmail.Name != null && existingUserByEmail.Email != null)
                    {
                        ModelState.AddModelError(string.Empty, "The Email is already taken.");
                        reload = true;
                    }
                    else
                    {
                        emailinput = Input.Email;
                    }
                }else if (EmailTaken)
                {
                    ModelState.AddModelError(string.Empty, "fill out username");
                    reload = true;
                }
                
                if (reload)
                {
                    return Page();
                }
                user.Name = usernameinput;
                user.Cheeps = new List<Cheep>();
                user.Following = new List<Author>();
                user.Blocking = new List<Author>();
                //Console.WriteLine(info.Principal.Identity.Name + " " + info.Principal.FindFirstValue(ClaimTypes.Email));
                await _userStore.SetUserNameAsync(user, emailinput, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, emailinput, CancellationToken.None);

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                        if (returnUrl == "/")
                        {
                            returnUrl = "/"+usernameinput;
                        }
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ProviderDisplayName = info.ProviderDisplayName;
            if (returnUrl == "/")
            {
                returnUrl = "/"+usernameinput;
            }
            ReturnUrl = returnUrl;
            return Page();
        }

        private Author CreateUser()
        {
            try
            {
                return Activator.CreateInstance<Author>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(Author)}'. " +
                    $"Ensure that '{nameof(Author)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the external login page in /Areas/Identity/Pages/Account/ExternalLogin.cshtml");
            }
        }

        private IUserEmailStore<Author> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<Author>)_userStore;
        }
    }
}
