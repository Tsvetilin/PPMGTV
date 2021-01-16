using Common.Enums;
using Common.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts.Data;
using Services.Contracts.External;
using System;
using System.IO;
using System.Threading.Tasks;
using Web.Models.Image;

namespace Web.Controllers
{
    [EditorAuthorization]
    public class ImageController : Controller
    {
        private readonly ICloudinary cloudinary;
        private readonly IImageService imageService;

        public ImageController(ICloudinary cloudinary, IImageService imageService)
        {
            this.cloudinary = cloudinary;
            this.imageService = imageService;
        }

        public IActionResult Upload()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(ImageInputModel inputModel)
        {
            if (inputModel.PhotoUpload != null)
            {
                var timestamp = $"{DateTime.Today.Day}-{DateTime.Today.Month}-{DateTime.Today.Year}";
                var fileName = $"_{timestamp}_GalleryPhoto";

                IFormFile file = inputModel.PhotoUpload;

                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                var photoUrl = await cloudinary.UploadImageAsync(stream, fileName);
                if (photoUrl.StartsWith("Error"))
                {
                    ModelState.AddModelError("Снимка", $"Възникна грешка при качване на снимката: {photoUrl}. Ако не може да я разрешите, свържете се с администратор.");
                    return this.View(inputModel);
                }

                var image = await this.imageService.CreateAsync(photoUrl, ImageType.Unknow, null, null);

                return RedirectToAction(nameof(Preview), new { image.Id });
            }
            else
            {
                ModelState.AddModelError("Снимка", "Снимката е задължителна! Въведете връзка към снимка или качете такава.");
            }

            return this.View(inputModel);
        }

        public async Task<IActionResult> Preview(string id)
        {
            var viewModel = await this.imageService.GetByIdAsync<ImageViewModel>(id);
            return this.View(viewModel);
        }
    }
}
