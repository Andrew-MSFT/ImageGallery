using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ImageGallery.Models;
using Microsoft.ProjectOxford.Face.Contract;

namespace ImageGallery.Services
{
    public interface IStorageService
    {
        Task<IEnumerable<Image>> GetImagesAsync();
        Task<Image> AddImageAsync(Stream stream, string fileName);
        //Task AddMetadataAsync(Image image, Face[] faces);
    }
}