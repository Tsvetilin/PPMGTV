using System.Linq;
using System.Threading.Tasks;
using Common.Constants;
using Common.Helpers;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.CronJobs;
using Services.Data;
using Web.Models.Contact;

namespace Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IContactsService contactsService;

        public ContactController(UserManager<ApplicationUser> userManager, IContactsService contactsService)
        {
            this.userManager = userManager;
            this.contactsService = contactsService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Send(ContactInputModel inputModel)
        {
            if(!ModelState.IsValid)
            {
                return this.View(nameof(Index), inputModel);
            }

            ApplicationUser user = null;
            if(User.Identity.IsAuthenticated)
            {
                user = await userManager.GetUserAsync(User);
            }

            var letter = await contactsService.CreateAsync(
                inputModel.SenderName,
                inputModel.SenderEmail,
                inputModel.About,
                inputModel.Description,
                inputModel.OtherContactInfo,
                user);

            JobManager.StartContactLetterJob(letter);

            this.TempData[TempDataParams.ContactLetterSendSuccessDataParam] = true;
            return this.RedirectToAction(nameof(Index));
        }

        [EditorAuthorization]
        public async Task<IActionResult> AdminView()
        {
            var letters = await contactsService.GetAllAsync<ContactViewModel>();

            var viewModel = new ContactAdminViewViewModel
            {
                ContactLetters = letters.ToList()
            };

            return this.View(viewModel);
        }

        [EditorAuthorization]
        public async Task<IActionResult> Detail(string id)
        {
            var viewModel = await contactsService.GetByIdAsync<ContactViewModel>(id);
            if(viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [EditorAuthorization]
        [HttpPost]
        public async Task<IActionResult> Detail([Bind("IsViewed","IsPinned")] ContactViewModel input, bool? isDeleted, string id)
        {
            await contactsService.UpdateAsync(id,isDeleted??false,input.IsPinned,input.IsViewed);

            return this.RedirectToAction(nameof(AdminView));
        }
    }
}
