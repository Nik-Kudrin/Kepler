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

        public ActionResult PartialBuildView(int? buildId)
        {
            ViewData["BuildId"] = RouteData.Values["BuildId"];

            if (buildId != null)
                ViewData["BuildId"] = buildId;

            return PartialView();
        }
    }
}