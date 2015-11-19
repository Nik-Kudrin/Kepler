using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kepler.UI.Controllers
{
    public class KeplerController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PartialBranchView()
        {
            return PartialView();
        }

        public ActionResult PartialBuildView()
        {
            return PartialView();
        }

    }
}