using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kepler.UI.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult IndexView()
        {
            return PartialView();
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

		public ActionResult SchedulerView()
        {
            return PartialView();
        }
    }
}