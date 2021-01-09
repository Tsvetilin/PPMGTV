using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [DisplayName("Потребителско име")]
        public string Username { get; set; }
        public string Email { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Име")]
            [Required]
            [MinLength(2,ErrorMessage ="Името може да бъде минимум 2 символа")]
            [MaxLength(50, ErrorMessage = "Името може да бъде максимум 50 символа")]
            public string FullName { get; set; }

            [Phone]
            [Display(Name = "Телефонен номер")]
            public string PhoneNumber { get; set; }

            [DisplayName("Искам да получавам известия с актуалната информация по имейл")]
            public bool IsNewsLetterSubscribed { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var email = await _userManager.GetEmailAsync(user);

            Username = userName;
            Email = email;
            Input = new InputModel
            {
                FullName = user.FullName,
                PhoneNumber = phoneNumber,
                IsNewsLetterSubscribed = user.IsNewsLetterSubscriber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            var isSub = user.IsNewsLetterSubscriber;
            if (Input.IsNewsLetterSubscribed != isSub)
            {
                user.IsNewsLetterSubscriber = Input.IsNewsLetterSubscribed;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set news letter subscription preference.";
                    return RedirectToPage();
                }
            }

            var fullNmae = user.FullName;
            if(Input.FullName!=fullNmae)
            {
                user.FullName = Input.FullName;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set full name.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
