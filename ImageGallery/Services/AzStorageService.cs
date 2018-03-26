using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using ImageGallery.Models;
using Microsoft.ProjectOxford.Face.Contract;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace ImageGallery.Services
{
    public class AzStorageService : IStorageService
    {
        private readonly CloudStorageAccount _account;
        private readonly CloudBlobClient _client;
        private readonly CloudBlobContainer _container;
        private readonly HttpClient _httpClient;
        private readonly string _imagePrefix = "img_";

        public AzStorageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            var connectionString = WebConfigurationManager.AppSettings["AzureStorageConnectionString"];

            _account = CloudStorageAccount.Parse(connectionString);
            _client = _account.CreateCloudBlobClient();
            _container = _client.GetContainerReference("images");
            _container.CreateIfNotExists();

            var permissions = _container.GetPermissions();
            if (permissions.PublicAccess == BlobContainerPublicAccessType.Off || permissions.PublicAccess == BlobContainerPublicAccessType.Unknown)
            {
                // If blob isn't public, we can't directly link to the pictures
                _container.SetPermissions(new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob });
            }
        }
        public async Task<Image> AddImageAsync(Stream stream, string fullFileName)
        {
            var fileName = _imagePrefix + Path.GetFileName(fullFileName);
            var imageBlob = _container.GetBlockBlobReference(fileName);
            await imageBlob.UploadFromStreamAsync(stream);

            return new Image()
            {
                FileName = fileName,
                ImagePath = imageBlob.Uri.ToString()
            };
        }

        

        public async Task<IEnumerable<Image>> GetImagesAsync()
        {
            var imageList = new List<Image>();
            var blobList = _container.ListBlobs(_imagePrefix);

            foreach (var blob in blobList)
            {
                var image = new Image
                {
                    ImagePath = blob.Uri.ToString()
                };

                imageList.Add(image);
            }

            return imageList;
        }
    }
}
