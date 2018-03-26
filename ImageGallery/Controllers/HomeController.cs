using ImageGallery.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ImageGallery.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStorageService _storageService;

        public HomeController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public async Task<ActionResult> Index()
        {
            var images = await _storageService.GetImagesAsync();
            return View(images);
        }

        [HttpPost]
        public async Task<ActionResult> Upload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return RedirectToAction("Index", new { value = "uploadFailure" });
            }

            try
            {
                var image = await _storageService.AddImageAsync(file.InputStream, file.FileName);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", new { value = "uploadFailure" });
            }
            
            return RedirectToAction("Index", new { value = "uploadSuccess" });
        }
    }
}