using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ImageGallery.Models;
using Microsoft.ProjectOxford.Face.Contract;

namespace ImageGallery.Services
{
    public class LocalStorageService : IStorageService
    {
        string BasePath = HttpContext.Current.Server.MapPath("~");
        string RelativeCachePath => "Images";
        string FileCacheDir => this.BasePath + $"{RelativeCachePath}\\";

        public LocalStorageService()
        {
            if (!Directory.Exists(FileCacheDir))
            {
                Directory.CreateDirectory(FileCacheDir);
            }
        }

        public async Task<Image> AddImageAsync(Stream stream, string fullFileName)
        {
            var fileName = Path.GetFileName(fullFileName);
            var filePath = $"{FileCacheDir}{fileName}";

            using (var writer = File.Create(filePath))
            {
                await stream.CopyToAsync(writer);
            }

            return new Image()
            {
                FileName = fileName,
                ImagePath = $"{RelativeCachePath}/{fileName}"
            };
        }

        public async Task<IEnumerable<Image>> GetImagesAsync()
        {
            var files = Directory.EnumerateFiles(FileCacheDir);
            var imageList = new List<Image>();

            foreach (var file in files)
            {
                var fileName = file.Substring(file.LastIndexOf("\\") + 1);
                var imageUriString = HttpContext.Current.Request.Url.AbsoluteUri + $"{RelativeCachePath}/{fileName}";
                var image = new Image
                {
                    FileName = fileName,
                    ImagePath = $"{RelativeCachePath}/{fileName}"
                };

                imageList.Add(image);
            }

            return imageList;
        }
    }
}