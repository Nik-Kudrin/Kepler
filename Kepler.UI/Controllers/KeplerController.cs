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

        public ActionResult PartialBranchView()
        {
            return PartialView();
        }

        public ActionResult PartialBuildView()
        {
            ViewData["BuildId"] = Convert.ToInt32(RouteData.Values["BuildId"]);
            return PartialView();
        }
    }
}