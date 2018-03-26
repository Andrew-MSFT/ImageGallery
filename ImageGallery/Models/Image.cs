using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageGallery.Models
{
    public class Image
    {
        public string FileName { get; set; }
        //public Guid ImageGuid { get; set; }
        public string ImagePath { get; set; }
        public ICollection<Face> FaceAttributes { get; set; }
    }
}