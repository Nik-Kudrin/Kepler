using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kepler.UI.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImageWorkersView()
        {
            return PartialView();
        }

        public ActionResult SourceImagePathView()
        {
            return PartialView();
        }

        public ActionResult ServiceUrlView()
        {
            return PartialView();
        }
    }
}