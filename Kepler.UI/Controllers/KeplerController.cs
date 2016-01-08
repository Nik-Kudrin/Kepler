using System;
using System.Web.Mvc;

namespace Kepler.UI.Controllers
{
    public class KeplerController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LeftMenu()
        {
            return PartialView();
        }

        public ActionResult PartialBranchView()
        {
            return PartialView();
        }

        public ActionResult PartialBuildView()
        {
            return PartialView();
        }

        public ActionResult ErrorLogView()
        {
            return PartialView();
        }
    }
}