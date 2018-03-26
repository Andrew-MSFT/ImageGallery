using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageGallery.Controllers
{
    public class FailController : Controller
    {
        // GET: Fail
        public ActionResult Index()
        {
            throw new InvalidOperationException("Some message goes here");
            return View();
        }
    }
}