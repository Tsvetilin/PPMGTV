﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using Common.Helpers;
using System;

namespace Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Полето за имейл е задължително.")]
            [EmailAddress(ErrorMessage = "Невалиден имейл адрес.")]
            [Display(Name = "Имейл")]
            public string Email { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Полето за парола е задължително.")]
            [StringLength(100, ErrorMessage = "Паролата трябда да е минимум {2} символа и максимум {1} символа дълга.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Парола")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Потвърди паролата")]
            [Compare("Password", ErrorMessage = "Паролите не съвпадат.")]
            public string ConfirmPassword { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Името е задължително.")]
            [StringLength(50,ErrorMessage = "Името трябда да е минимум {2} символа и максимум {1} символа дълга.", MinimumLength = 2)]
            [DisplayName("Име")]
            public string FullName { get; set; }

            /*[Required(AllowEmptyStrings = false, ErrorMessage = "Потребителското име е задължително.")]
            [MaxLength(50)]
            [DisplayName("Потребителско име")]
            public string UserName { get; set; }*/

            [Required]
            [DisplayName("Искам да получавам известия с актуалната информация по имейл (може да се промени по-късно)")]
            public bool IsNewsLetterSubscribed { get; set; }

            [GoogleReCaptchaValidation]
            public string RecaptchaValue { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var userName = Input.FullName.Trim().Replace(" ", "").ConvertCyrillicToLatinLetters();
                if( (await _userManager.FindByNameAsync(userName)) != null)
                {
                    userName = Input.Email;
                }

                var user = new ApplicationUser
                {
                    Email = Input.Email,
                    FullName = Input.FullName,
                    UserName = userName,
                    IsNewsLetterSubscriber = Input.IsNewsLetterSubscribed,
                    LockoutEnabled = true
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = UrlGenerator.GenerateUrl(
                        "account",
                        "confirmemail",
                        null, null,
                        "identity",
                        new Dictionary<string, string>
                        {
                            { "userId",user.Id },
                            { "code",code }
                        }
                     );
                    /*var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code, returnUrl },
                        protocol: "https");
                    */
                    await _emailSender.SendEmailAsync(Input.Email, "Потвърдете имейла си",
                        $"Моля потвърдете и активирайте акаунта си в PPMGTV.com като <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>натиснете тук</a>.");
 
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
