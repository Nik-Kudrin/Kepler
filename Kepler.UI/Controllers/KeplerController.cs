using System;
using System.Dynamic;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kepler.UI.Controllers
{
    public class KeplerController : Controller
    {
        protected dynamic model = new ExpandoObject();

        protected void FillModelData()
        {
            model.ProjectName = RouteData.Values["project"];
            model.BranchName = RouteData.Values["branch"];
            model.BuildId = RouteData.Values["build"];
        }

        public ActionResult Index()
        {
            FillModelData();
            return View(model);
        }

        public ActionResult PartialBranchView()
        {
            ViewData["ProjectName"] = Request.QueryString["project"];
            ViewData["BranchName"] = Request.QueryString["branch"];

            return PartialView(model);
        }

        public ActionResult PartialBuildView()
        {
            /* ViewData["BuildId"] = RouteData.Values["BuildId"];

            if (buildId != null)
                ViewData["BuildId"] = buildId;*/
//            Index();

            return PartialView(model);
        }

/*        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            FillModelData();
            base.OnActionExecuting(filterContext);
        }*/
    }
}