using Common.Enums;
using Common.Helpers;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Gallery;

namespace Web.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IGalleryService galleryService;
        private readonly IImageService imageService;
        private readonly UserManager<ApplicationUser> userManager;

        public GalleryController(IGalleryService galleryService, IImageService imageService, UserManager<ApplicationUser> userManager)
        {
            this.galleryService = galleryService;
            this.imageService = imageService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var galleries = await this.galleryService.GetAllAsync<GalleryViewModel>();
            var viewModel = new GalleryIndexViewModel
            {
                Galleries=galleries.ToList(),
            };

            return this.View(viewModel);
        }

        [EditorAuthorization]
        public IActionResult Create()
        {
            return this.View();
        }


        [EditorAuthorization]
        [HttpPost]
        public async Task<IActionResult> Create(GalleryInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var images = await this.imageService.CreateImageListAsync(
                inputModel.ImagesUrls,
                ImageType.GalleryImage,
                inputModel.ImagesNote,
                inputModel.ImagesDescription
                );

            var gallery = await galleryService.CreateAsync(
                inputModel.Title,
                images,
                inputModel.PreDescription,
                inputModel.Description
                );

            SitemapFactory.AppendSitemapNode(UrlGenerator.GenerateGalleryUrl(gallery.Id, SlugGeneratorExtensions.GenerateSlug(gallery.Title)), DateTime.UtcNow);
            SitemapFactory.UpdateSitemap();

            return this.RedirectToAction(nameof(Watch), "Gallery", new { gallery.Id });
        }

        public async Task<IActionResult> Watch(string id, string slug)
        {
            var viewModel = await galleryService.GetByIdAsync<GalleryViewModel>(id);
            if (viewModel == null)
            {
                return NotFound();
            }

            return this.View(viewModel);
        }

        [EditorAuthorization]
        public async Task<IActionResult> Edit(string id)
        {
            var inputModel = await galleryService.GetByIdAsync<GalleryInputModel>(id);

            if (inputModel == null)
            {
                return this.NotFound();
            }

            return this.View(inputModel);
        }


        [EditorAuthorization]
        [HttpPost]
        public async Task<IActionResult> Edit(GalleryInputModel inputModel, string id)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var video = await galleryService.GetByIdAsync<GalleryViewModel>(id);
            if (video == null)
            {
                return this.NotFound();
            }

            var images = await this.imageService.UpdateImagesListAsync(
                inputModel.ImagesUrls,
                ImageType.GalleryImage, 
                inputModel.ImagesNote,
                inputModel.ImagesDescription
                );

            await galleryService.UpdateAsync(
                id,
                inputModel.Title,
                images,
                inputModel.PreDescription,
                inputModel.Description
                );

            return this.RedirectToAction(nameof(Watch), "Gallery", new { id });
        }

        [EditorAuthorization]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (!await galleryService.DeleteAsync(id))
            {
                return this.NotFound();
            }

            return this.RedirectToAction(nameof(Index));
        }
    }
}
